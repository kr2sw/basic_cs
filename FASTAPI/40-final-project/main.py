import asyncio
import logging
import time
import uuid
from collections import defaultdict, deque
from contextlib import asynccontextmanager
from typing import Literal, Optional

from fastapi import Depends, FastAPI, HTTPException, Query, Request, status
from fastapi.security import OAuth2PasswordBearer, OAuth2PasswordRequestForm
from fastapi.responses import JSONResponse
from jose import JWTError, jwt
from passlib.context import CryptContext
from pydantic import BaseModel
from sqlalchemy import String, create_engine, select
from sqlalchemy.orm import DeclarativeBase, Mapped, Session, mapped_column, sessionmaker

# ---------- 설정 ----------
SECRET_KEY = "final-project-secret-key"  # 운영에서는 환경 변수로 관리
ALGORITHM = "HS256"
ACCESS_TOKEN_EXPIRE = 30 * 60
RATE_LIMIT = int(__import__("os").getenv("RATE_LIMIT", "60"))
RATE_PERIOD = 60

logging.basicConfig(level=logging.INFO)
log = logging.getLogger("final-project")

# ---------- DB ----------
engine = create_engine("sqlite:///./shop.db", connect_args={"check_same_thread": False})
SessionLocal = sessionmaker(bind=engine, autoflush=False, autocommit=False)


class Base(DeclarativeBase):
    pass


class User(Base):
    __tablename__ = "users"
    id: Mapped[int] = mapped_column(primary_key=True)
    username: Mapped[str] = mapped_column(String(50), unique=True)
    hashed_password: Mapped[str] = mapped_column(String(200))
    role: Mapped[str] = mapped_column(String(20), default="user")
    is_active: Mapped[bool] = mapped_column(default=True)


class Product(Base):
    __tablename__ = "products"
    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str] = mapped_column(String(100))
    price: Mapped[int]
    stock: Mapped[int] = mapped_column(default=0)


# ---------- 인증 ----------
pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="auth/login")


def hash_password(pw: str) -> str:
    return pwd_context.hash(pw)


def verify_password(pw: str, hashed: str) -> bool:
    return pwd_context.verify(pw, hashed)


def create_token(username: str, role: str) -> str:
    payload = {"sub": username, "role": role, "exp": int(time.time()) + ACCESS_TOKEN_EXPIRE}
    return jwt.encode(payload, SECRET_KEY, algorithm=ALGORITHM)


# ---------- 스키마 ----------
class UserCreate(BaseModel):
    username: str
    password: str
    role: Literal["admin", "user"] = "user"


class UserOut(BaseModel):
    id: int
    username: str
    role: str

    class Config:
        from_attributes = True


class Token(BaseModel):
    access_token: str
    token_type: str = "bearer"


class ProductCreate(BaseModel):
    name: str
    price: int
    stock: int = 0


class ProductOut(BaseModel):
    id: int
    name: str
    price: int
    stock: int

    class Config:
        from_attributes = True


class Page(BaseModel):
    items: list[ProductOut]
    total: int
    page: int
    page_size: int
    pages: int


# ---------- 백그라운드 작업 큐 ----------
job_queue: asyncio.Queue = asyncio.Queue()
jobs: dict[str, dict] = {}


def send_order_notification(order_id: str, username: str):
    """주문 알림 전송 (실제로는 이메일/푸시)"""
    time.sleep(1)
    log.info("order notification sent", order_id=order_id, username=username)
    return f"주문 {order_id} 알림 전송 완료"


async def job_worker():
    while True:
        job_id = await job_queue.get()
        try:
            job = jobs[job_id]
            job["status"] = "running"
            result = await asyncio.to_thread(send_order_notification, job["order_id"], job["username"])
            job["status"] = "done"
            job["result"] = result
        except Exception as e:
            jobs[job_id]["status"] = "failed"
            jobs[job_id]["error"] = str(e)
        finally:
            job_queue.task_done()


@asynccontextmanager
async def lifespan(app: FastAPI):
    # DB 초기화 (운영에서는 Alembic 사용) + 워커 기동
    Base.metadata.create_all(bind=engine)
    with SessionLocal() as db:
        if db.execute(select(User)).first() is None:
            db.add(User(username="admin", hashed_password=hash_password("admin123"), role="admin"))
            db.commit()
    worker = asyncio.create_task(job_worker())
    yield
    worker.cancel()


# ---------- 속도 제한 미들웨어 ----------
class SlidingWindowLimiter:
    def __init__(self, limit: int, period: int):
        self.limit = limit
        self.period = period
        self.hits: dict[str, deque] = defaultdict(deque)

    def allow(self, key: str) -> bool:
        now = time.monotonic()
        dq = self.hits[key]
        while dq and now - dq[0] > self.period:
            dq.popleft()
        if len(dq) >= self.limit:
            return False
        dq.append(now)
        return True


class RateLimitMiddleware:
    def __init__(self, app, limiter, exempt=()):
        self.app = app
        self.limiter = limiter
        self.exempt = set(exempt)

    async def __call__(self, scope, receive, send):
        if scope["type"] != "http":
            await self.app(scope, receive, send)
            return
        request = Request(scope)
        if request.url.path in self.exempt:
            await self.app(scope, receive, send)
            return
        client_ip = request.client.host if request.client else "unknown"
        if not self.limiter.allow(client_ip):
            await JSONResponse(429, {"detail": "요청이 너무 많습니다"})(scope, receive, send)
            return
        await self.app(scope, receive, send)


limiter = SlidingWindowLimiter(RATE_LIMIT, RATE_PERIOD)

# ---------- 앱 ----------
app = FastAPI(
    title="프로덕션급 종합 API",
    description="인증(RBAC), DB, 페이지네이션, 속도 제한, 백그라운드 작업을 하나로 통합한 예제",
    version="1.0.0",
    lifespan=lifespan,
    openapi_tags=[
        {"name": "auth", "description": "회원가입/로그인"},
        {"name": "products", "description": "상품 조회/관리"},
        {"name": "orders", "description": "주문 생성과 상태"},
        {"name": "system", "description": "상태/작업 확인"},
    ],
)
app.add_middleware(RateLimitMiddleware, limiter=limiter, exempt={"/health", "/docs", "/redoc", "/openapi.json"})


def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()


# ---------- 인증 의존성 ----------
def get_current_user(token: str = Depends(oauth2_scheme), db: Session = Depends(get_db)) -> User:
    try:
        payload = jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
    except JWTError:
        raise HTTPException(status.HTTP_401_UNAUTHORIZED, "토큰이 유효하지 않습니다")
    user = db.execute(select(User).where(User.username == payload["sub"])).scalar_one_or_none()
    if user is None or not user.is_active:
        raise HTTPException(status.HTTP_401_UNAUTHORIZED, "인증이 필요합니다")
    return user


def require_admin(user: User = Depends(get_current_user)) -> User:
    if user.role != "admin":
        raise HTTPException(status.HTTP_403_FORBIDDEN, "관리자 권한이 필요합니다")
    return user


# ---------- 인증 API ----------
@app.post("/auth/register", response_model=UserOut, tags=["auth"])
def register(data: UserCreate, db: Session = Depends(get_db)):
    if db.execute(select(User).where(User.username == data.username)).first():
        raise HTTPException(status.HTTP_400_BAD_REQUEST, "이미 존재하는 사용자입니다")
    user = User(username=data.username, hashed_password=hash_password(data.password), role=data.role)
    db.add(user)
    db.commit()
    db.refresh(user)
    return user


@app.post("/auth/login", response_model=Token, tags=["auth"])
def login(form: OAuth2PasswordRequestForm = Depends(), db: Session = Depends(get_db)):
    user = db.execute(select(User).where(User.username == form.username)).scalar_one_or_none()
    if not user or not verify_password(form.password, user.hashed_password):
        raise HTTPException(status.HTTP_401_UNAUTHORIZED, "아이디 또는 비밀번호가 올바르지 않습니다")
    return Token(access_token=create_token(user.username, user.role))


@app.get("/users/me", response_model=UserOut, tags=["auth"])
def read_me(user: User = Depends(get_current_user)):
    return user


# ---------- 상품 API ----------
@app.get("/products", response_model=Page, tags=["products"])
def list_products(
    page: int = Query(1, ge=1),
    page_size: int = Query(10, ge=1, le=50),
    search: Optional[str] = Query(None),
    db: Session = Depends(get_db),
):
    """페이지네이션 + 검색이 결합된 목록 조회"""
    stmt = select(Product)
    if search:
        stmt = stmt.where(Product.name.contains(search))
    total = len(db.execute(stmt).scalars().all())
    products = db.execute(stmt.offset((page - 1) * page_size).limit(page_size)).scalars().all()
    pages = (total + page_size - 1) // page_size
    return Page(items=products, total=total, page=page, page_size=page_size, pages=pages)


@app.post("/products", response_model=ProductOut, tags=["products"])
def create_product(data: ProductCreate, db: Session = Depends(get_db), _: User = Depends(require_admin)):
    product = Product(**data.model_dump())
    db.add(product)
    db.commit()
    db.refresh(product)
    return product


# ---------- 주문 API ----------
@app.post("/orders", status_code=202, tags=["orders"])
def create_order(product_id: int, quantity: int = Query(1, ge=1), db: Session = Depends(get_db),
                 user: User = Depends(get_current_user)):
    """주문 생성 + 백그라운드로 알림 작업 예약"""
    product = db.get(Product, product_id)
    if product is None:
        raise HTTPException(status.HTTP_404_NOT_FOUND, "상품을 찾을 수 없습니다")
    if product.stock < quantity:
        raise HTTPException(status.HTTP_409_CONFLICT, "재고가 부족합니다")
    product.stock -= quantity
    db.commit()

    job_id = uuid.uuid4().hex
    jobs[job_id] = {"id": job_id, "order_id": job_id[:8], "username": user.username, "status": "queued"}
    asyncio.create_task(job_queue.put(job_id))
    log.info("order created", user=user.username, product=product_id, qty=quantity)
    return {"message": "주문 접수 완료", "job_id": job_id}


@app.get("/orders/jobs", tags=["orders"])
def list_jobs(user: User = Depends(get_current_user)):
    """접수된 주문 작업의 상태 확인"""
    return [{"id": j["id"], "status": j["status"], "result": j.get("result"), "error": j.get("error")}
            for j in jobs.values() if j["username"] == user.username]


# ---------- 시스템 ----------
@app.get("/health", tags=["system"])
def health():
    return {"status": "ok", "jobs_queued": job_queue.qsize()}

from contextlib import asynccontextmanager

from fastapi import Depends, FastAPI
from pydantic import BaseModel
from sqlalchemy import String, create_engine
from sqlalchemy.orm import DeclarativeBase, Mapped, Session, mapped_column, sessionmaker

DATABASE_URL = "sqlite:///./alembic.db"
engine = create_engine(DATABASE_URL, connect_args={"check_same_thread": False})
SessionLocal = sessionmaker(bind=engine, autoflush=False, autocommit=False)


class Base(DeclarativeBase):
    pass


# 스키마는 Alembic revision으로 관리된다.
# 아래 모델을 수정한 뒤에는:
#   alembic revision --autogenerate -m "add column"
#   alembic upgrade head
class User(Base):
    __tablename__ = "users"

    id: Mapped[int] = mapped_column(primary_key=True)
    email: Mapped[str] = mapped_column(String(120), unique=True)
    nickname: Mapped[str] = mapped_column(String(50), default="")
    is_active: Mapped[bool] = mapped_column(default=True)


class UserCreate(BaseModel):
    email: str
    nickname: str = ""


class UserOut(BaseModel):
    id: int
    email: str
    nickname: str
    is_active: bool

    class Config:
        from_attributes = True


@asynccontextmanager
async def lifespan(app: FastAPI):
    # 운영에서는 Alembic이 스키마를 관리한다.
    # 데모용으로만 create_all을 사용한다.
    Base.metadata.create_all(bind=engine)
    yield


app = FastAPI(lifespan=lifespan, title="Alembic 데모")


def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()


@app.post("/users", response_model=UserOut)
def create_user(data: UserCreate, db: Session = Depends(get_db)):
    user = User(email=data.email, nickname=data.nickname)
    db.add(user)
    db.commit()
    db.refresh(user)
    return user


@app.get("/users", response_model=list[UserOut])
def list_users(db: Session = Depends(get_db)):
    return db.query(User).all()

from contextlib import asynccontextmanager

from fastapi import Depends, FastAPI, HTTPException
from pydantic import BaseModel
from sqlalchemy import ForeignKey, String, select
from sqlalchemy.ext.asyncio import AsyncSession, async_sessionmaker, create_async_engine
from sqlalchemy.orm import DeclarativeBase, Mapped, mapped_column, relationship, selectinload

DATABASE_URL = "sqlite+aiosqlite:///./app.db"

# 비동기 엔진: aiosqlite 드라이버를 사용해 blocking 없이 DB 접근
engine = create_async_engine(DATABASE_URL, echo=False)
SessionLocal = async_sessionmaker(engine, expire_on_commit=False)


class Base(DeclarativeBase):
    pass


# 관계(relationship): User 1 <-> Post N
class User(Base):
    __tablename__ = "users"

    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str] = mapped_column(String(50), unique=True)
    posts: Mapped[list["Post"]] = relationship(
        back_populates="author", cascade="all, delete-orphan"
    )


class Post(Base):
    __tablename__ = "posts"

    id: Mapped[int] = mapped_column(primary_key=True)
    title: Mapped[str] = mapped_column(String(100))
    user_id: Mapped[int] = mapped_column(ForeignKey("users.id"))
    author: Mapped["User"] = relationship(back_populates="posts")


# ---- Pydantic 스키마 (from_attributes로 ORM 객체 직렬화) ----
class UserCreate(BaseModel):
    name: str


class PostCreate(BaseModel):
    title: str


class PostOut(BaseModel):
    id: int
    title: str
    user_id: int

    class Config:
        from_attributes = True


class UserOut(BaseModel):
    id: int
    name: str
    posts: list[PostOut] = []

    class Config:
        from_attributes = True


async def init_db():
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)


@asynccontextmanager
async def lifespan(app: FastAPI):
    await init_db()
    yield
    await engine.dispose()  # 종료 시 연결 풀 정리


app = FastAPI(lifespan=lifespan, title="SQLAlchemy 고급 - 관계/조인/비동기")


async def get_db() -> AsyncSession:
    async with SessionLocal() as session:
        yield session


@app.post("/users", response_model=UserOut)
async def create_user(data: UserCreate, db: AsyncSession = Depends(get_db)):
    user = User(name=data.name)
    db.add(user)
    await db.commit()
    # 새 사용자의 posts 관계를 eager loading으로 다시 조회
    stmt = select(User).where(User.id == user.id).options(selectinload(User.posts))
    result = await db.execute(stmt)
    return result.scalar_one()


@app.post("/users/{user_id}/posts", response_model=PostOut)
async def create_post(user_id: int, data: PostCreate, db: AsyncSession = Depends(get_db)):
    user = await db.get(User, user_id)
    if user is None:
        raise HTTPException(status_code=404, detail="사용자를 찾을 수 없습니다")
    post = Post(title=data.title, user_id=user_id)
    db.add(post)
    await db.commit()
    await db.refresh(post)
    return post


@app.get("/users", response_model=list[UserOut])
async def list_users(db: AsyncSession = Depends(get_db)):
    """selectinload: N+1 문제를 피하며 관계(포스트)를 한 번에 조회"""
    result = await db.execute(select(User).options(selectinload(User.posts)))
    return result.scalars().all()


@app.get("/users/{user_id}/posts", response_model=list[PostOut])
async def list_user_posts(user_id: int, db: AsyncSession = Depends(get_db)):
    """조인(join): Post와 User 테이블을 조인해 특정 사용자의 글만 조회"""
    stmt = select(Post).join(User).where(User.id == user_id)
    result = await db.execute(stmt)
    return result.scalars().all()


@app.get("/users/{user_id}/stats")
async def user_stats(user_id: int, db: AsyncSession = Depends(get_db)):
    """관계를 따라 작성자 정보까지 한 번에 조회"""
    stmt = select(Post).join(User).where(User.id == user_id)
    posts = (await db.execute(stmt)).scalars().all()
    return {"user_id": user_id, "post_count": len(posts), "total_chars": sum(len(p.title) for p in posts)}

# 22: SQLAlchemy 고급 — 관계(relationship), 조인, 비동기 엔진

기초 챕터 10에서 다룬 기본 CRUD를 넘어, 실제 서비스에서 필수인 **관계(relationship)**, **조인(join)**, 그리고 고성능 **비동기 엔진**을 다룹니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

## 주요 개념

### 관계(relationship) 정의

`relationship()`을 모델 양쪽에 정의하면 두 테이블이 하나의 객체 그래프처럼 다뤄집니다. `cascade="all, delete-orphan"`은 부모 삭제 시 자식까지 함께 삭제하도록 합니다.

```python
class User(Base):
    __tablename__ = "users"
    id: Mapped[int] = mapped_column(primary_key=True)
    posts: Mapped[list["Post"]] = relationship(back_populates="author")

class Post(Base):
    __tablename__ = "posts"
    user_id: Mapped[int] = mapped_column(ForeignKey("users.id"))
    author: Mapped["User"] = relationship(back_populates="posts")
```

### N+1 문제와 selectinload

사용자 목록을 조회한 뒤 각 사용자의 글을 하나씩 추가 조회하면 **N+1 쿼리** 문제가 생깁니다. `selectinload`로 관계를 미리 로딩(eager loading)하면 쿼리가 2번으로 줄어듭니다.

```python
select(User).options(selectinload(User.posts))
```

### 조인(join)

두 테이블을 조인해 조건에 맞는 행만 가져옵니다.

```python
select(Post).join(User).where(User.id == user_id)
```

### 비동기 엔진

`create_async_engine` + `aiosqlite`를 쓰면 요청 처리 동안 DB 작업이 이벤트 루프를 막지 않습니다. 엔드포인트는 `async def`로 작성하고 `await db.execute(...)`로 실행합니다.

```python
engine = create_async_engine(DATABASE_URL)
SessionLocal = async_sessionmaker(engine, expire_on_commit=False)
```

`expire_on_commit=False`는 커밋 뒤에도 객체 속성 접근을 가능하게 해 응답 직렬화를 안전하게 만듭니다.

### 비동기 주의점

- `db.query()`는 동기 API라 사용 불가 → **`select()` 문을 `db.execute()`로 실행**합니다.
- relationship lazy load는 비동기에서 예외를 던지므로 **사용 전에 반드시 eager loading**합니다.

## 연습

1. `Comment`(댓글) 모델을 `Post`에 관계로 추가하고 댓글 수를 응답에 포함해 보세요.
2. `create_user`에서 `expire_on_commit=False`를 제거하면 어떤 문제가 생기는지 확인해 보세요.

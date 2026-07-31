import time
from typing import Literal

from fastapi import Depends, FastAPI, HTTPException, status
from fastapi.security import OAuth2PasswordBearer, OAuth2PasswordRequestForm
from jose import JWTError, jwt
from passlib.context import CryptContext
from pydantic import BaseModel

# 실제 서비스에서는 반드시 환경 변수로 관리할 것!
SECRET_KEY = "intermediate-course-secret-key"
ACCESS_TOKEN_EXPIRE = 15 * 60           # 액세스 토큰: 15분
REFRESH_TOKEN_EXPIRE = 7 * 24 * 60 * 60  # 리프레시 토큰: 7일
ALGORITHM = "HS256"

pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="auth/login")

# RBAC(Role Based Access Control): 역할 정의
Role = Literal["admin", "moderator", "user"]
ROLE_LEVEL = {"admin": 3, "moderator": 2, "user": 1}

# 리프레시 토큰 저장소 (프로덕션에서는 Redis/DB 사용)
# token -> {"username", "expires_at"}
refresh_token_store: dict[str, dict] = {}

# 데모용 사용자 DB
fake_users_db: dict[str, dict] = {
    "admin": {"username": "admin", "role": "admin"},
    "alice": {"username": "alice", "role": "user"},
    "bob": {"username": "bob", "role": "moderator"},
}
# 비밀번호는 전부 "password123" (데모용)
for _u in fake_users_db.values():
    _u["hashed_password"] = pwd_context.hash("password123")


class RegisterIn(BaseModel):
    username: str
    password: str
    role: Role = "user"


class UserOut(BaseModel):
    username: str
    role: Role


class TokenPair(BaseModel):
    access_token: str
    refresh_token: str
    token_type: str = "bearer"


class RefreshIn(BaseModel):
    refresh_token: str


# ---- JWT 헬퍼 ----
def create_token(username: str, role: str, expires_in: int) -> str:
    payload = {
        "sub": username,
        "role": role,
        "exp": int(time.time()) + expires_in,
    }
    return jwt.encode(payload, SECRET_KEY, algorithm=ALGORITHM)


def decode_token(token: str) -> dict | None:
    try:
        return jwt.decode(token, SECRET_KEY, algorithms=[ALGORITHM])
    except JWTError:
        return None


app = FastAPI(title="고급 보안 - RBAC + 리프레시 토큰")


# ---- 의존성 ----
def get_current_user(token: str = Depends(oauth2_scheme)) -> dict:
    payload = decode_token(token)
    if payload is None:
        raise HTTPException(status.HTTP_401_UNAUTHORIZED, "토큰이 유효하지 않습니다")
    user = fake_users_db.get(payload.get("sub"))
    if user is None:
        raise HTTPException(status.HTTP_401_UNAUTHORIZED, "사용자를 찾을 수 없습니다")
    return user


def require_role(*roles: Role):
    """지정된 역할만 허용하는 의존성 팩토리 (RBAC)"""
    def checker(user: dict = Depends(get_current_user)) -> dict:
        if user["role"] not in roles:
            raise HTTPException(status.HTTP_403_FORBIDDEN, "권한이 없습니다")
        return user
    return checker


# ---- 인증 API ----
@app.post("/auth/register", response_model=UserOut)
def register(data: RegisterIn):
    if data.username in fake_users_db:
        raise HTTPException(status.HTTP_400_BAD_REQUEST, "이미 존재하는 사용자입니다")
    fake_users_db[data.username] = {
        "username": data.username,
        "role": data.role,
        "hashed_password": pwd_context.hash(data.password),
    }
    return UserOut(username=data.username, role=data.role)


@app.post("/auth/login", response_model=TokenPair)
def login(form: OAuth2PasswordRequestForm = Depends()):
    user = fake_users_db.get(form.username)
    if not user or not pwd_context.verify(form.password, user["hashed_password"]):
        raise HTTPException(status.HTTP_401_UNAUTHORIZED, "아이디 또는 비밀번호가 올바르지 않습니다")

    access = create_token(user["username"], user["role"], ACCESS_TOKEN_EXPIRE)
    refresh = create_token(user["username"], user["role"], REFRESH_TOKEN_EXPIRE)
    # 리프레시 토큰을 서버 저장소에 기록 (무효화 가능 -> 보안 강화)
    refresh_token_store[refresh] = {
        "username": user["username"],
        "expires_at": int(time.time()) + REFRESH_TOKEN_EXPIRE,
    }
    return TokenPair(access_token=access, refresh_token=refresh)


@app.post("/auth/refresh", response_model=TokenPair)
def refresh(data: RefreshIn):
    """액세스 토큰 만료 시 리프레시 토큰으로 새 토큰 쌍 발급"""
    entry = refresh_token_store.get(data.refresh_token)
    if entry is None:
        raise HTTPException(status.HTTP_401_UNAUTHORIZED, "리프레시 토큰이 유효하지 않습니다")
    if entry["expires_at"] < int(time.time()):
        refresh_token_store.pop(data.refresh_token, None)
        raise HTTPException(status.HTTP_401_UNAUTHORIZED, "리프레시 토큰이 만료되었습니다")

    user = fake_users_db[entry["username"]]
    # 리프레시 토큰 재사용 방지: 기존 토큰 무효화 후 새로 발급
    refresh_token_store.pop(data.refresh_token, None)
    return login_pair(user)


def login_pair(user: dict) -> TokenPair:
    access = create_token(user["username"], user["role"], ACCESS_TOKEN_EXPIRE)
    refresh = create_token(user["username"], user["role"], REFRESH_TOKEN_EXPIRE)
    refresh_token_store[refresh] = {
        "username": user["username"],
        "expires_at": int(time.time()) + REFRESH_TOKEN_EXPIRE,
    }
    return TokenPair(access_token=access, refresh_token=refresh)


@app.post("/auth/logout")
def logout(data: RefreshIn):
    """리프레시 토큰을 저장소에서 제거해 무효화"""
    refresh_token_store.pop(data.refresh_token, None)
    return {"message": "로그아웃 완료"}


# ---- 보호된 엔드포인트 ----
@app.get("/users/me", response_model=UserOut)
def read_me(user: dict = Depends(get_current_user)):
    return UserOut(username=user["username"], role=user["role"])


@app.get("/posts", response_model=list[dict])
def list_posts(user: dict = Depends(get_current_user)):
    """로그인한 사용자면 누구나 조회 가능"""
    return [{"id": 1, "title": "공개 게시글"}]


@app.post("/posts/{post_id}/moderate")
def moderate_post(post_id: int, user: dict = Depends(require_role("admin", "moderator"))):
    """admin, moderator만 가능"""
    return {"message": f"게시글 {post_id} 검토 완료 by {user['username']}"}


@app.delete("/users/{username}")
def delete_user(username: str, user: dict = Depends(require_role("admin"))):
    """admin만 가능"""
    fake_users_db.pop(username, None)
    return {"message": f"{username} 삭제 완료"}

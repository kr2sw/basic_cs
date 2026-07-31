# 24: 고급 보안 — 역할(RBAC), 리프레시 토큰, 토큰 저장

기초 챕터 11에서 JWT 로그인을 다뤘습니다. 이번엔 실제 서비스 수준의 보안 패턴인 **RBAC(역할 기반 접근 제어)**, **리프레시 토큰**, **토큰 저장 방식**을 구현합니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

모든 사용자의 비밀번호는 데모용으로 `password123`입니다. (`alice`, `bob`, `admin`)

## 주요 개념

### 왜 액세스 토큰이 짧아야 하나?

JWT는 서버가 상태를 저장하지 않아 **한번 발급되면 만료 전까지는 취소가 어렵습니다.** 탈취 피해를 줄이기 위해 액세스 토큰은 짧게(15분) 두고, 갱신은 리프레시 토큰으로 합니다.

```python
ACCESS_TOKEN_EXPIRE = 15 * 60
REFRESH_TOKEN_EXPIRE = 7 * 24 * 60 * 60
```

### 리프레시 토큰 + 서버 저장

리프레시 토큰은 서버 저장소에 기록해 **로그아웃/탈취 시 무효화**할 수 있습니다. 저장소는 프로덕션에서 Redis나 DB를 사용하고, 토큰 자체 대신 사용자-디바이스 매핑을 저장합니다.

```python
refresh_token_store[refresh] = {
    "username": username,
    "expires_at": int(time.time()) + REFRESH_TOKEN_EXPIRE,
}
```

재사용 방지 패턴: `/auth/refresh` 호출 시 기존 리프레시 토큰을 즉시 무효화하고 새 토큰을 발급합니다.

### RBAC (역할 기반 접근 제어)

역할(`admin` > `moderator` > `user`)을 정의하고, 의존성 팩토리로 엔드포인트마다 접근 권한을 선언합니다.

```python
def require_role(*roles):
    def checker(user=Depends(get_current_user)):
        if user["role"] not in roles:
            raise HTTPException(403, "권한이 없습니다")
        return user
    return checker

@app.post("/posts/{id}/moderate")
def moderate(post_id: int, user=Depends(require_role("admin", "moderator"))):
    ...
```

### 토큰 저장 위치 (클라이언트)

- **localStorage**: XSS 공격에 취약하므로 사용 금지.
- **쿠키(httponly + secure + samesite=lax)**: XSS에는 안전하지만 CSRF 대비 필요.
- **메모리 저장**: SPA에서 가장 안전하지만 새로고침 시 로그인 유실.

## 연습

1. `require_role`을 정수 레벨 비교(예: `role_level >= 2`) 방식으로 바꿔 계층적 권한을 만들어 보세요.
2. 리프레시 토큰 저장소를 Redis로 교체하면 어떤 변화가 필요한지 정리해 보세요.

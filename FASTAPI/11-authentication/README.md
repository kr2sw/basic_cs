# 11: 인증 — OAuth2, JWT

## 설치

```bash
pip install python-jose[cryptography] passlib[bcrypt]
```

## 실행

```bash
uvicorn main:app --reload
```

POST /auth/register - 회원가입
POST /auth/login - 로그인 (JWT 토큰 발급)
GET /users/me - 현재 사용자 정보

## 주요 개념

- **OAuth2PasswordBearer**: 토큰 인증 스키마
- **JWT (JSON Web Token)**: 토큰 기반 인증
- **passlib**: 비밀번호 해싱 (bcrypt)
- **의존성 주입**: get_current_user로 보호된 엔드포인트

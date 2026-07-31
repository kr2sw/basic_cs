# 40: 프로덕션급 API — 종합 프로젝트

중급 과정에서 배운 내용을 **하나의 쇼핑몰 API**로 종합합니다. 인증, 데이터베이스, 페이지네이션, 속도 제한, 백그라운드 작업, 로깅, OpenAPI 메타데이터가 모두 함께 동작합니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

최초 실행 시 `admin` / `admin123` 관리자 계정이 생성됩니다.

## 주요 엔드포인트

| 메서드 | 경로 | 설명 |
|--------|------|------|
| POST | /auth/register | 회원가입 (role: admin/user) |
| POST | /auth/login | 로그인 (JWT 발급) |
| GET | /users/me | 내 정보 |
| GET | /products | 상품 목록 (페이지네이션 + 검색) |
| POST | /products | 상품 등록 (**admin만**) |
| POST | /orders?product_id=&quantity= | 주문 생성 + 알림 작업 예약 |
| GET | /orders/jobs | 내 주문 작업 상태 |
| GET | /health | 상태 확인 |

## 아키텍처 요약

```
클라이언트
   │  Bearer JWT
   ▼
RateLimitMiddleware ── IP당 N회/분 제한 (429)
   ▼
인증 의존성 ── get_current_user / require_admin (RBAC)
   ▼
라우터 ── 페이지네이션, 검증(Pydantic)
   ▼
SQLAlchemy(SQLite) ── User, Product
   ▼
백그라운드 큐 ── 주문 알림 작업 (asyncio.Queue + worker)
```

## 이번 과정에서 통합한 기술

- **Pydantic v2**: `Literal`, `Query(ge=1)`, `from_attributes` 응답 스키마 (챕터 21)
- **SQLAlchemy**: 모델, 세션 의존성, `select()` 쿼리 (챕터 22)
- **보안**: JWT + passlib, RBAC `require_admin` (챕터 24)
- **페이지네이션**: `Page` 응답 모델 + 검색 (챕터 25)
- **속도 제한**: 슬라이딩 윈도우 ASGI 미들웨어 (챕터 28)
- **백그라운드 작업**: InProcess 큐 + 워커, 202 응답 (챕터 27)
- **모니터링**: 구조화된 로그, `/health` (챕터 32)
- **OpenAPI**: 메타데이터/태그, Swagger UI (챕터 31)

## 실제 프로덕션으로 전환하려면

1. **보안**: `SECRET_KEY`를 환경 변수로 이동, 리프레시 토큰 + 토큰 저장소 도입.
2. **DB**: SQLite -> PostgreSQL, Alembic으로 마이그레이션 관리 (챕터 23).
3. **배포**: Gunicorn/Uvicorn 워커, Nginx, Docker 멀티스테이지, systemd (챕터 33-34).
4. **작업 큐**: asyncio.Queue -> Celery/ARQ + Redis로 교체 (챕터 27).
5. **속도 제한**: 다중 워커 공유를 위해 Redis 기반으로 교체 (챕터 28).
6. **테스트**: pytest-asyncio + coverage로 핵심 로직을 보호 (챕터 30).

## 연습

1. `Product`에 `category` 컬럼을 추가하고 필터링을 적용해 보세요.
2. 주문 시 상품명과 수량을 저장하는 `Order` 테이블을 만들고, 주문 목록 조회를 구현해 보세요.
3. `/orders`를 챕터 24의 리프레시 토큰 흐름과 연결해 보세요.

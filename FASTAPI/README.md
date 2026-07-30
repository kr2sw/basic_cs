# FastAPI 기초 강의 (20개 챕터)

FastAPI는 현대적인 Python 웹 프레임워크로, 빠른 성능과 자동 문서화를 제공합니다.

## 역사

FastAPI는 2018년 Sebastián Ramírez(티아고)가 공개했습니다. Starlette(ASGI 프레임워크) 위에 구축되었으며, Pydantic을 활용한 자동 데이터 검증과 OpenAPI/Swagger 문서 자동 생성을 핵심 기능으로 내세웠습니다. 2019년부터 GitHub Star가 급격히 증가하며 Django, Flask에 이어 Python 웹 프레임워크 3강 구도를 형성했습니다. 2020년에는 async/await 기반의 고성능이 주목받으며 Netflix, Uber, Microsoft 등 대기업에서 채택하기 시작했습니다. 2023년 FastAPI 0.100 버전에서는 Pydantic V2 지원이 도입되었고, 2024년에는 FastAPI 공식 팀이 구성되어 지속적인 개발이 이루어지고 있습니다.

## 특징

- **고성능**: Node.js, Go에 필적하는 속도 (Starlette + Pydantic 기반)
- **자동 API 문서화**: Swagger UI (/docs)와 ReDoc (/redoc) 자동 생성
- **타입 힌트 기반**: Python 타입 힌트를 활용한 직관적인 데이터 검증
- **비동기 지원**: async/await을 완벽 지원하여 높은 동시성 처리
- **Pydantic 통합**: 강력한 데이터 검증, 직렬화, 설정 관리
- **의존성 주입**: Depends를 통한 모듈화된 의존성 관리
- **OpenAPI 표준**: OpenAPI 스펙 완벽 준수, API 클라이언트 자동 생성 가능

## 실행

```bash
cd FASTAPI/01-introduction
pip install -r requirements.txt
uvicorn main:app --reload
# http://127.0.0.1:8000
# API 문서: http://127.0.0.1:8000/docs
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 00 | Setup | FastAPI 설치, Uvicorn, 가상 환경 |
| 01 | Introduction | 첫 FastAPI 앱, 자동 문서화 |
| 02 | Path & Query | 경로/쿼리 매개변수, Path/Query 검증 |
| 03 | Request Body | Pydantic 모델, POST 요청 |
| 04 | Response Model | 응답 모델, 상태 코드, 필드 제어 |
| 05 | HTTP Methods | GET/POST/PUT/PATCH/DELETE CRUD |
| 06 | Validation | Field, @field_validator, 커스텀 검증 |
| 07 | Error Handling | HTTPException, 커스텀 예외 처리기 |
| 08 | Dependencies | Depends, 의존성 주입, DB 세션 |
| 09 | Middleware | CORS, 커스텀 미들웨어, 로깅 |
| 10 | Database | SQLAlchemy, ORM, CRUD with DB |
| 11 | Authentication | OAuth2, JWT, passlib 해싱 |
| 12 | File Upload | UploadFile, 다중 파일 업로드 |
| 13 | Static & Templates | Jinja2, StaticFiles, HTML 렌더링 |
| 14 | WebSocket | 실시간 채팅, WebSocket 연결 관리 |
| 15 | Background Tasks | BackgroundTasks, 이메일 발송 |
| 16 | Testing | TestClient, pytest, parametrize |
| 17 | Versioning | APIRouter, prefix, 다중 버전 |
| 18 | Async Advanced | httpx.AsyncClient, asyncio.gather |
| 19 | Caching | ETag, lru_cache, Redis 캐싱 |
| 20 | Deployment | Uvicorn/Gunicorn, Docker, docker-compose |

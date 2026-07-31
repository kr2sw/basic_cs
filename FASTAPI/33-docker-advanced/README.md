# 33: Docker 고급 — 멀티스테이지 빌드, docker-compose

기초 챕터 20에서 단일 스테이지 Dockerfile을 다뤘습니다. 이번에는 이미지 크기와 보안을 개선하는 **멀티스테이지 빌드**와 **docker-compose**를 다룹니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

Docker 빌드/실행:

```bash
docker build -t fastapi-advanced .
docker run -p 8000:8000 fastapi-advanced
docker compose up --build     # 컴포즈 사용 시
```

## 주요 개념

### 멀티스테이지 빌드

빌드 단계와 실행 단계를 분리하면 최종 이미지에서 **불필요한 도구(pip, 소스 빌드 도구)를 제거**해 크기를 크게 줄이고 공격 표면을 줄입니다.

```dockerfile
# 1) builder: 의존성만 설치
FROM python:3.11-slim AS builder
COPY requirements.txt .
RUN pip install --no-cache-dir --prefix=/install -r requirements.txt

# 2) runtime: 설치된 패키지만 복사
FROM python:3.11-slim
COPY --from=builder /install /usr/local
COPY . .
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
```

이미지 크기 비교: 패키지 설치 캐시와 파이썬 개발 헤더가 빠져 보통 수십~수백 MB 차이가 납니다.

### 빌드 팁

- `COPY requirements.txt .` 먼저 → 의존성이 바뀌지 않으면 **레이어 캐시** 재사용.
- `--no-cache-dir`로 pip 캐시 제거.
- 비루트 사용자로 실행: `USER appuser`.
- 환경 변수: `PYTHONDONTWRITEBYTECODE`, `PYTHONUNBUFFERED`.

### docker-compose

여러 컨테이너(앱, DB, Redis, 프록시)를 한 번에 정의/관리합니다. `ports`, `environment`, `healthcheck`, `restart` 정책을 선언합니다.

```yaml
services:
  app:
    build: .
    ports: ["8000:8000"]
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8000/health"]
      interval: 30s
      retries: 3
    restart: unless-stopped
```

핵심 명령:

```bash
docker compose up -d --build   # 백그라운드 빌드/실행
docker compose logs -f app     # 로그 팔로우
docker compose down            # 컨테이너 종료 (볼륨은 유지)
docker compose down -v         # 볼륨까지 삭제
```

## 연습

1. `docker build` 후 `docker images`로 이미지 크기를 확인해 보세요.
2. `docker-compose.yml`에 Postgres 컨테이너를 추가하고 앱에서 연결해 보세요.

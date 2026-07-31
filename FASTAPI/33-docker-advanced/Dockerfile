# ---- 1단계: builder - 의존성 설치 ----
# python:3.11-slim을 기반으로 설치만 하고 결과물만 넘긴다.
FROM python:3.11-slim AS builder

WORKDIR /build

# 레이어 캐시를 위해 requirements.txt를 먼저 복사
COPY requirements.txt .
RUN pip install --no-cache-dir --prefix=/install -r requirements.txt

# ---- 2단계: runtime - 실행 환경 ----
# 설치 도구(pip 등)를 제외한 가벼운 이미지로 최종 컨테이너를 구성한다.
FROM python:3.11-slim

ENV PYTHONDONTWRITEBYTECODE=1 \
    PYTHONUNBUFFERED=1 \
    APP_ENV=production

WORKDIR /app

# builder 스테이지에서 설치된 패키지만 복사
COPY --from=builder /install /usr/local

# 소스 코드 복사
COPY . .

# 비루트 사용자로 실행 (보안 강화)
RUN useradd --create-home appuser
USER appuser

EXPOSE 8000

CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000", "--workers", "2"]

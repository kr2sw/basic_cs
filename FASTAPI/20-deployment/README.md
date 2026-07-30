# 20: 배포 — Uvicorn, Gunicorn, Docker

## 실행

```bash
uvicorn main:app --host 0.0.0.0 --port 8000 --workers 4
```

## 주요 개념

### Uvicorn (프로덕션)
```bash
uvicorn main:app --host 0.0.0.0 --port 8000 --workers 4
```

### Gunicorn + Uvicorn (프로덕션, Linux)
```bash
gunicorn -k uvicorn.workers.UvicornWorker main:app --bind 0.0.0.0:8000 --workers 4
```

### Docker
```bash
docker build -t fastapi-app .
docker run -p 8000:8000 fastapi-app
```

## Dockerfile

```dockerfile
FROM python:3.11-slim
WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt
COPY . .
EXPOSE 8000
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
```

## 환경 변수

```bash
# .env 파일
DATABASE_URL=postgresql://user:pass@localhost/db
SECRET_KEY=your-secret-key
DEBUG=false
```

## 환경별 설정
- 개발: `uvicorn main:app --reload`
- 프로덕션: `gunicorn -k uvicorn.workers.UvicornWorker main:app --workers=4`
- Docker: `docker compose up`

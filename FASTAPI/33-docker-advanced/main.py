import os

from fastapi import FastAPI

app = FastAPI(title="Docker 고급 - 멀티스테이지 빌드", version="1.0.0")


@app.get("/")
def home():
    return {"app": "docker-advanced", "env": os.getenv("APP_ENV", "local")}


@app.get("/health")
def health():
    return {"status": "ok"}

import os

from fastapi import FastAPI

app = FastAPI(title="프로덕션 배포", version="1.0.0")


@app.get("/")
def home():
    return {"app": "deployment", "env": os.getenv("APP_ENV", "production")}


@app.get("/health")
def health():
    return {"status": "ok"}

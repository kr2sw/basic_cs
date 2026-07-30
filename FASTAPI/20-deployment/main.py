import os

from fastapi import FastAPI

app = FastAPI(
    title="FastAPI App",
    version="1.0.0",
    docs_url="/docs" if os.getenv("ENV") != "production" else None,
    redoc_url=None,
)


@app.get("/")
def root():
    return {
        "message": "FastAPI Deployment Demo",
        "environment": os.getenv("ENV", "development"),
        "version": "1.0.0",
    }


@app.get("/health")
def health():
    return {"status": "healthy"}

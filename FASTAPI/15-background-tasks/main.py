import time
from typing import Optional

from fastapi import FastAPI, BackgroundTasks

app = FastAPI()


def write_log(message: str):
    with open("app.log", "a") as f:
        f.write(f"[{time.strftime('%Y-%m-%d %H:%M:%S')}] {message}\n")


def send_welcome_email(email: str, username: str):
    time.sleep(2)
    write_log(f"Welcome email sent to {email} for user {username}")


def cleanup_temp_files(file_id: str):
    time.sleep(1)
    write_log(f"Temp file {file_id} cleaned up")


@app.post("/users")
def create_user(username: str, email: str, tasks: BackgroundTasks):
    tasks.add_task(send_welcome_email, email, username)
    tasks.add_task(write_log, f"User {username} created")
    return {"message": f"User {username} created. Welcome email will be sent."}


@app.post("/files")
def upload_file(file_id: str, tasks: BackgroundTasks):
    tasks.add_task(cleanup_temp_files, file_id)
    return {"message": f"File {file_id} uploaded. Cleanup scheduled."}


@app.get("/logs")
def read_logs():
    try:
        with open("app.log", "r") as f:
            return {"logs": f.readlines()}
    except FileNotFoundError:
        return {"logs": []}

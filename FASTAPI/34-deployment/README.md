# 34: 배포 — Gunicorn/Uvicorn 워커, Nginx, systemd

개발 서버(`uvicorn main:app --reload`)는 프로덕션용이 아닙니다. 이번 챕터에서는 **Gunicorn + Uvicorn 워커**, **Nginx 리버스 프록시**, **systemd 서비스**까지 실제 배포 파이프라인을 다룹니다.

## 실행

```bash
pip install -r requirements.txt && uvicorn main:app --reload
```

## 주요 개념

### Gunicorn + Uvicorn 워커 (Linux)

Gunicorn은 프로세스/워커 관리, Uvicorn은 ASGI 실행을 담당합니다. `--workers`는 보통 CPU 코어 수 x 2 + 1을 권장합니다.

```bash
gunicorn -k uvicorn.workers.UvicornWorker \
  --workers 5 --bind 0.0.0.0:8000 \
  --timeout 120 main:app
```

- `--reload`는 절대 금지 (감독 없이 무중단 배포 불가).
- 오류 원격지/표준 로그는 파일로 남기고 **logrotate**로 순환.
- 서비스 계정(비루트)으로 실행.

### Nginx 리버스 프록시

Nginx가 클라이언트 요청을 받아 8000번 포트로 전달합니다. 정적 파일, TLS 종료, 로드밸런싱을 담당합니다.

```nginx
server {
    listen 80;
    server_name api.example.com;

    location / {
        proxy_pass http://127.0.0.1:8000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_read_timeout 300s;
        client_max_body_size 100m;
    }
}
```

WebSocket/SSE 사용 시 `proxy_read_timeout`을 충분히 주고 `Connection: upgrade` 헤더를 허용해야 합니다. 클라이언트 IP를 얻으려면 `--proxy-headers`와 `X-Forwarded-For` 처리가 필요합니다.

### systemd 서비스

재부팅 후 자동 재시작과 크래시 대응을 systemd로 관리합니다.

```ini
# /etc/systemd/system/fastapi.service
[Unit]
Description=FastAPI application
After=network.target

[Service]
User=fastapi
WorkingDirectory=/opt/fastapi
Environment="APP_ENV=production"
ExecStart=/opt/fastapi/.venv/bin/gunicorn \
    -k uvicorn.workers.UvicornWorker --workers 5 \
    --bind 127.0.0.1:8000 main:app
Restart=always
RestartSec=3

[Install]
WantedBy=multi-user.target
```

```bash
sudo systemctl daemon-reload
sudo systemctl enable --now fastapi
sudo systemctl status fastapi
```

### 배포 체크리스트

- 환경 변수로 설정 관리 (`.env` 비밀값 금지)
- Gunicorn/Uvicorn 워커 수, 타임아웃 튜닝
- Nginx 로그 + 액세스 로그 중앙 수집
- TLS 인증서 갱신 (certbot)
- Blue/Green 또는 Rolling 배포 전략

## 연습

1. `gunicorn`을 실행하고 워커 수에 따라 동시 처리량이 어떻게 달라지는지 확인해 보세요.
2. systemd 유닛 파일을 만들어 `systemctl restart fastapi`가 동작하도록 구성해 보세요.

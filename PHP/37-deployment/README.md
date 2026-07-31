# 37: 배포 — Docker, Nginx + PHP-FPM, 환경 변수

## 환경 변수 (.env)

설정값은 코드에 하드코딩하지 않고 환경 변수로 분리합니다.

```dotenv
APP_ENV=production
APP_DEBUG=false
DB_HOST=db
DB_PORT=3306
DB_PASSWORD="secret"
```

`.env`는 비밀번호를 포함하므로 **git에 커밋하지 않고** `.env.example`만 공유합니다. Laravel은 `phpdotenv`로 자동 로드합니다.

## Docker Compose 구성

| 서비스 | 역할 |
|--------|------|
| `app` | PHP-FPM 컨테이너 |
| `web` | Nginx (포트 80 노출) |
| `db` | MySQL |

```yaml
services:
  app:
    build: .
  web:
    image: nginx:alpine
    ports: ["80:80"]
  db:
    image: mysql:8.0
```

## Nginx + PHP-FPM

정적 파일은 Nginx가, `.php` 요청은 **FastCGI**로 PHP-FPM에 전달합니다.

```nginx
location ~ \.php$ {
    include fastcgi_params;
    fastcgi_param SCRIPT_FILENAME $document_root$fastcgi_script_name;
    fastcgi_pass app:9000;
}
```

Apache `mod_php`보다 메모리 효율과 장애 격리 측면에서 권장되는 조합입니다.

## 배포 절차

1. 코드 푸시/풀
2. `composer install --no-dev --optimize-autoloader`
3. `.env` 설정 (`APP_DEBUG=false`)
4. 마이그레이션 실행
5. 캐시 생성 (`config:cache`, `route:cache`, `view:cache`)
6. 컨테이너 빌드 및 기동
7. HTTPS 적용

## 체크리스트

- `APP_DEBUG=false` — 오류 화면으로 내부 경로·스택 노출 방지
- storage/캐시 디렉토리 쓰기 권한
- 롤백 전략 (이전 이미지로 재배포)

## 실행

```bash
php index.php
```

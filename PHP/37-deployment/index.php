<?php
// --- 배포: Docker, Nginx + PHP-FPM, 환경 변수 ---

echo "=== 1. 환경 변수 (.env) ===\n\n";

// 실제 프로젝트의 .env 파일 내용 예시
$envContent = "APP_ENV=production\n"
    . "APP_DEBUG=false\n"
    . "DB_HOST=db\n"
    . "DB_PORT=3306\n"
    . "# 주석은 무시됩니다\n"
    . "DB_PASSWORD=\"secret!pass\"\n";

// .env 파서 (phpdotenv 축약 버전)
function parseEnv(string $content): array {
    $env = [];
    foreach (explode("\n", $content) as $line) {
        $line = trim($line);
        if ($line === '' || str_starts_with($line, '#')) {
            continue;
        }
        [$key, $value] = explode('=', $line, 2);
        $env[trim($key)] = trim($value, " \t\n\r\0\x0B\"'");
    }
    return $env;
}

// 로드: putenv + $_ENV
function loadEnv(string $content): void {
    foreach (parseEnv($content) as $key => $value) {
        putenv("$key=$value");
        $_ENV[$key] = $value;
    }
}

loadEnv($envContent);

function config(string $key, mixed $default = null): mixed {
    return $_ENV[$key] ?? getenv($key) ?: $default;
}

echo "  .env 파싱 결과:\n";
foreach (parseEnv($envContent) as $key => $value) {
    echo "    $key = $value\n";
}
echo "  config('APP_ENV') → " . config('APP_ENV') . "\n";
echo "  config('DB_HOST') → " . config('DB_HOST') . "\n";
echo "  config('없는키', '기본값') → " . config('없는키', '기본값') . "\n\n";

echo "=== 2. Dockerfile ===\n\n";

$dockerfile = <<<'DOCKER'
FROM php:8.3-fpm-alpine

# 필요한 확장 설치
RUN docker-php-ext-install pdo_mysql opcache

# PHP 설정 복사
COPY php.ini /usr/local/etc/php/conf.d/php.ini

# 작업 디렉토리
WORKDIR /var/www/html

# 소스 복사
COPY . .

# 런타임 사용자 (보안: root로 실행하지 않음)
USER www-data
DOCKER;
echo $dockerfile . "\n\n";

echo "=== 3. docker-compose.yml ===\n\n";

$compose = <<<'YML'
services:
  app:
    build: .
    container_name: php_app
    environment:
      APP_ENV: production
      APP_DEBUG: "false"
      DB_HOST: db
    volumes:
      - ./public:/var/www/html/public
    depends_on:
      - db

  web:
    image: nginx:alpine
    ports:
      - "80:80"
    volumes:
      - ./public:/var/www/html/public
      - ./nginx/default.conf:/etc/nginx/conf.d/default.conf
    depends_on:
      - app

  db:
    image: mysql:8.0
    environment:
      MYSQL_ROOT_PASSWORD: secret
      MYSQL_DATABASE: myapp
    volumes:
      - db_data:/var/lib/mysql

volumes:
  db_data:
YML;
echo $compose . "\n\n";

echo "=== 4. Nginx + PHP-FPM 설정 ===\n\n";

$nginx = <<<'NGINX'
server {
    listen 80;
    server_name example.com;
    root /var/www/html/public;
    index index.php;

    # Laravel 스타일 프론트 컨트롤러
    location / {
        try_files $uri $uri/ /index.php?$query_string;
    }

    # .php 요청 → PHP-FPM으로 전달
    location ~ \.php$ {
        include fastcgi_params;
        fastcgi_param SCRIPT_FILENAME $document_root$fastcgi_script_name;
        fastcgi_pass app:9000;          # docker-compose의 app 서비스
        fastcgi_index index.php;
    }

    # 숨김 파일 접근 차단
    location ~ /\. {
        deny all;
    }
}
NGINX;
echo $nginx . "\n\n";

echo "=== 5. PHP-FPM 풀 설정 (www.conf) ===\n\n";

$fpm = <<<'INI'
[www]
user = www-data
group = www-data
listen = 9000

pm = dynamic                    # 프로세스 관리 방식
pm.max_children = 20            # 최대 프로세스 수
pm.start_servers = 5            # 시작 시 프로세스
pm.min_spare_servers = 3
pm.max_spare_servers = 10
INI;
echo $fpm . "\n\n";

echo "=== 6. 배포 절차 ===\n";
echo "  1) git push (CI/CD 트리거)\n";
echo "  2) composer install --no-dev --optimize-autoloader\n";
echo "  3) .env 설정 (APP_ENV=production, APP_DEBUG=false)\n";
echo "  4) php artisan migrate --force\n";
echo "  5) php artisan config:cache && route:cache && view:cache\n";
echo "  6) docker compose up -d --build\n";
echo "  7) 인증서 적용 (HTTPS)\n\n";

echo "=== 7. 프로덕션 체크리스트 ===\n";
echo "  - APP_DEBUG=false: 오류 화면에 경로/스택 노출 방지\n";
echo "  - HTTPS 강제 + HSTS\n";
echo "  - storage/, bootstrap/cache/ 쓰기 권한\n";
echo "  - 로그를 stdout/stderr로 수집 (docker logs)\n";
echo "  - 롤백 전략: 이미지 태그 고정 + 마이그레이션 순서 관리\n";

# 00 개발환경 설정

## 필수 도구

- **PHP** 8.x (https://www.php.net/downloads)
- **Composer** (PHP 의존성 관리, https://getcomposer.org)
- **XAMPP / Laragon / WAMP** (통합 환경, 선택 사항)

## PHP 설치

### Windows (scoop)
```bash
scoop install php
```

### Windows (직접)
1. https://windows.php.net/download 방문
2. ZIP 파일 다운로드 후 원하는 폴더에 압축 해제
3. PHP 경로를 시스템 PATH에 추가
4. `php.ini-development`를 `php.ini`로 복사 후 필요 시 수정

### macOS
```bash
brew install php
```

### Linux
```bash
sudo apt update
sudo apt install php-cli php-mbstring php-xml php-mysql php-curl
```

### 설치 확인
```bash
php --version
```

## Composer 설치

```bash
# Windows (scoop)
scoop install composer

# macOS/Linux
php -r "copy('https://getcomposer.org/installer', 'composer-setup.php');"
php composer-setup.php
php -r "unlink('composer-setup.php');"
mv composer.phar /usr/local/bin/composer
```

## 내장 개발 서버 실행

```bash
cd 01-hello-world
php -S localhost:8000
# 브라우저에서 http://localhost:8000 접속
```

## VS Code 확장

- **PHP IntelliSense**
- **PHP Debug**
- **Composer**

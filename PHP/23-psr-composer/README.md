# 23: PSR과 Composer — autoload, PSR-4, 버전 제약

## PSR 표준

PHP 표준 권고안(PHP Standards Recommendations)입니다.

| PSR | 내용 |
|-----|------|
| PSR-1 | 기본 코딩 표준 (파일, 네임스페이스, 클래스 규칙) |
| PSR-4 | **Autoloading** — 네임스페이스 → 디렉토리 매핑 |
| PSR-12 | 확장 코딩 표준 (PSR-1/2의 후속) |
| PSR-7 | HTTP 메시지 인터페이스 |

## PSR-4 오토로딩

네임스페이스 `App\Models\User`는 `src/Models/User.php`에서 찾습니다.

```json
{
    "autoload": {
        "psr-4": {
            "App\\": "src/"
        }
    }
}
```

`composer dump-autoload` 실행 후 `require 'vendor/autoload.php'`만으로 클래스가 자동 로드됩니다.

## Semantic Versioning (유의적 버전)

`MAJOR.MINOR.PATCH` 형식입니다.

- **MAJOR**: 하위 호환되지 않는 변경
- **MINOR**: 하위 호환되는 기능 추가
- **PATCH**: 버그 수정

## 버전 제약 (Constraints)

| 표현 | 의미 |
|------|------|
| `^1.2` | `>= 1.2 < 2.0` |
| `~1.2` | `>= 1.2 < 2.0` (1.2.x) |
| `~1.2.3` | `>= 1.2.3 < 1.3.0` (패치만) |
| `1.2.*` | `1.2.0` ~ `1.2.x` |
| `>=1.2` | 하한만 지정 |
| `*` | 모든 버전 |

## 기본 명령어

```bash
composer create-project laravel/laravel my-app "^10.0"
composer require monolog/monolog:^3.0
composer remove monolog/monolog
composer update
composer show
```

## 실행

```bash
php index.php
```

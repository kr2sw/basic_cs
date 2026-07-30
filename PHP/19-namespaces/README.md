# 19: Namespaces — 네임스페이스와 Composer

## 네임스페이스 (Namespace)

클래스, 함수, 상수의 이름 충돌을 방지합니다.

```php
namespace App\Models;
namespace App\Controllers;
```

## import (use)

```php
use App\Models\User;
use App\Controllers as Ctrl;
use function App\Helpers\formatDate;
use const App\Config\VERSION;
```

## PSR-4 Autoloading

Composer의 표준 오토로딩 방식입니다.

```json
{
    "autoload": {
        "psr-4": {
            "App\\": "src/"
        }
    }
}
```

## Composer 기본 명령어

```bash
composer init          # 프로젝트 초기화
composer require 패키지  # 패키지 설치
composer install        # 의존성 설치
composer dump-autoload  # 오토로더 갱신
```

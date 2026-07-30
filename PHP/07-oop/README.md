# 07: OOP — 객체 지향 프로그래밍

## 클래스와 객체

```php
class Car {
    public string $model;   // 프로퍼티 (속성)
    private int $year;

    public function __construct(string $model, int $year) {
        $this->model = $model;
        $this->year = $year;
    }

    public function start(): void {
        echo "시동";
    }
}
```

## 접근 제어자 (Visibility)

| 제어자 | 같은 클래스 | 자식 클래스 | 외부 |
|--------|-----------|-----------|-----|
| `public` | O | O | O |
| `protected` | O | O | - |
| `private` | O | - | - |

## 생성자 / 소멸자

- `__construct()`: 객체 생성 시 자동 호출
- `__destruct()`: 객체 소멸 시 자동 호출

## static

- `static` 키워드로 클래스 레벨 멤버 정의
- `::` 범위 해결 연산자로 접근
- `self::`, `static::`, `parent::`

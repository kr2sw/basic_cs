# 31: 테스팅 — PHPUnit 스타일 어설션, 테스트 구조 (간단 구현)

## PHPUnit

PHP 표준 테스트 프레임워크입니다.

```bash
composer require --dev phpunit/phpunit
vendor/bin/phpunit tests
```

## 테스트 구조

```php
class CalculatorTest extends TestCase {
    public function setUp(): void {
        $this->calc = new Calculator();
    }

    public function testAddition(): void {
        $this->assertEquals(5, $this->calc->add(2, 3));
    }
}
```

- 테스트 클래스는 `TestCase`를 상속
- 테스트 메서드는 `test`로 시작
- 테스트마다 새 인스턴스가 생성되어 **격리**됩니다
- `setUp()`/`tearDown()`으로 사전/사후 작업 처리

## 주요 어설션

| 메서드 | 의미 |
|--------|------|
| `assertEquals` | 값 비교 (`==`) |
| `assertSame` | 타입까지 비교 (`===`) |
| `assertTrue` / `assertFalse` | 참/거짓 확인 |
| `assertNull` | null 확인 |
| `assertCount` | 배열 길이 확인 |
| `assertStringContainsString` | 문자열 포함 확인 |
| `expectException` | 예외 발생 확인 |

## 데이터 제공자 (Data Provider)

```php
#[DataProvider('addProvider')]
public function testAdd(int $a, int $b, int $expected) { ... }

public static function addProvider(): array {
    return [[1, 2, 3], [5, 5, 10]];
}
```

## 실행

```bash
php index.php
```

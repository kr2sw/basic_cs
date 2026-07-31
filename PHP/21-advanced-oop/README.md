# 21: 고급 OOP — 트레이트, 익명 클래스, 매직 메서드, 객체 복사

## 트레이트 (Trait)

다중 상속을 흉내 내며 코드를 재사용하는 메커니즘입니다. `use` 키워드로 클래스에 포함합니다.

```php
trait Timestampable {
    public function getCreatedAt(): string { ... }
}

class Post {
    use Timestampable;
}
```

충돌이 발생하면 `insteadof`로 우선 순위를 정하고 `as`로 별칭을 부여합니다.

```php
use A, B {
    A::hello insteadof B;
    B::hello as helloFromB;
}
```

## 익명 클래스 (Anonymous Class)

이름 없는 클래스를 `new class` 구문으로 즉석에서 만듭니다.

```php
$greeter = new class implements Greeter {
    public function greet(string $name): string { ... }
};
```

## 매직 메서드 (Magic Methods)

| 메서드 | 설명 |
|--------|------|
| `__get` / `__set` | 존재하지 않는 프로퍼티 접근/할당 |
| `__isset` / `__unset` | 존재하지 않는 프로퍼티의 isset/unset |
| `__call` / `__callStatic` | 존재하지 않는 메서드 호출 |
| `__toString` | 객체를 문자열로 변환 |
| `__invoke` | 객체를 함수처럼 호출 |
| `__clone` | clone 시 복제 동작 커스터마이즈 |

## 객체 복사 (Cloning)

`clone` 키워드로 객체를 복사하면 객체는 새로 만들어지지만 프로퍼티가 **객체(참조)**인 경우 얕은 복사가 됩니다. `__clone` 메서드에서 깊은 복사를 직접 처리해야 합니다.

```php
public function __clone() {
    $this->address = clone $this->address;
}
```

## Enum (PHP 8.1+)

상수 집합을 하나의 타입으로 정의합니다. backed enum(`: string`)은 `->value`로 원본 값을 사용합니다.

## 실행

```bash
php index.php
```

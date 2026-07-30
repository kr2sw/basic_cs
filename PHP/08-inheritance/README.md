# 08: Inheritance — 상속

## 상속

```php
class Dog extends Animal {
    // Animal의 모든 public/protected 멤버 상속
}
```

## parent 키워드

- `parent::__construct()`: 부모 생성자 호출
- `parent::메서드()`: 부모 메서드 호출

## 오버라이딩 (Overriding)

- 자식 클래스에서 부모 메서드를 재정의
- 시그니처는 동일해야 함 (PHP 8.1+부터 strict)

## final 키워드

- `final class`: 상속 불가능 클래스
- `final method`: 오버라이딩 불가능 메서드

## 클래스 상수 / static

- `parent::`, `self::`, `static::` (Late Static Binding)

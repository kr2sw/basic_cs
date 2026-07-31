# 22: 디자인 패턴 — 싱글턴, 팩토리, 전략, 옵저버, 의존성 주입

## 싱글턴 (Singleton)

클래스의 인스턴스가 하나만 생성되도록 보장합니다.

```php
final class Logger {
    private static ?Logger $instance = null;
    private function __construct() {}
    public static function getInstance(): Logger {
        return self::$instance ??= new self();
    }
}
```

생성자를 `private`로 막고 정적 메서드로만 접근합니다.

## 팩토리 (Factory)

객체 생성을 전담하는 메서드로, 생성 로직을 한곳에 모읍니다.

```php
PaymentFactory::create('kakao');  // 카드/카카오/네이버 결제 객체 생성
```

`match` 표현식과 함께 쓰면 분기 처리가 깔끔합니다.

## 전략 (Strategy)

알고리즘(전략)을 인터페이스로 캡슐화하고 실행 시점에 교체합니다.

```php
$order->setShippingStrategy(new ExpressShipping());
```

## 옵저버 (Observer)

상태 변화를 구독자에게 알리는 패턴입니다. 이벤트/알림 시스템의 기반입니다.

```php
$user->attach(new EmailNotifier());
$user->register('a@example.com');  // 구독자에게 통지
```

## 의존성 주입 (DI)

객체가 필요한 의존성을 스스로 만들지 않고 **생성자로 주입**받습니다.

```php
new UserService(new SmtpMailer());
```

테스트에서는 `MockMailer`를 주입해 외부 서비스 없이 검증할 수 있습니다.

## 실행

```bash
php index.php
```

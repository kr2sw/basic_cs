# 39: SOLID 원칙 — 단일 책임, 개방-폐쇄 등 5원칙

## S — 단일 책임 (Single Responsibility)

클래스는 **변경 이유가 하나**여야 합니다. 데이터, 포맷, 저장을 한 클래스에 몰아 넣지 말고 분리합니다.

```
ReportData (데이터) → HtmlFormatter (표시) → ReportFileWriter (저장)
```

## O — 개방-폐쇄 (Open/Closed)

확장에는 열리고 **수정에는 닫혀** 있어야 합니다. 새 기능은 기존 코드를 고치는 대신 추가(다형성)로 구현합니다.

```php
interface Shape { public function area(): float; }
```

새 도형을 추가해도 `AreaCalculator`는 수정하지 않습니다.

## L — 리스코프 치환 (Liskov Substitution)

하위 타입은 상위 타입이 쓰이는 곳에 **문제없이 치환**되어야 합니다.

```php
class BadSquare extends BadRectangle {
    public function setWidth(int $w): void { $this->w = $this->h = $w; }
}
```

정사각형이 직사각형의 불변식(가로≠세로 가능)을 깨는 전형적인 위반입니다. 상속 대신 공통 인터페이스로 대체합니다.

## I — 인터페이스 분리 (Interface Segregation)

클라이언트는 **사용하지 않는 메서드에 의존하면 안** 됩니다. 큰 인터페이스 대신 역할별 인터페이스로 나눕니다.

```php
interface Coder { public function code(): void; }
interface Designer { public function design(): void; }
```

## D — 의존성 역전 (Dependency Inversion)

상위 모듈은 **추상화에 의존**해야 합니다. 구체 클래스를 직접 new 하지 말고 인터페이스로 주입받습니다.

```php
class MailService {
    public function __construct(private MailerInterface $mailer) {}
}
```

테스트에서는 Mock 구현으로 교체할 수 있습니다.

## 실행

```bash
php index.php
```

# 39: Design Patterns — 디자인 패턴

## 싱글턴 (Singleton)

인스턴스를 하나만 만들고 전역에서 공유합니다.

```java
public enum Config {
    INSTANCE;   // enum 으로 안전한 싱글턴

    private final Properties props = load();
}
```

- 게으른 초기화(지연 생성), 이중 검사 잠금(DCL) 기법이 대표적
- enum 기반 싱글턴은 스레드 안전 + 직렬화 안전

## 팩토리 (Factory)

객체 생성 로직을 별도 클래스/메서드로 분리합니다.

```java
Shape shape = ShapeFactory.create("circle");   // 어떤 객체인지는 호출자가 모름
```

- 팩토리 메서드, 추상 팩토리 패턴
- 생성 지점을 한곳에 모아 변경에 유연

## 전략 (Strategy)

알고리즘을 인터페이스로 캡슐화해 런타임에 교체합니다.

```java
payment.pay(amount, new CreditCardStrategy());
payment.pay(amount, new KakaoPayStrategy());
```

- 람다를 쓰면 전략이 하나의 함수로 표현됨
- `if-else` 분기를 제거하는 대표 패턴

## 옵저버 (Observer)

상태 변화를 다른 객체들에게 알립니다.

```java
subject.register(listener);   // 구독
subject.notifyListeners(msg); // 이벤트 발행
```

- 이벤트 리스너, pub/sub 구조의 기본
- 결합도 낮춰 확장에 유리

## 실행

```bash
cd JAVA/39-design-patterns
javac Main.java && java Main
```

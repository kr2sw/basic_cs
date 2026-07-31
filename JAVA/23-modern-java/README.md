# 23: Modern Java — 모던 자바

## record (불변 데이터 클래스)

생성자, `equals`, `hashCode`, `toString` 을 자동 생성합니다.

```java
record Point(int x, int y) {}
var p = new Point(1, 2);
p.x();                 // 접근자 (getX() 아님)
```

- 필드는 `private final` 로 불변
- 컴팩트 생성자로 검증 로직 추가 가능
- `with` 형태의 수정은 새 인스턴스를 만들어야 함

## sealed class (봉인된 클래스, Java 17+)

상속 가능한 하위 클래스를 명시적으로 제한합니다.

```java
sealed interface Shape permits Circle, Square {}
record Circle(double r) implements Shape {}
record Square(double s) implements Shape {}
```

- `permits` 로 허용된 타입만 상속 가능
- 모든 하위 타입은 `sealed`, `non-sealed`, `final` 중 하나여야 함

## pattern matching for instanceof

`instanceof` 검사 후 자동 형변환합니다.

```java
if (obj instanceof String s && s.length() > 5) {
    System.out.println(s.toUpperCase());
}
```

## switch 패턴 매칭 (Java 21+)

switch 문에서 타입 패턴과 null 처리가 가능합니다.

```java
String result = switch (shape) {
    case Circle c -> "원, 반지름 " + c.r();
    case Square s -> "정사각형, 한변 " + s.s();
    default       -> "알 수 없음";
};
```

## 그 외 모던 기능

- `var` : 지역 변수 타입 추론
- Text Block (`"""`) : 멀티라인 문자열
- 새 switch 문 (`->` 화살표, yield)

## 실행

```bash
cd JAVA/23-modern-java
javac Main.java && java Main
```

# 06: OOP — 객체 지향 프로그래밍

## 클래스와 객체

```java
public class Car {
    String model;       // 필드 (속성)
    int year;

    void start() { }    // 메서드 (동작)
}
```

## 생성자 (Constructor)

- 클래스명과 동일, 반환타입 없음
- 기본 생성자는 생략 가능 (다른 생성자가 없을 때)
- `this()`로 다른 생성자 호출 가능

## 접근 제어자 (Access Modifiers)

| 제어자 | 같은 클래스 | 같은 패키지 | 자식 클래스 | 전체 |
|--------|-----------|-----------|-----------|-----|
| `private` | O | - | - | - |
| (default) | O | O | - | - |
| `protected` | O | O | O | - |
| `public` | O | O | O | O |

## this 키워드

- `this.필드` : 인스턴스 변수 참조
- `this()` : 같은 클래스의 다른 생성자 호출

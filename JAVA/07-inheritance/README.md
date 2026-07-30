# 07: Inheritance — 상속

## 상속(Inheritance)

```java
class 자식 extends 부모 {
    // 부모의 필드와 메서드 상속
}
```

## super 키워드

- `super()` : 부모 생성자 호출
- `super.필드` / `super.메서드()` : 부모의 멤버 참조

## 오버라이딩 (Overriding)

- 부모 메서드를 자식에서 재정의
- `@Override` 어노테이션으로 명시 (필수는 아니지만 권장)
- 규칙: 시그니처 동일, 접근 범위 좁힐 수 없음

## Object 클래스

모든 클래스의 최상위 부모. 주요 메서드:
- `toString()`, `equals()`, `hashCode()`, `getClass()`

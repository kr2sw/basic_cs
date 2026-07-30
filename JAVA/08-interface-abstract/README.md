# 08: Interface & Abstract — 인터페이스와 추상 클래스

## 추상 클래스 (Abstract Class)

- `abstract` 키워드로 선언
- 추상 메서드(본문 없음)를 가질 수 있음
- **단일 상속**만 가능
- 생성자, 필드, 일반 메서드도 가질 수 있음

## 인터페이스 (Interface)

- `interface` 키워드로 선언
- 모든 메서드는 기본적으로 `public abstract` (Java 8+에서 default/static 메서드 가능)
- **다중 구현** 가능
- 필드는 `public static final` (상수)

| 특징 | 추상 클래스 | 인터페이스 |
|------|-----------|-----------|
| 상속 | 단일 상속 | 다중 구현 |
| 생성자 | 가능 | 불가능 |
| 필드 | 일반 필드 가능 | 상수만 가능 |
| 메서드 | 일반 + 추상 | 추상 + default + static |

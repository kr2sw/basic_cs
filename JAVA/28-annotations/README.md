# 28: Annotations — 어노테이션 처리

## 커스텀 어노테이션 정의

`@interface` 키워드로 정의합니다.

```java
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.METHOD)
public @interface Todo {
    String value() default "";
    int priority() default 5;
}
```

- 요소(element)는 메서드처럼 선언하고 `default` 로 기본값 지정
- 요소 타입: 기본형, String, Class, enum, 어노테이션, 배열

## Retention (유지 정책)

| 정책 | 의미 |
|------|------|
| `SOURCE` | 소스 코드까지만 (컴파일러가 버림) |
| `CLASS` | 바이트코드에 남음 (런타임엔 없음) |
| `RUNTIME` | 런타임까지 유지 (리플렉션으로 읽기 가능) |

## Target (적용 대상)

| 값 | 적용 위치 |
|----|-----------|
| `TYPE` | 클래스/인터페이스 |
| `METHOD` | 메서드 |
| `FIELD` | 필드 |
| `PARAMETER` | 파라미터 |
| `ANNOTATION_TYPE` | 어노테이션 위 |

## 리플렉션으로 읽기

```java
if (cls.isAnnotationPresent(MyAnno.class)) {
    MyAnno a = cls.getAnnotation(MyAnno.class);
}
```

- `getAnnotations()` : 모든 어노테이션
- `getDeclaredAnnotations()` : 선언된 어노테이션
- 런타임 처리 예: 스프링 컴포넌트 스캔, JUnit

## 어노테이션 활용

- 문서화, 컴파일 체크 (`@Override`, `@SuppressWarnings`)
- 런타임 처리 (프레임워크 설정, 테스트, 검증)

## 실행

```bash
cd JAVA/28-annotations
javac Main.java && java Main
```

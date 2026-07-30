# 13: Generics — 제네릭

## 제네릭 (Generics)

타입을 파라미터로 받아 타입 안전성을 보장하고 형변환을 제거합니다.

```java
class Box<T> {          // 제네릭 클래스
    private T value;
    public T get() { return value; }
}
```

## 타입 파라미터 관례

| 문자 | 의미 |
|------|------|
| `E` | Element (컬렉션) |
| `K` | Key |
| `V` | Value |
| `N` | Number |
| `T` | Type |
| `S, U, V` | 기타 타입 |

## 제네릭 메서드

```java
public <T> T getFirst(List<T> list) { ... }
```

## 제한된 타입 파라미터 (Bounded Type)

```java
<T extends Number>  // Number 또는 그 하위 타입만 허용
```

## 와일드카드

- `?` : 알 수 없는 타입 (Unbounded)
- `? extends T` : T 또는 T의 하위 타입 (공변)
- `? super T` : T 또는 T의 상위 타입 (반공변)

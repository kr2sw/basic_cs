# 22: Optional & Functional — Optional과 함수형 인터페이스

## Optional 다시 보기

`Optional` 은 값이 있을 수도 없을 수도 있음을 표현하는 컨테이너입니다.

```java
Optional<String> opt = Optional.ofNullable(str);
opt.map(String::toUpperCase)
   .filter(s -> s.length() > 3)
   .orElse("기본값");
```

| 메서드 | 설명 |
|--------|------|
| `of(value)` | null이 아닌 값 생성 |
| `ofNullable(value)` | null 허용 생성 |
| `empty()` | 빈 Optional |
| `orElse(v)` / `orElseGet(supplier)` | 기본값 반환 |
| `orElseThrow()` | 예외 발생 |
| `ifPresent(consumer)` | 값 있으면 소비 |
| `map` / `flatMap` | 값 변환 체이닝 |

## Supplier 와 Consumer

- `Supplier<T>` : `T get()` — 값을 공급만 함 (지연 계산에 유용)
- `Consumer<T>` : `void accept(T)` — 값을 소비만 함
- `BiFunction<T,U,R>` : 두 값을 받아 한 값을 반환
- `BiConsumer`, `BiPredicate` 도 존재

```java
Supplier<String> s = () -> "지연 생성된 값";
BiFunction<Integer, Integer, Integer> add = Integer::sum;
```

## Function 합성

`Function` 의 `andThen` 과 `compose` 로 함수를 조합합니다.

```java
Function<Integer, Integer> doubleIt = x -> x * 2;
Function<Integer, Integer> plusOne = x -> x + 1;

doubleIt.andThen(plusOne).apply(3);   // (3*2)+1 = 7
doubleIt.compose(plusOne).apply(3);   // (3+1)*2 = 8
```

## 커링 (Currying)

여러 인자를 하나씩 받는 함수로 쪼개는 기법입니다.

```java
// (x, y) -> x + y 를 x를 먼저 고정하는 함수로 분리
Function<Integer, Function<Integer, Integer>> curriedAdd =
    x -> y -> x + y;

curriedAdd.apply(3).apply(4);   // 7
```

부분 적용(partial application)으로 재사용성을 높일 수 있습니다.

## 함수형 파이프라인

함수형 인터페이스들을 조합해 입출력을 가지는 파이프라인을 구성할 수 있습니다.

## 실행

```bash
cd JAVA/22-optional-functional
javac Main.java && java Main
```

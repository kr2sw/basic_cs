# 38: 함수형 패턴 — Functional Patterns

C#에서 함수형 프로그래밍의 핵심 패턴인 **Option/Either**과 **파이프라인**을
구현합니다. null 대신 명시적인 값 부재를 표현하고, 예외 대신 타입으로
실패를 다룹니다.

## Option 타입

값이 있거나(`Some`) 없거나(`None`)를 타입으로 표현합니다. `null`과 달리
"없음"이 명시적이고 안전합니다.

```csharp
Option<int> ParseNumber(string s)
    => int.TryParse(s, out var n) ? Option<int>.Some(n) : Option<int>.None;
```

`Map`/`Bind`로 값이 있을 때만 변환을 적용합니다.

## Either 타입

성공(`Right`) 또는 실패(`Left`)를 함께 표현합니다. 예외 대신 오류 타입을
반환하는 함수형 오류 처리 방식입니다.

## 파이프라인

여러 단계의 변환을 함수 합성으로 연결합니다. LINQ의 `Select`/`SelectMany`
역시 모나딕 파이프라인으로 볼 수 있습니다.

```csharp
var result = input
    .Map(Parse)
    .Bind(Validate)
    .Bind(Save);
```

## 실행

```bash
dotnet run
```

## 핵심 요약

- `Option<T>`는 null 대신 값 부재를 타입으로 표현합니다.
- `Either<L, R>`는 실패 정보를 타입으로 전달해 예외에 의존하지 않습니다.
- 파이프라인 합성으로 로직을 작고 재사용 가능한 단위로 나눕니다.

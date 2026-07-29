# 12 제네릭(Generics)

C# 제네릭을 사용하여 타입-안전한 재사용 가능한 코드를 작성하는 방법을 학습합니다.

## 주요 개념

- 제네릭 클래스 (`Box<T>`, `Pair<TFirst, TSecond>`)
- 타입 제약 조건: `where T : class`, `struct`, `new()`, `IComparable<T>`
- 제네릭 메서드 (`Swap<T>`, `Max<T>`)
- 제네릭 인터페이스 (`IRepository<T>`)
- `Nullable<T>` (`int?`)
- `??` null-coalescing / `?.` null-conditional 연산자

## 예제 코드

```csharp
class Box<T> { public T Value { get; set; } }
class Pair<TFirst, TSecond> { /* 다중 타입 파라미터 */ }

public static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);
public static T Max<T>(T a, T b) where T : IComparable<T> { /* ... */ }

int? maybeNull = null;
int result = maybeNull ?? -1;
```

## 실행 방법

```bash
dotnet run --project ../12_generics
```

## 핵심 요약

- 제네릭은 타입을 파라미터화하여 코드 재사용성과 타입 안전성을 높입니다.
- `where` 제약 조건으로 허용되는 타입을 제한할 수 있습니다.
- `Nullable<T>`는 값 타입에 `null` 의미를 추가합니다.

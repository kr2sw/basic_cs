# 09 예외 처리(Exception Handling)

C#의 예외 처리 메커니즘과 사용자 정의 예외를 학습합니다.

## 주요 개념

- `try` / `catch` / `finally` — 기본 예외 처리
- 여러 `catch` 블록 — 예외 타입별 분기
- `throw` — 예외 발생
- 예외 필터 (`when` 키워드, C# 6.0+)
- `using` 문 — 안전한 리소스 관리
- 사용자 정의 예외 클래스

## 예제 코드

```csharp
try
{
    int number = int.Parse(input);
    int result = 100 / number;
}
catch (FormatException ex)
{
    Console.WriteLine($"Invalid format: {ex.Message}");
}
catch (DivideByZeroException)
{
    Console.WriteLine("Cannot divide by zero");
}
finally
{
    Console.WriteLine("Finally block always executes");
}
```

## 실행 방법

```bash
dotnet run --project ../09_exceptions
```

## 핵심 요약

- `finally` 블록은 예외 발생 여부와 관계없이 항상 실행됩니다.
- 예외 필터(`when`)로 특정 조건에서만 catch할 수 있습니다.
- 사용자 정의 예외는 `Exception` 클래스를 상속하여 만듭니다.

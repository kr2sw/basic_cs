# 20 단위 테스트 (Unit Testing)

C#에서 테스트 가능한 코드를 작성하고 계산기 클래스를 예제로 학습합니다. (실제 단위 테스트 프로젝트는 별도로 구성)

## 주요 개념

- 테스트 대상 클래스 (`Calculator`) 설계
- 사칙연산 메서드: `Add`, `Subtract`, `Multiply`, `Divide`
- 예외 상황 처리 (`DivideByZeroException`)
- XML 문서 주석 (`/// <summary>`)
- `Main()`에서 데모 실행으로 기능 검증

## 예제 코드

```csharp
public class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Subtract(int a, int b) => a - b;
    public int Multiply(int a, int b) => a * b;
    public int Divide(int a, int b)
    {
        if (b == 0)
            throw new DivideByZeroException("0으로 나눌 수 없습니다.");
        return a / b;
    }
}
```

## 실행 방법

```bash
dotnet run --project ../20_unit_testing
```

## 핵심 요약

- 단위 테스트 가능한 코드는 작고, 명확한 책임을 가지며, 의존성이 낮아야 합니다.
- `DivideByZeroException` 같은 예외도 테스트의 중요한 대상입니다.
- XML 문서 주석으로 코드의 의도를 명확히 전달할 수 있습니다.

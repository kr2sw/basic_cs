# 08 인터페이스(Interfaces)

C# 인터페이스를 통한 추상화와 다형성, 의존성 주입을 학습합니다.

## 주요 개념

- 인터페이스 선언과 구현
- 다중 인터페이스 구현
- `IDisposable` — 리소스 정리 (`using` 문)
- 의존성 주입 (Dependency Injection) 예시
- 기본 인터페이스 메서드 (C# 8.0+ Default Interface Method)

## 예제 코드

```csharp
interface ILogger { void Log(string message); }
class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[Console] {message}");
}

class Car : IEngine, IHorn, IDisposable { /* 다중 구현 */ }

interface ISpeak
{
    void SayHello() => Console.WriteLine("Hello!"); // default 구현
}
```

## 실행 방법

```bash
dotnet run --project ../08_interfaces
```

## 핵심 요약

- 인터페이스는 '무엇을 할 수 있는가'를 정의합니다. (계약)
- C# 8.0부터 인터페이스에 기본 구현을 포함할 수 있습니다.
- `IDisposable`을 구현하면 `using` 문으로 리소스를 자동 정리할 수 있습니다.

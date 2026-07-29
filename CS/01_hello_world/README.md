# 01 Hello World — C# 시작하기

C#의 기본 구조와 첫 프로그램을 작성하는 방법을 학습합니다.

## 주요 개념

- `using System;` — 네임스페이스 임포트
- `namespace` / `class` / `Main()` — 프로그램 진입점
- `Console.WriteLine()` / `Console.Write()` — 콘솔 출력
- `Console.ReadLine()` — 사용자 입력
- 문자열 보간 (`$"..."`)

## 예제 코드

```csharp
static void Main()
{
    Console.WriteLine("Hello, World!");
    Console.Write("Enter your name: ");
    string name = Console.ReadLine()!;
    Console.WriteLine($"Nice to meet you, {name}!");
}
```

## 실행 방법

```bash
dotnet run --project ../01_hello_world
```

## 핵심 요약

- `Main()` 메서드는 모든 C# 프로그램의 진입점입니다.
- `Console` 클래스로 콘솔 입출력을 처리합니다.
- `$""` 문자열 보간을 사용하면 변수를 간편하게 출력할 수 있습니다.

# 14 문자열 (Strings)

C# 문자열 처리의 다양한 기법과 `StringBuilder`를 학습합니다.

## 주요 개념

- `string` (불변) vs `StringBuilder` (가변)
- 문자열 메서드: `Trim`, `Substring`, `IndexOf`, `Replace`, `Contains`
- `Split` / `Join` — 문자열 분할 및 결합
- 문자열 보간 (`$""`) — 형식 지정 및 조건식 포함
- 축자 문자열 (`@""`) — 이스케이프 없이 경로/멀티라인
- `String.Format` — 날짜, 숫자, 통화 형식
- `StringComparison` — 문화권별 문자열 비교

## 예제 코드

```csharp
string text = "  Hello, C# World!  ";
Console.WriteLine($"Trim: \"{text.Trim()}\"");
Console.WriteLine($"Contains(\"C#\"): {text.Contains("C#")}");

var sb = new StringBuilder("Hello");
sb.Append(", World!");
sb.Replace("World", "C#");

string msg = $"My name is {name} and I'm {age} years old.";
string path = @"C:\Users\Alice\Documents\file.txt";
```

## 실행 방법

```bash
dotnet run --project ../14_strings
```

## 핵심 요약

- `string`은 불변 — 연결 시 새 객체 생성, `StringBuilder`는 가변으로 성능 우수.
- `$""` 보간 문자열로 간결하고 가독성 높은 문자열을 만들 수 있습니다.
- `@""` 축자 문자열은 백슬래시를 이스케이프 문자로 처리하지 않습니다.

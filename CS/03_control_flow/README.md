# 03 제어문(Control Flow)

조건문과 반복문을 사용하여 프로그램의 흐름을 제어하는 방법을 학습합니다.

## 주요 개념

- `if` / `else if` / `else` — 조건 분기
- `switch` 문 / `switch` 식 (C# 8.0+)
- `for` / `foreach` / `while` / `do-while` 반복문

## 예제 코드

```csharp
int score = 85;
if (score >= 90)
    Console.WriteLine("Grade: A");
else if (score >= 80)
    Console.WriteLine("Grade: B");

string category = score switch
{
    >= 90 => "Excellent",
    >= 80 => "Good",
    _ => "Needs improvement"
};

for (int i = 0; i < 5; i++)
    Console.Write($"{i} ");
```

## 실행 방법

```bash
dotnet run --project ../03_control_flow
```

## 핵심 요약

- `switch` 식은 값을 반환하는 간결한 분기 처리를 제공합니다.
- `foreach`는 컬렉션을 순회할 때 가장 자주 사용됩니다.

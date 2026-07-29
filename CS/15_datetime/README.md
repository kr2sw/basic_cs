# 15 날짜와 시간 (DateTime)

C#의 `DateTime`, `TimeSpan`, `DateTimeOffset`, `DateOnly`, `TimeOnly` 등을 학습합니다.

## 주요 개념

- `DateTime.Now` / `UtcNow` / `Today`
- `DateTime` 생성, 속성 (Year, Month, Day 등)
- `AddDays`, `AddMonths`, `AddYears` 등 날짜 연산
- `TimeSpan` — 시간 간격 계산 및 생성
- `Parse` / `TryParse` / `ParseExact` — 문자열 파싱
- `DateTimeOffset` — 시간대 오프셋 포함
- `TimeZoneInfo` — 시간대 변환
- `DateOnly` / `TimeOnly` (.NET 6+)
- `Stopwatch` — 성능 측정
- 문화권별 날짜 형식

## 예제 코드

```csharp
DateTime now = DateTime.Now;
TimeSpan duration = end - start;
DateTime parsed = DateTime.Parse("2026-12-25");
DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
Stopwatch sw = Stopwatch.StartNew();
```

## 실행 방법

```bash
dotnet run --project ../15_datetime
```

## 핵심 요약

- `DateTime`은 날짜와 시간을, `TimeSpan`은 시간 간격을 나타냅니다.
- `DateTimeOffset`은 UTC 오프셋 정보를 포함하여 시간대를 안전하게 처리합니다.
- .NET 6+의 `DateOnly` / `TimeOnly`는 날짜와 시간을 분리하여 표현합니다.

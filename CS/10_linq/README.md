# 10 LINQ (Language Integrated Query)

C#의 LINQ를 사용하여 컬렉션을 쿼리하고 변환하는 방법을 학습합니다.

## 주요 개념

- 쿼리 구문 (query syntax) / 메서드 구문 (method syntax, fluent)
- `Where`, `OrderBy`, `Select` — 필터링, 정렬, 변환
- 집계 함수: `Count`, `Average`, `Max`, `Min`
- `GroupBy` — 그룹화
- `Any`, `All`, `Contains` — 조건 검사
- `FirstOrDefault`, `SelectMany`, `Zip`
- `Enumerable.Range`

## 예제 코드

```csharp
var honorRoll = from s in students
                where s.Grade >= 3.5
                orderby s.Grade descending
                select $"{s.Name} ({s.Grade:F2})";

var topStudents = students
    .Where(s => s.Grade >= 3.0)
    .OrderBy(s => s.Name)
    .Select(s => new { s.Name, s.Grade });
```

## 실행 방법

```bash
dotnet run --project ../10_linq
```

## 핵심 요약

- LINQ는 컬렉션에 대한 선언적 쿼리를 제공합니다.
- 쿼리 구문과 메서드 구문은 상호 변환 가능하며, 같은 IL로 컴파일됩니다.
- 지연 실행(deferred execution)으로 성능을 최적화합니다.

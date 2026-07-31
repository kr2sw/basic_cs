# 21: 고급 LINQ — Advanced LINQ

중급 과정의 첫 장. LINQ의 내부 동작 원리와 고급 연산자를 학습합니다.

## Expression 트리 개념

LINQ 쿼리는 두 가지 형태로 컴파일됩니다. `Func<T>` 델리게이트는 IL 코드로
컴파일되는 반면, `Expression<Func<T>>`은 **데이터로 표현되는 코드**입니다.
Expression 트리는 쿼리를 분석·변형해서 다른 언어(SQL 등)로 번역할 수 있게 해줍니다.

```csharp
Expression<Func<int, bool>> isEven = n => n % 2 == 0;
// 트리 구조로 분해 가능
BinaryExpression body = (BinaryExpression)isEven.Body;
```

EF Core가 C# 쿼리를 SQL로 바꾸는 원리도 바로 이 Expression 트리입니다.
실행 코드 예제에서는 Expression 트리를 직접 구성하고, 파라미터로 컴파일해서
실행하는 방법을 보여줍니다.

## 커스텀 LINQ 연산자

`IEnumerable<T>`에 대한 확장 메서드를 직접 작성하면 체이닝에 끼워 넣을 수 있는
커스텀 연산자를 만들 수 있습니다. 예: 지연 실행을 보장하는 `WhereNotNull`,
`DistinctBy` 직접 구현.

## GroupJoin

두 컬렉션을 관계로 연결하는 조인 연산입니다. SQL의 LEFT JOIN과 유사하게
왼쪽 요소마다 오른쪽 컬렉션(중첩 시퀀스)을 매칭합니다.

```csharp
var result = from product in products
             join order in orders on product.Id equals order.ProductId into g
             select new { product.Name, Count = g.Count() };
```

## 기타 고급 연산자

- `Zip` — 두 시퀀스를 인덱스 단위로 병합
- `Aggregate` — 누적 연산 (fold)
- `ToDictionary` / `ToLookup` — 인덱싱된 컬렉션 생성
- `SelectMany` — 중첩 시퀀스 평탄화

## 실행

```bash
dotnet run
```

## 핵심 요약

- `Expression<Func<T>>`는 코드를 트리 데이터로 표현하며, EF Core의 기반입니다.
- 커스텀 LINQ 연산자는 `IEnumerable<T>` 확장 메서드로 손쉽게 만들 수 있습니다.
- `GroupJoin`은 일대다 관계 컬렉션을 다룰 때 유용합니다.

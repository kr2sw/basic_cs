# 22: 고급 LINQ — Advanced LINQ

## 소개

기초 챕터의 LINQ(Where, Select, Join 등)를 넘어 GroupJoin, Aggregate, Expression 트리 개념을 다룹니다. 이들은 보고서 생성, 데이터 집계, ORM(예: EF Core)의 쿼리 변환 원리를 이해하는 데 핵심입니다.

## 주요 개념

### 1. GroupJoin — 그룹 조인

`Join`이 조건이 맞는 쌍만 남기는 반면, `GroupJoin`은 왼쪽 항목을 기준으로 오른쪽 항목을 그룹으로 묶습니다. `Left Join`처럼 빈 그룹도 보존됩니다.

```vb
Dim grouped = From c In customers
              Group Join o In orders On c.Id Equals o.CustomerId Into OrderList = Group
              Select New With {.Customer = c, .OrderList = OrderList}
```

주문이 없는 고객도 `OrderList`가 빈 그룹으로 나타납니다.

### 2. Aggregate — 누적 집계

시퀀스 전체를 하나의 값으로 접어(fold)내는 메서드입니다. 첫 인자는 시드(seed) 값입니다.

```vb
Dim sum = numbers.Aggregate(0, Function(acc, n) acc + n)
Dim max = numbers.Aggregate(Function(acc, n) If(n > acc, n, acc))
```

`Sum`, `Average` 등이 이 Aggregate의 특수화입니다.

### 3. Expression (식 트리) 개념

`Expression(Of Func(Of ...))`은 람다를 실행 코드가 아니라 **데이터(트리)**로 보관합니다. EF Core 같은 ORM은 이 트리를 해석해서 SQL로 변환합니다. `Func(Of ...)`와 달리 `Expression`은 `.Compile()`을 호출해야 실행됩니다.

```vb
Dim expr As Expression(Of Func(Of Integer, Integer)) = Function(x) x * 2 + 1
Dim compiled = expr.Compile()
Console.WriteLine(compiled(5))      ' 11
```

`IQueryable(Of T)`는 이 원리로 SQL을 생성합니다.

### 4. Zip / SelectMany

- `Zip`: 두 시퀀스를 쌍으로 묶습니다.
- `SelectMany`: 컬렉션을 평평하게(flatten) 펼칩니다.

```vb
Dim zipped = names.Zip(values, Function(n, v) $"{n}={v}")
Dim words = sentences.SelectMany(Function(s) s.Split(" "c))
```

### 5. ToLookup — 즉석 그룹화

`GroupBy`를 즉시 실행(즉시 실행)하고 인덱서로 접근 가능하게 만듭니다. 키가 없으면 빈 시퀀스를 반환합니다.

```vb
Dim byCustomer = orders.ToLookup(Function(o) o.CustomerId)
byCustomer(3).Count()
```

## 실행

```bash
dotnet run
```

## 정리

- `GroupJoin`은 1:N 관계를 왼쪽 기준으로 묶을 때 사용합니다.
- `Aggregate`로 합, 곱, 최대값 등 임의의 누적 집계를 구현합니다.
- `Expression(Of T)`은 ORM의 SQL 변환 원리이므로 반드시 이해해야 합니다.
- `SelectMany`/`Zip`/`ToLookup`은 조합 데이터를 다룰 때 자주 쓰입니다.

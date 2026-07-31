# 32: 병렬 프로그래밍 — Parallel.For, PLINQ, 동기화

## 소개

CPU 코어를 활용해 대량 작업을 병렬로 처리하는 기법을 다룹니다. `Parallel.For`, PLINQ(`AsParallel`), 그리고 병렬 환경에서 필수인 동기화(Interlocked, Concurrent 컬렉션)를 살펴봅니다.

## 주요 개념

### 1. Parallel.For / Parallel.ForEach

데이터가 많고 각 항목이 독립적인 경우 병렬 반복을 사용합니다. 스레드 풀을 자동으로 활용합니다.

```vb
Dim squares As New ConcurrentBag(Of Integer)
Parallel.For(1, 100001, Sub(i) squares.Add(i * i))
```

### 2. PLINQ — 병렬 LINQ

LINQ 쿼리에 `.AsParallel()`을 붙이면 쿼리가 병렬 실행됩니다. 결과 순서가 필요하면 `.AsOrdered()`를 추가합니다.

```vb
Dim sum = numbers.AsParallel()
                 .Where(Function(n) n Mod 2 = 0)
                 .Sum(Function(n) CLng(n))
```

### 3. 동기화 — 공유 상태 보호

병렬 코드에서 **공유 변수를 함부로 수정하면** 경쟁 조건(race condition)이 발생합니다.

```vb
Dim counter = 0
Parallel.For(0, 100000, Sub(i) Interlocked.Increment(counter))   ' 안전
' Parallel.For(0, 100000, Sub(i) counter += 1)                   ' 위험!

Dim bag As New ConcurrentBag(Of Integer)      ' 스레드 안전 컬렉션
Dim dict As New ConcurrentDictionary(Of String, Integer)()
```

### 4. 과잉 병렬화 주의

작업이 너무 작으면 병렬 전환 오버헤드가 오히려 느려집니다. 대상 작업이 충분히 크거나 I/O 병목이 있을 때 효과적입니다. 사용자 정의 동시성은 `WithDegreeOfParallelism`으로 조절합니다.

```vb
numbers.AsParallel().WithDegreeOfParallelism(4)
```

## 실행

```bash
dotnet run
```

## 정리

- `Parallel.For`/`AsParallel()`로 CPU 바운드 작업을 병렬화합니다.
- 공유 상태 변경은 반드시 `Interlocked`나 Concurrent 컬렉션으로 동기화합니다.
- 결과 순서가 중요하면 `AsOrdered()`를 사용합니다.
- 작업 크기와 오버헤드를 고려해 병렬화 여부를 결정합니다.

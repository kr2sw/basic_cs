# 28: 성능 최적화 — Performance

성능을 측정하고 병목을 찾아 개선하는 방법을 학습합니다. 측정 없이 최적화하면
안 됩니다 — 항상 `Stopwatch`(또는 BenchmarkDotNet)로 먼저 재보세요.

## Stopwatch 벤치마크

`Stopwatch`는 가장 기본적인 측정 도구입니다. 실제 프로덕션에서는
BenchmarkDotNet(NuGet)을 권장합니다.

```csharp
var sw = Stopwatch.StartNew();
DoWork();
sw.Stop();
Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
```

## 컬렉션 선택 기준

| 자료구조 | 검색 | 추가/삭제 | 용도 |
|---------|------|----------|------|
| `List<T>` | O(n) | 끝 추가 O(1) | 일반 순차 접근 |
| `Dictionary<K,V>` | O(1) | O(1) | 키 기반 검색 |
| `HashSet<T>` | O(1) | O(1) | 중복 제거·집합 |
| `SortedSet<T>` | O(log n) | O(log n) | 정렬 유지 |
| `Stack/Queue` | - | O(1) | LIFO/FIFO |

## 실행

```bash
dotnet run
```

## 핵심 요약

- 측정 없이 최적화하지 마세요. `Stopwatch`로 시작 지점을 정합니다.
- 작업에 맞는 컬렉션 선택이 무조건적인 미세 최적화보다 효과적입니다.
- `StringBuilder`, `Span`, 배열 예열(preallocation) 등으로 할당을 줄입니다.

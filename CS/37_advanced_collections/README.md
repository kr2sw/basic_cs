# 37: 고급 컬렉션 — Advanced Collections

표준 컬렉션을 넘어, 고급 시나리오에 맞는 컬렉션들을 학습합니다.

## 불변 컬렉션 (Immutable Collections)

불변 컬렉션은 생성 후 변경이 불가능합니다. 대신 "변경"을 하면 내부 구조를
공유한 새 컬렉션이 반환됩니다. 스레드 안전성이 높고 부수 효과가 없습니다.

```csharp
using System.Collections.Immutable;

ImmutableArray<int> arr = ImmutableArray.Create(1, 2, 3);
ImmutableArray<int> arr2 = arr.Add(4); // arr은 그대로, arr2는 새 컬렉션
```

- `ImmutableArray<T>` — 배열 기반 (조회 빠름)
- `ImmutableList<T>`, `ImmutableDictionary<K,V>` — 트리 구조 공유

## Channel

스레드 안전한 생산자-소비자 큐입니다. `Writer`/`Reader` API로 데이터를
넣고 스트리밍으로 꺼냅니다. (34장 백그라운드 서비스에서 더 자세히)

## PriorityQueue

.NET 6+부터 제공되는 우선순위 큐입니다. 우선순위가 낮을수록(기본) 먼저
나옵니다. 다익스트라 알고리즘, 작업 스케줄링에 유용합니다.

```csharp
var pq = new PriorityQueue<string, int>();
pq.Enqueue("낮음", 3);
pq.Enqueue("높음", 1);
pq.Dequeue(); // "높음"
```

## 실행

```bash
dotnet run
```

## 핵심 요약

- 불변 컬렉션은 공유·병렬 환경에서 안전한 데이터를 제공합니다.
- `Channel`은 생산자-소비자 패턴의 안전한 파이프라인입니다.
- `PriorityQueue`는 우선순위 기반 처리가 필요할 때 사용합니다.

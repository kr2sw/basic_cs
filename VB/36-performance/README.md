# 36: 성능 최적화 — StringBuilder, 컬렉션 튜닝, GC

## 소개

VB.NET 애플리케이션의 성능을 좌우하는 실용 기법을 다룹니다. 문자열 처리(StringBuilder), 컬렉션 선택, 그리고 가비지 컬렉터(GC)의 동작 원리를 측정(Stopwatch)을 통해 확인합니다.

## 주요 개념

### 1. StringBuilder vs 문자열 연결

문자열은 **불변(immutable)**입니다. `&`로 연결하면 매번 새 객체가 생성됩니다. 반복 연결은 `StringBuilder`를 사용하고, 미리 용량을 지정하면 재할당도 줄일 수 있습니다.

```vb
Dim sb As New StringBuilder(capacity:=iterations)
sb.Append("a")
```

### 2. 컬렉션 선택

| 목적 | 추천 컬렉션 |
|------|------------|
| 인덱스 순차 접근 | `List(Of T)` |
| 존재 여부 검사 | `HashSet(Of T)` |
| 키 조회 | `Dictionary(Of K, V)` |
| 선입선출/후입선출 | `Queue(Of T)` / `Stack(Of T)` |

`List.Contains`는 O(n)인 반면 `HashSet.Contains`는 O(1)입니다. 데이터가 크면 차이가 큽니다.

### 3. GC(가비지 컬렉터) 이해

- 힙에 할당된 객체는 더 이상 참조되지 않으면 GC가 수집합니다.
- 세대(Generation): 0세대가 짧게 살고, 오래 살수록 높은 세대. LOH(대형 개체 힙, 85KB 이상)는 별도 관리.
- `GC.Collect()`는 원칙적으로 호출하지 않는 것이 좋습니다(성능 저하).
- `Using`/`Dispose`로 비관리 리소스는 즉시 해제합니다.

### 4. 측정과 프로파일링

```vb
Dim sw = Stopwatch.StartNew()
' ...
sw.Stop()
```

큰 컬렉션은 `New List(Of Integer)(10000)`처럼 초기 용량을 지정해 재할당을 피합니다.

## 실행

```bash
dotnet run
```

## 정리

- 반복 문자열 결합은 `StringBuilder`, 검색이 잦으면 `HashSet`/`Dictionary`.
- GC를 직접 부르지 말고, 객체 수명(세대)을 이해하세요.
- `Stopwatch`로 항상 측정해 최적화 효과를 검증합니다.
- 마이크로 최적화보다 컬렉션/알고리즘 선택이 훨씬 큰 영향을 줍니다.

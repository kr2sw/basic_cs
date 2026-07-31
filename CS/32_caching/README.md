# 32: 캐싱 — Caching

반복적으로 계산·조회되는 결과를 임시 저장해 성능을 높이는 캐싱을 학습합니다.
ASP.NET Core의 `IMemoryCache` 개념을 익히고, **LRU(Least Recently Used) 캐시**를
직접 구현합니다.

## IMemoryCache 개념

메모리 캐시는 키-값 형태로 객체를 저장하고 만료 시간(절대/슬라이딩)을 지원합니다.

```csharp
builder.Services.AddMemoryCache();   // DI 등록

var data = cache.GetOrCreateAsync("users", entry =>
{
    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
    return FetchUsersAsync();         // 캐시 미스 시에만 실행
});
```

## LRU 캐시

캐시가 가득 차면 **가장 오래 사용되지 않은 항목**을 제거하는 정책입니다.
`LinkedList<T>`로 접근 순서를 추적하고 `Dictionary`로 O(1) 조회를 제공합니다.

## 실행

```bash
dotnet run
```

## 핵심 요약

- 캐시는 자주 쓰는 결과를 저장해 DB/계산 비용을 줄입니다.
- `IMemoryCache`는 만료 시간과 크기 제한을 관리합니다.
- LRU는 접근 순서를 추적해 낡은 항목부터 제거합니다.

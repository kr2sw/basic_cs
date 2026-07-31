# 24: 고급 비동기 — Advanced Async

기초 async/await를 넘어, 실제 프로덕션에서 쓰이는 고급 비동기 API들을 학습합니다.

## ValueTask

`Task`는 클래스라서 완료된 작업도 힙에 객체를 할당합니다. `ValueTask`는 구조체라
결과를 이미 알고 있는 경우(캐시 등) **할당 없이** 반환할 수 있습니다.
`IValueTaskSource`에 기반해 한 번만 기다릴 수 있는 제약이 있습니다.

```csharp
async ValueTask<int> GetCachedAsync(int key)
{
    return _cache.TryGetValue(key, out int v) ? v : await FetchAsync(key);
}
```

## IAsyncEnumerable

여러 개의 결과가 시간에 걸쳐 도착할 때 사용하는 **비동기 스트림**입니다.
`yield return`을 `async` 메서드에서 사용하고, `await foreach`로 소비합니다.

```csharp
await foreach (int n in GenerateAsync(10)) { }
```

## CancellationToken

장기 실행 작업을 협력적으로 취소하는 방법입니다. `CancellationTokenSource`를
만들어 토큰을 전달하고, 작업 루프에서 주기적으로 취소 여부를 확인합니다.

## 실행

```bash
dotnet run
```

## 핵심 요약

- `ValueTask`는 완료된 값 반환에서 힙 할당을 줄입니다.
- `IAsyncEnumerable` + `await foreach`는 스트리밍 비동기 처리에 사용합니다.
- `CancellationToken`으로 안전하게 작업을 취소하고 리소스를 정리합니다.

# 13 비동기 프로그래밍 (Async/Await)

C#의 `async` / `await` 키워드를 사용한 비동기 프로그래밍을 학습합니다.

## 주요 개념

- `async Task` / `async Task<T>` 메서드
- `await` — 비동기 작업 완료 대기
- `Task.Delay()` — 작업 시뮬레이션
- `Task.WhenAll()` — 여러 작업 병렬 실행
- `Task.WhenAny()` — 가장 먼저 완료되는 작업 처리
- `CancellationToken` — 작업 취소
- `IProgress<T>` / `Progress<T>` — 진행률 보고
- `ValueTask` — 성능 최적화
- `IAsyncEnumerable<T>` — 비동기 스트림 (`await foreach`)

## 예제 코드

```csharp
static async Task<int> GetContentLengthAsync(string url)
{
    using var client = new HttpClient();
    string content = await client.GetStringAsync(url);
    return content.Length;
}

await Task.WhenAll(
    SimulateWorkAsync("Task A", 1200),
    SimulateWorkAsync("Task B", 800));

await foreach (int number in GenerateNumbersAsync(5)) { }
```

## 실행 방법

```bash
dotnet run --project ../13_async_await
```

## 핵심 요약

- `async` / `await`는 비동기 코드를 동기 코드처럼 읽을 수 있게 합니다.
- `Task.WhenAll`로 병렬 처리를, `Task.WhenAny`로 경쟁 패턴을 구현합니다.
- `CancellationToken`으로 장기 실행 작업을 안전하게 취소할 수 있습니다.

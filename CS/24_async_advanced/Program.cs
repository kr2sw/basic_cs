namespace BasicCS.Chapter24;

static class Program
{
    // ---- ValueTask: 캐시가 있으면 할당 없이 즉시 반환 ----
    private static readonly Dictionary<int, int> Cache = new();
    private static int _fetchCount;

    static async ValueTask<int> GetAsync(int key)
    {
        // 캐시 히트 시 Task 할당 없이 완료된 값을 반환
        if (Cache.TryGetValue(key, out int cached))
            return cached;

        // 미스 시에만 실제 비동기 작업 수행
        await Task.Delay(100); // 네트워크 호출 시뮬레이션
        _fetchCount++;
        Cache[key] = key * 10;
        return key * 10;
    }

    // ---- IAsyncEnumerable: 비동기 스트림 생성 ----
    static async IAsyncEnumerable<int> GenerateNumbersAsync(int count, CancellationToken ct)
    {
        for (int i = 1; i <= count; i++)
        {
            await Task.Delay(150, ct); // ct 전달로 취소에 즉시 반응
            yield return i * i;        // 하나씩 스트리밍
        }
    }

    // ---- CancellationToken: 협력적 취소 ----
    static async Task LongRunningWorkAsync(CancellationToken ct)
    {
        int i = 0;
        try
        {
            while (i < 20)
            {
                ct.ThrowIfCancellationRequested(); // 취소 시 OperationCanceledException
                await Task.Delay(200, ct);
                Console.WriteLine($"  작업 진행 중... ({++i}/20)");
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("  작업이 취소되었습니다. 정리 작업 수행...");
        }
    }

    static async Task Main()
    {
        // ---- ValueTask 사용 ----
        Console.WriteLine("[ValueTask] 캐시된 값 반환 (할당 최소화)");
        int v1 = await GetAsync(5);
        int v2 = await GetAsync(5);
        int v3 = await GetAsync(7);
        Console.WriteLine($"  v1={v1}, v2={v2}(캐시), v3={v3} — 실제 fetch 횟수: {_fetchCount}");

        // ---- IAsyncEnumerable + await foreach ----
        Console.WriteLine("\n[IAsyncEnumerable] 1~5 제곱 스트리밍");
        using var cts = new CancellationTokenSource();
        await foreach (int square in GenerateNumbersAsync(5, cts.Token))
            Console.WriteLine($"  받은 값: {square}");

        // ---- CancellationToken 취소 예제 ----
        Console.WriteLine("\n[CancellationToken] 600ms 후 취소");
        using var cancelCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(600));
        await LongRunningWorkAsync(cancelCts.Token);

        // ---- Task.WhenAll / WhenAny 병렬 처리 ----
        Console.WriteLine("\n[병렬 처리] WhenAll로 3개 다운로드 시뮬레이션");
        var downloads = Enumerable.Range(1, 3).Select(i => DownloadAsync(i));
        var results = await Task.WhenAll(downloads);
        foreach (var r in results)
            Console.WriteLine($"  {r}");

        Console.WriteLine("\n완료");
    }

    static async Task<string> DownloadAsync(int fileId)
    {
        await Task.Delay(300);
        return $"파일 {fileId} 다운로드 완료";
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BasicCS.Chapter13;

internal class AsyncAwait
{
    static async Task Main()
    {
        Console.WriteLine("=== async/await 기본 예제 ===\n");

        // ---- 1. 기본 async Task 메서드 ----
        Console.WriteLine("1. 기본 async Task 메서드");
        await BasicAsyncMethod();

        // ---- 2. async Task<T> 반환값 ----
        Console.WriteLine("\n2. async Task<T> 반환값");
        int length = await GetContentLengthAsync("https://example.com");
        Console.WriteLine($"Content length: {length}");

        // ---- 3. Task.Delay() 시뮬레이션 ----
        Console.WriteLine("\n3. Task.Delay() 시뮬레이션");
        var sw = Stopwatch.StartNew();
        await SimulateWorkAsync(1500);
        sw.Stop();
        Console.WriteLine($"Simulated work completed in {sw.ElapsedMilliseconds}ms");

        // ---- 4. Task.WhenAll ----
        Console.WriteLine("\n4. Task.WhenAll (병렬 실행)");
        sw.Restart();
        await Task.WhenAll(
            SimulateWorkAsync("Task A", 1200),
            SimulateWorkAsync("Task B", 800),
            SimulateWorkAsync("Task C", 1000)
        );
        sw.Stop();
        Console.WriteLine($"All tasks completed in {sw.ElapsedMilliseconds}ms (병렬 처리)");

        // ---- 5. Task.WhenAny ----
        Console.WriteLine("\n5. Task.WhenAny (가장 먼저 완료되는 작업)");
        Task<string> fast = SlowOperationAsync("Fast", 500);
        Task<string> slow = SlowOperationAsync("Slow", 2000);
        Task<string> completed = await Task.WhenAny(fast, slow);
        Console.WriteLine($"First completed: {completed.Result}");

        // ---- 6. Task.Run (CPU 바운드 작업) ----
        Console.WriteLine("\n6. Task.Run (CPU 바운드 작업을 별도 스레드에서 실행)");
        int cpuResult = await Task.Run(() => ComputeFibonacci(40));
        Console.WriteLine($"Fibonacci(40) = {cpuResult} (CPU intensive task)");

        // ---- 7. CancellationToken을 이용한 취소 ----
        Console.WriteLine("\n7. CancellationToken을 이용한 작업 취소");
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(500); // 500ms 후 취소

        try
        {
            await LongRunningTaskAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Task was cancelled (OperationCanceledException)");
        }

        // ---- 8. async/await와 try-catch 예외 처리 ----
        Console.WriteLine("\n8. async/await 예외 처리");
        try
        {
            await FaultyOperationAsync();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Caught exception: {ex.Message}");
        }

        // ---- 9. IProgress<T> / Progress<T> (진행률 보고) ----
        Console.WriteLine("\n9. IProgress<T> / Progress<T> 진행률 보고");
        var progress = new Progress<int>(p =>
            Console.WriteLine($"  Progress: {p}%"));
        await LongProgressOperationAsync(progress, 5);

        // ---- 10. ConfigureAwait(false) ----
        Console.WriteLine("\n10. ConfigureAwait(false) - 컨텍스트 전환 방지");
        await Task.Run(() =>
        {
            // CPU 작업 수행
            int sum = Enumerable.Range(1, 100).Sum();
            return sum;
        }).ConfigureAwait(false);

        // ---- 11. ValueTask 사용 ----
        Console.WriteLine("\n11. ValueTask (성능 최적화)");
        int cached = await GetOrFetchCachedAsync();
        Console.WriteLine($"Cached value: {cached}");
        int cachedAgain = await GetOrFetchCachedAsync(); // 즉시 반환
        Console.WriteLine($"Cached value (again): {cachedAgain}");

        // ---- 12. async Stream (IAsyncEnumerable) ----
        Console.WriteLine("\n12. async stream (IAsyncEnumerable)");
        await foreach (int number in GenerateNumbersAsync(5))
        {
            Console.WriteLine($"  Yielded: {number}");
        }

        Console.WriteLine("\n=== All async/await examples completed ===");
    }

    // ---- 기본 async Task 메서드 ----
    static async Task BasicAsyncMethod()
    {
        Console.WriteLine("  BasicAsyncMethod started");
        await Task.Delay(300);
        Console.WriteLine("  BasicAsyncMethod completed");
    }

    // ---- Task<T> 반환 ----
    static async Task<int> GetContentLengthAsync(string url)
    {
        using var client = new HttpClient();
        string content = await client.GetStringAsync(url);
        Console.WriteLine($"  Fetched {url}: {content.Length} chars");
        return content.Length;
    }

    // ---- Task.Delay로 작업 시뮬레이션 ----
    static async Task SimulateWorkAsync(int ms)
    {
        Console.WriteLine($"  Working for {ms}ms...");
        await Task.Delay(ms);
    }

    // ---- 이름 포함 시뮬레이션 ----
    static async Task SimulateWorkAsync(string name, int ms)
    {
        Console.WriteLine($"  {name} started ({ms}ms)");
        await Task.Delay(ms);
        Console.WriteLine($"  {name} completed");
    }

    // ---- WhenAny용 작업 ----
    static async Task<string> SlowOperationAsync(string name, int ms)
    {
        await Task.Delay(ms);
        return name;
    }

    // ---- CPU 바운드 작업 (Fibonacci) ----
    static int ComputeFibonacci(int n)
    {
        if (n <= 1) return n;
        return ComputeFibonacci(n - 1) + ComputeFibonacci(n - 2);
    }

    // ---- CancellationToken을 지원하는 장기 실행 작업 ----
    static async Task LongRunningTaskAsync(CancellationToken ct)
    {
        for (int i = 0; i < 10; i++)
        {
            ct.ThrowIfCancellationRequested();
            Console.WriteLine($"  LongRunningTask: iteration {i + 1}");
            await Task.Delay(200, ct);
        }
        Console.WriteLine("  LongRunningTask completed normally");
    }

    // ---- 예외를 던지는 비동기 메서드 ----
    static async Task FaultyOperationAsync()
    {
        await Task.Delay(100);
        throw new InvalidOperationException("Something went wrong in async method!");
    }

    // ---- IProgress<T> 사용 ----
    static async Task LongProgressOperationAsync(IProgress<int> progress, int steps)
    {
        for (int i = 1; i <= steps; i++)
        {
            await Task.Delay(300);
            progress.Report(i * 100 / steps);
        }
    }

    // ---- ValueTask 예제 (캐싱) ----
    static int _cachedValue;
    static bool _cachePopulated;

    static async ValueTask<int> GetOrFetchCachedAsync()
    {
        if (_cachePopulated)
            return _cachedValue;

        await Task.Delay(200);
        _cachedValue = 42;
        _cachePopulated = true;
        return _cachedValue;
    }

    // ---- async stream (IAsyncEnumerable) ----
    static async IAsyncEnumerable<int> GenerateNumbersAsync(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            await Task.Delay(200);
            yield return i * 10;
        }
    }
}

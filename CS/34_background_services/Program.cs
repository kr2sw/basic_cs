using System.Threading.Channels;

namespace BasicCS.Chapter34;

/*
 * 실제 IHostedService 예제 (NuGet: Microsoft.Extensions.Hosting):
 *
 * var builder = Host.CreateApplicationBuilder(args);
 * builder.Services.AddHostedService<EmailQueueWorker>();   // 시작/종료 자동
 * var host = builder.Build();
 * await host.RunAsync();
 *
 * public class EmailQueueWorker : BackgroundService
 * {
 *     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
 *     {
 *         while (!stoppingToken.IsCancellationRequested)
 *         {
 *             await DoWorkAsync();
 *             await Task.Delay(1000, stoppingToken);
 *         }
 *     }
 * }
 */

// ---- 채널 기반 이메일 큐 ----
public class EmailQueue
{
    private readonly Channel<string> _channel =
        Channel.CreateUnbounded<string>();

    // 생산자: 이메일 전송 요청을 큐에 넣는다
    public void Enqueue(string email)
    {
        _channel.Writer.TryWrite(email);
        Console.WriteLine($"  [생산자] {email} 큐에 추가");
    }

    // 소비자: 큐에서 하나씩 꺼내어 처리
    public async Task ProcessAsync(CancellationToken ct)
    {
        await foreach (var email in _channel.Reader.ReadAllAsync(ct))
        {
            await Task.Delay(100, ct); // SMTP 전송 시뮬레이션
            Console.WriteLine($"  [소비자] {email} 전송 완료");
        }
    }

    public void Complete() => _channel.Writer.TryComplete();
}

// ---- BackgroundService 흉내: 앱 수명 주기와 함께 시작/종료 ----
public class EmailBackgroundService
{
    private readonly EmailQueue _queue;
    private CancellationTokenSource? _cts;
    private Task? _worker;

    public EmailBackgroundService(EmailQueue queue) => _queue = queue;

    // 호스트 시작 시 (IHostedService.StartAsync)
    public void Start()
    {
        _cts = new CancellationTokenSource();
        _worker = _queue.ProcessAsync(_cts.Token);
        Console.WriteLine("[서비스] 이메일 백그라운드 서비스 시작");
    }

    // 호스트 종료 시 (IHostedService.StopAsync)
    public async Task StopAsync()
    {
        _queue.Complete();               // 대기 중인 항목을 모두 소진
        _cts?.Cancel();
        if (_worker is not null)
            await _worker;
        Console.WriteLine("[서비스] 이메일 백그라운드 서비스 종료");
    }
}

static class Program
{
    static async Task Main()
    {
        var queue = new EmailQueue();
        var service = new EmailBackgroundService(queue);

        service.Start(); // 호스트 시작에 해당

        // 생산자가 6건의 이메일을 밀어넣는다
        for (int i = 1; i <= 6; i++)
            queue.Enqueue($"user{i}@example.com");

        Console.WriteLine();

        // 소비자가 처리하는 동안 잠시 대기
        await Task.Delay(800);

        // 호스트 종료 시뮬레이션 (graceful shutdown)
        await service.StopAsync();
        Console.WriteLine("\n[완료] 모든 대기 이메일 처리");
    }
}

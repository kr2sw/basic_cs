namespace BasicCS.Chapter33;

/*
 * 실제 Serilog 예제 (NuGet: Serilog.AspNetCore):
 *
 * Log.Logger = new LoggerConfiguration()
 *     .MinimumLevel.Debug()
 *     .WriteTo.Console()                                   // 콘솔 싱크
 *     .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
 *     .Enrich.FromLogContext()                             // 컨텍스트 확장
 *     .CreateLogger();
 *
 * builder.Host.UseSerilog();
 *
 * // 구조적 로깅: {OrderId}는 이름 있는 값으로 저장됨
 * Log.Information("주문 {OrderId} 완료, 금액 {Amount:C0}", 1001, 50000m);
 */

// ---- 로그 레벨 ----
public enum LogLevel { Trace, Debug, Information, Warning, Error, Critical }

// ---- 커스텀 로거 인터페이스 ----
public interface IAppLogger
{
    void Log(LogLevel level, string message, params object?[] args);
    void Info(string message, params object?[] args) => Log(LogLevel.Information, message, args);
    void Warn(string message, params object?[] args) => Log(LogLevel.Warning, message, args);
    void Error(string message, params object?[] args) => Log(LogLevel.Error, message, args);
}

// ---- 커스텀 로거 구현: 레벨 필터 + 구조적 메시지 + 파일 출력 ----
public class FileLogger : IAppLogger
{
    private readonly LogLevel _minLevel;
    private readonly string _filePath;
    private static readonly object Sync = new();

    public FileLogger(LogLevel minLevel, string filePath)
    {
        _minLevel = minLevel;
        _filePath = filePath;
    }

    public void Log(LogLevel level, string message, params object?[] args)
    {
        if (level < _minLevel) return; // 레벨 필터링

        // 구조적 로깅: {자리} 를 이름+값 형태로 치환
        string formatted = FormatTemplate(message, args);
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {formatted}";

        Console.WriteLine(line);

        // 파일 싱크 (Serilog의 WriteTo.File 흉내)
        lock (Sync)
            File.AppendAllText(_filePath, line + Environment.NewLine);
    }

    // "{OrderId}" 자리를 "OrderId=1001" 형태로 채운다
    private static string FormatTemplate(string template, object?[] args)
    {
        var regex = new System.Text.RegularExpressions.Regex(@"\{(\w+)\}");
        int idx = 0;
        return regex.Replace(template, m =>
            idx < args.Length ? $"{m.Groups[1].Value}={args[idx++]}" : m.Value);
    }
}

// ---- 로그 사용처 ----
public class OrderProcessor
{
    private readonly IAppLogger _logger;

    public OrderProcessor(IAppLogger logger) => _logger = logger;

    public void Process(int orderId, decimal amount)
    {
        _logger.Log(LogLevel.Debug, "OrderProcessor 시작");
        if (amount < 0)
        {
            _logger.Error("주문 {OrderId} 실패: 금액이 음수입니다", orderId);
            return;
        }
        _logger.Info("주문 {OrderId} 완료, 금액 {Amount:C0}", orderId, amount);
        _logger.Warn("주문 {OrderId} 수량이 많아 배송이 지연될 수 있음", orderId);
    }
}

static class Program
{
    static void Main()
    {
        string logFile = Path.Combine(AppContext.BaseDirectory, "app.log");
        var logger = new FileLogger(LogLevel.Debug, logFile);
        var processor = new OrderProcessor(logger);

        Console.WriteLine("[로그 시연] (콘솔 + app.log 파일 동시 출력)\n");
        processor.Process(1001, 50000m);
        processor.Process(1002, -1);

        Console.WriteLine($"\n파일에 기록된 로그 (마지막 3줄):");
        var lines = File.ReadAllLines(logFile).Reverse().Take(3);
        foreach (var line in lines)
            Console.WriteLine($"  {line}");

        Console.WriteLine("\n[참고] Serilog은 위 패턴을 싱크 확장으로 제공합니다.");
    }
}

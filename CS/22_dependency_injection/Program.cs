using System.Collections.Concurrent;

namespace BasicCS.Chapter22;

// ---- 추상화 (인터페이스) ----
public interface ILogger
{
    void Log(string message);
}

public interface IEmailSender
{
    void Send(string to, string body);
}

// ---- 구현체 ----
public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[LOG] {DateTime.Now:HH:mm:ss} {message}");
}

public class SmtpEmailSender : IEmailSender
{
    private readonly ILogger _logger;
    public SmtpEmailSender(ILogger logger) => _logger = logger;

    public void Send(string to, string body)
    {
        // 실제 SMTP 호출 대신 콘솔로 시뮬레이션
        _logger.Log($"이메일 전송 -> {to}: {body}");
    }
}

// ---- 수동 DI 컨테이너 구현 ----
public enum ServiceLifetime { Transient, Singleton }

public sealed class ServiceCollection
{
    private readonly Dictionary<Type, (Type Impl, ServiceLifetime Lifetime)> _map = new();

    public ServiceCollection AddSingleton<TInterface, TImpl>() where TImpl : class
    {
        _map[typeof(TInterface)] = (typeof(TImpl), ServiceLifetime.Singleton);
        return this;
    }

    public ServiceCollection AddTransient<TInterface, TImpl>() where TImpl : class
    {
        _map[typeof(TInterface)] = (typeof(TImpl), ServiceLifetime.Transient);
        return this;
    }

    public ServiceProvider BuildProvider() => new(_map);
}

public sealed class ServiceProvider
{
    private readonly Dictionary<Type, (Type Impl, ServiceLifetime Lifetime)> _map;
    private readonly ConcurrentDictionary<Type, object> _singletons = new();

    public ServiceProvider(Dictionary<Type, (Type Impl, ServiceLifetime Lifetime)> map) => _map = map;

    public T GetService<T>() where T : class => (T)Resolve(typeof(T));

    private object Resolve(Type interfaceType)
    {
        // 등록된 구현체가 없으면 기본 생성자로 시도
        if (!_map.TryGetValue(interfaceType, out var entry))
        {
            return CreateInstance(interfaceType);
        }

        if (entry.Lifetime == ServiceLifetime.Singleton)
            return _singletons.GetOrAdd(interfaceType, _ => CreateInstance(entry.Impl));

        return CreateInstance(entry.Impl);
    }

    private object CreateInstance(Type type)
    {
        // 파라미터가 가장 많은 public 생성자를 선택
        var ctor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First();
        var args = ctor.GetParameters()
                       .Select(p => Resolve(p.ParameterType))
                       .ToArray();
        return ctor.Invoke(args);
    }
}

// ---- 사용처: OrderService는 IEmailSender, ILogger를 주입받음 ----
public class OrderService
{
    private readonly IEmailSender _sender;
    private readonly ILogger _logger;

    public OrderService(IEmailSender sender, ILogger logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public void PlaceOrder(string customer, string product)
    {
        _logger.Log($"주문 접수: {customer}님의 {product}");
        _sender.Send(customer, $"주문하신 {product}가 접수되었습니다.");
    }
}

static class Program
{
    static void Main()
    {
        // ---- 컨테이너 설정 ----
        var services = new ServiceCollection();
        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddTransient<IEmailSender, SmtpEmailSender>();
        services.AddTransient<OrderService, OrderService>();

        var provider = services.BuildProvider();

        // ---- 생성자 주입으로 그래프 자동 생성 ----
        var orderService = provider.GetService<OrderService>();
        orderService.PlaceOrder("홍길동", "노트북");

        // ---- 싱글턴: 같은 인스턴스 반환 ----
        var logger1 = provider.GetService<ILogger>();
        var logger2 = provider.GetService<ILogger>();
        Console.WriteLine($"\n싱글턴 확인: 같은 인스턴스? {ReferenceEquals(logger1, logger2)}");

        // ---- 트랜지언트: 매번 새 인스턴스 ----
        var order1 = provider.GetService<OrderService>();
        var order2 = provider.GetService<OrderService>();
        Console.WriteLine($"트랜지언트 확인: 같은 인스턴스? {ReferenceEquals(order1, order2)}");

        // ---- 인터페이스 없이 구체 타입도 등록/해석 가능 ----
        var consoleLogger = provider.GetService<ConsoleLogger>();
        consoleLogger.Log("구체 타입 해석 성공");
    }
}

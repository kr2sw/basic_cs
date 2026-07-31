namespace BasicCS.Chapter35;

/*
 * 실제 HTTP 기반 마이크로서비스 예제 (NuGet: Microsoft.Extensions.Http):
 *
 * // 주문 서비스가 결제 서비스 호출
 * builder.Services.AddHttpClient("payment", client =>
 * {
 *     client.BaseAddress = new Uri("http://payment-service/api");
 *     client.Timeout = TimeSpan.FromSeconds(5);   // 타임아웃
 * });
 *
 * // Polly 등을 이용한 재시도/회로 차단으로 부분 장애 대응
 * var payment = httpClient.PostAsJsonAsync("/payments", order);
 *
 * // 서비스 레지스트리: Consul, etcd 등이 대표적
 * // 인스턴스가 시작되면 /register로 등록, 정기적 heartbeat, 종료 시 해지
 */

// ---- 서비스 레지스트리 ----
public class ServiceRegistry
{
    private readonly Dictionary<string, List<string>> _services = new();
    private readonly Dictionary<string, int> _roundRobin = new();

    public void Register(string serviceName, string url)
    {
        if (!_services.TryGetValue(serviceName, out var list))
            _services[serviceName] = list = new List<string>();
        if (!list.Contains(url))
            list.Add(url);
        Console.WriteLine($"  [등록] {serviceName} -> {url}");
    }

    public void Unregister(string serviceName, string url)
    {
        if (_services.TryGetValue(serviceName, out var list))
        {
            list.Remove(url);
            Console.WriteLine($"  [해지] {serviceName} -> {url}");
        }
    }

    // 라운드로빈으로 인스턴스 선택 (부하 분산)
    public string? Resolve(string serviceName)
    {
        if (!_services.TryGetValue(serviceName, out var list) || list.Count == 0)
            return null;
        int idx = _roundRobin.GetValueOrDefault(serviceName) % list.Count;
        _roundRobin[serviceName] = idx + 1;
        return list[idx];
    }

    public void PrintHealth()
    {
        Console.WriteLine("\n[레지스트리 상태]");
        foreach (var (name, urls) in _services)
            Console.WriteLine($"  {name}: {(urls.Count == 0 ? "인스턴스 없음" : string.Join(", ", urls))}");
    }
}

// ---- HTTP 클라이언트 시뮬레이터 ----
public class HttpSimulator
{
    private readonly Random _rng = new();

    public string Get(string url)
    {
        // 20% 확률로 서비스 장애 시뮬레이션
        if (_rng.Next(100) < 20)
            throw new HttpRequestException($"503 Service Unavailable: {url}");
        return $"응답: {url}";
    }
}

static class Program
{
    static void Main()
    {
        var registry = new ServiceRegistry();
        var http = new HttpSimulator();

        Console.WriteLine("[1] 서비스 인스턴스 시작 -> 레지스트리 등록");
        registry.Register("order-service", "http://host1:5001");
        registry.Register("order-service", "http://host2:5001");
        registry.Register("payment-service", "http://host3:5002");

        registry.PrintHealth();

        Console.WriteLine("\n[2] 클라이언트가 레지스트리로 서비스 위치 조회 (라운드로빈)");
        for (int i = 0; i < 4; i++)
        {
            var url = registry.Resolve("order-service");
            Console.WriteLine($"  요청 {i + 1} -> {url}");
        }

        Console.WriteLine("\n[3] HTTP 호출 + 재시도(리트라이) 패턴");
        string CallWithRetry(string service, int maxRetries = 3)
        {
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                var url = registry.Resolve(service)!;
                try
                {
                    return http.Get(url);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"    시도 {attempt} 실패: {ex.Message}");
                    Thread.Sleep(100);
                }
            }
            return "최종 실패 (회로 차단)";
        }

        Console.WriteLine($"  결과: {CallWithRetry("payment-service")}");

        Console.WriteLine("\n[4] 인스턴스 종료 -> 레지스트리 해지");
        registry.Unregister("order-service", "http://host2:5001");
        registry.PrintHealth();

        Console.WriteLine("\n[참고] 실제 운영은 Consul/etcd + Kubernetes DNS 기반 발견 사용");
    }
}

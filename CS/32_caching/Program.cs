namespace BasicCS.Chapter32;

/*
 * 실제 IMemoryCache 예제 (NuGet: Microsoft.Extensions.Caching.Memory):
 *
 * var builder = WebApplication.CreateBuilder(args);
 * builder.Services.AddMemoryCache();  // IMemoryCache DI 등록
 *
 * app.MapGet("/products", (IMemoryCache cache) =>
 * {
 *     var list = cache.GetOrCreate("product-list", entry =>
 *     {
 *         entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
 *         return GetProductsFromDb();
 *     });
 *     return Results.Ok(list);
 * });
 *
 * // 만료 정책:
 * //   AbsoluteExpiration   -> 지정 시각에 무조건 만료
 * //   SlidingExpiration    -> 마지막 접근 후 일정 시간이 지나면 만료
 */

// ---- LRU 캐시 구현 ----
public class LruCache<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, LinkedListNode<(TKey Key, TValue Value)>> _map;
    private readonly LinkedList<(TKey Key, TValue Value)> _order;

    public LruCache(int capacity)
    {
        _capacity = capacity;
        _map = new Dictionary<TKey, LinkedListNode<(TKey, TValue)>>(capacity);
        _order = new LinkedList<(TKey, TValue)>();
    }

    public bool TryGet(TKey key, out TValue? value)
    {
        if (_map.TryGetValue(key, out var node))
        {
            // 접근된 항목을 맨 앞으로 이동 (가장 최근 사용 표시)
            _order.Remove(node);
            _order.AddFirst(node);
            value = node.Value.Value;
            return true;
        }
        value = default;
        return false;
    }

    public void Set(TKey key, TValue value)
    {
        if (_map.TryGetValue(key, out var existing))
        {
            _order.Remove(existing);
            _map.Remove(key);
        }
        else if (_map.Count >= _capacity)
        {
            // 가득 차면 가장 오래 안 쓰인 항목(맨 뒤) 제거
            var last = _order.Last!;
            _order.RemoveLast();
            _map.Remove(last.Value.Key);
        }

        var node = _order.AddFirst((key, value));
        _map[key] = node;
    }

    public void PrintState()
    {
        Console.Write("    캐시 내용 (최근 사용순): ");
        foreach (var item in _order)
            Console.Write($"[{item.Key}] ");
        Console.WriteLine();
    }
}

// ---- DB 조회 시뮬레이션 (비싼 작업) ----
static class Database
{
    private static int _calls;

    public static string FetchUser(string id)
    {
        _calls++;
        Thread.Sleep(50); // 비싼 작업 흉내
        return $"사용자{id}의 데이터";
    }

    public static int TotalCalls => _calls;
}

static class Program
{
    static void Main()
    {
        Console.WriteLine("== 1) LRU 캐시 기본 동작 (용량 3) ==");
        var cache = new LruCache<string, string>(3);
        cache.Set("A", "a");
        cache.Set("B", "b");
        cache.Set("C", "c");
        cache.PrintState();                 // [C] [B] [A]

        // B 접근 -> B가 가장 최근으로 이동
        cache.TryGet("B", out _);
        cache.PrintState();                 // [B] [C] [A]

        // D 추가 -> A가 퇴거
        cache.Set("D", "d");
        cache.PrintState();                 // [D] [B] [C]
        Console.WriteLine($"    A 존재 여부: {cache.TryGet("A", out _)} (false면 LRU 퇴거)");

        Console.WriteLine("\n== 2) DB 조회 캐싱 (IMemoryCache 패턴 흉내) ==");
        var userCache = new LruCache<string, string>(5);

        string GetUserCached(string id)
        {
            if (userCache.TryGet(id, out var cached))
            {
                Console.WriteLine($"    캐시 히트: {id} (DB 호출 안 함)");
                return cached!;
            }
            Console.WriteLine($"    캐시 미스: {id} -> DB 조회");
            var data = Database.FetchUser(id);
            userCache.Set(id, data);
            return data;
        }

        GetUserCached("u1");
        GetUserCached("u2");
        GetUserCached("u1");  // 히트
        GetUserCached("u3");
        GetUserCached("u2");  // 히트
        Console.WriteLine($"    DB 총 호출 횟수: {Database.TotalCalls} (캐시 덕분에 3번)");
    }
}

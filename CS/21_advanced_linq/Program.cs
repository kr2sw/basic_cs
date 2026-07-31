using System.Linq.Expressions;

namespace BasicCS.Chapter21;

// 커스텀 LINQ 연산자: null이 아닌 값만 남기는 확장 메서드
static class LinqExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
        where T : class
    {
        foreach (var item in source)
        {
            if (item is not null)
                yield return item;
        }
    }
}

record Product(int Id, string Name, string Category, decimal Price);
record Order(int OrderId, int ProductId, int Quantity);

static class Program
{
    static void Main()
    {
        var products = new[]
        {
            new Product(1, "노트북", "전자기기", 1200000m),
            new Product(2, "마우스", "전자기기", 25000m),
            new Product(3, "키보드", "전자기기", 45000m),
            new Product(4, "책상", "가구", 180000m),
            new Product(5, "의자", "가구", 90000m),
        };

        var orders = new[]
        {
            new Order(101, 1, 1),
            new Order(102, 2, 2),
            new Order(103, 1, 1),
            new Order(104, 4, 1),
        };

        // ---- Expression 트리 개념 ----
        // 람다를 Expression으로 받으면 코드를 데이터로 다룰 수 있다.
        Expression<Func<int, bool>> isEvenExpr = n => n % 2 == 0;
        Console.WriteLine($"표현식 본문: {isEvenExpr.Body}");          // (n % 2) == 0
        Console.WriteLine($"노드 종류:   {isEvenExpr.NodeType}");

        // Expression 트리를 파라미터로 전달해 컴파일 + 실행
        int Evaluate(Expression<Func<int, int>> expr, int input)
            => expr.Compile()(input);
        Console.WriteLine($"컴파일 실행: x*3+1 where x=5 -> {Evaluate(x => x * 3 + 1, 5)}");

        // Expression 트리 직접 조립 (n => n + 2)
        var param = Expression.Parameter(typeof(int), "n");
        var two = Expression.Constant(2);
        var body = Expression.Add(param, two);
        var built = Expression.Lambda<Func<int, int>>(body, param).Compile();
        Console.WriteLine($"직접 조립:   built(10) -> {built(10)}");

        // ---- GroupJoin: 일대다 관계 ----
        Console.WriteLine("\n[GroupJoin] 상품별 주문 개수");
        var orderCounts = products.GroupJoin(
            orders,
            p => p.Id,
            o => o.ProductId,
            (p, os) => new { p.Name, Count = os.Count() });

        foreach (var item in orderCounts)
            Console.WriteLine($"  {item.Name}: {item.Count}건");

        // ---- 커스텀 연산자 WhereNotNull ----
        string?[] names = { "Alice", null, "Bob", null, "Carol" };
        Console.WriteLine("\n[커스텀 연산자] WhereNotNull");
        foreach (var name in names.WhereNotNull())
            Console.WriteLine($"  {name}");

        // ---- Aggregate 누적 연산 ----
        decimal total = products.Aggregate(0m, (sum, p) => sum + p.Price);
        Console.WriteLine($"\n[Aggregate] 전체 가격 합계: {total:C0}");

        // ---- Zip 두 시퀀스 병합 ----
        var prices = new[] { "저가", "중가", "고가" };
        var zipped = products.Zip(prices, (p, label) => $"{p.Name} = {label}");
        Console.WriteLine("\n[Zip]");
        foreach (var line in zipped)
            Console.WriteLine($"  {line}");

        // ---- ToLookup: 키 기반 룩업 ----
        var byCategory = products.ToLookup(p => p.Category);
        Console.WriteLine("\n[ToLookup] 카테고리별 상품");
        foreach (var group in byCategory)
            Console.WriteLine($"  {group.Key}: {string.Join(", ", group.Select(p => p.Name))}");

        // ---- ToDictionary ----
        var byId = products.ToDictionary(p => p.Id);
        Console.WriteLine($"\n[ToDictionary] Id=3 -> {byId[3].Name}");
    }
}

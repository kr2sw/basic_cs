namespace BasicCS.Chapter23;

/*
 * 실제 EF Core 사용 예시 (NuGet 필요):
 *
 *   var options = new DbContextOptionsBuilder<AppDbContext>()
 *       .UseSqlite("Data Source=app.db")   // UseSqlServer(...) 등으로 DB 변경
 *       .Options;
 *   using var db = new AppDbContext(options);
 *
 *   db.Products.Add(new Product { Name = "노트북", Price = 1200000m });
 *   await db.SaveChangesAsync();           // INSERT 실행
 *
 *   var list = await db.Products
 *       .Where(p => p.Price > 100000m)
 *       .OrderBy(p => p.Name)
 *       .ToListAsync();                    // SELECT 실행 (SQL 변환)
 *
 * public class AppDbContext : DbContext
 * {
 *     public AppDbContext(DbContextOptions options) : base(options) { }
 *     public DbSet<Product> Products => Set<Product>();
 * }
 */

// ---- 엔티티 (테이블에 대응) ----
public record Product(int Id, string Name, decimal Price, string Category);

// ---- 리포지토리 추상화 ----
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task DeleteAsync(int id);
}

// ---- 인메모리 리포지토리 (실제 DB 대신) ----
public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();
    private int _nextId = 1;

    public InMemoryProductRepository()
    {
        AddAsync(new Product(0, "노트북", 1200000m, "전자기기")).Wait();
        AddAsync(new Product(0, "마우스", 25000m, "전자기기")).Wait();
        AddAsync(new Product(0, "책상", 180000m, "가구")).Wait();
    }

    public Task<IEnumerable<Product>> GetAllAsync() => Task.FromResult(_products.AsEnumerable());

    public Task<Product?> GetByIdAsync(int id) => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task AddAsync(Product product)
    {
        // EF Core에서는 Add + SaveChangesAsync가 하는 일을 시뮬레이션
        _products.Add(product with { Id = _nextId++ });
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        _products.RemoveAll(p => p.Id == id);
        return Task.CompletedTask;
    }
}

// ---- 서비스 계층 ----
public class StoreService
{
    private readonly IProductRepository _repo;

    public StoreService(IProductRepository repo) => _repo = repo;

    public async Task ShowProductsAsync()
    {
        var products = await _repo.GetAllAsync();
        foreach (var p in products)
            Console.WriteLine($"  #{p.Id} {p.Name} — {p.Price:C0} [{p.Category}]");
    }

    public async Task AddAsync(string name, decimal price, string category)
    {
        await _repo.AddAsync(new Product(0, name, price, category));
        Console.WriteLine($"추가 완료: {name}");
    }
}

static class Program
{
    static async Task Main()
    {
        var repo = new InMemoryProductRepository();
        var store = new StoreService(repo);

        Console.WriteLine("== 상품 목록 (시드 데이터) ==");
        await store.ShowProductsAsync();

        Console.WriteLine("\n== 상품 추가 ==");
        await store.AddAsync("의자", 90000m, "가구");
        await store.ShowProductsAsync();

        Console.WriteLine("\n== 단건 조회 ==");
        var found = await repo.GetByIdAsync(2);
        Console.WriteLine($"  Id=2 -> {found?.Name ?? "없음"}");

        Console.WriteLine("\n== 삭제 ==");
        await repo.DeleteAsync(1);
        await store.ShowProductsAsync();
    }
}

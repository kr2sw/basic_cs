namespace BasicCS.Chapter29;

/*
 * 실제 ASP.NET Core Minimal API 예제 (NuGet 필요):
 *
 * var builder = WebApplication.CreateBuilder(args);
 * var app = builder.Build();
 *
 * // 라우팅: 경로 템플릿 + HTTP 메서드
 * app.MapGet("/", () => "Hello World");
 * app.MapGet("/hello/{name}", (string name) => $"안녕, {name}!");
 * app.MapPost("/users", (User user) => Results.Created($"/users/{user.Id}", user));
 * app.MapPut("/users/{id}", (int id, User user) => Results.NoContent());
 * app.MapDelete("/users/{id}", (int id) => Results.NoContent());
 *
 * // 엔드포인트 필터: 요청 전/후 공통 로직
 * app.MapGet("/admin", (HttpContext ctx) => "관리자 전용")
 *    .AddEndpointFilter(async (ctx, next) =>
 *    {
 *        Console.WriteLine("-> 인증 필터 실행");
 *        var result = await next(ctx);
 *        Console.WriteLine("<- 필터 종료");
 *        return result;
 *    });
 *
 * app.Run();
 */

// ---- 라우팅 엔트리 모델 ----
public record Route(string Method, string Pattern, Func<HttpRequestContext, object> Handler);

// ---- 요청/응답 문맥 시뮬레이션 ----
public record HttpRequestContext(string Method, string Path, Dictionary<string, string> RouteValues);

// ---- 미니멀 API 시뮬레이터 ----
public class WebApp
{
    private readonly List<Route> _routes = new();
    private readonly List<Func<Func<HttpRequestContext, object>, Func<HttpRequestContext, object>>> _filters = new();

    public void MapGet(string pattern, Func<HttpRequestContext, object> handler)
        => _routes.Add(new Route("GET", pattern, handler));

    public void AddEndpointFilter(Func<Func<HttpRequestContext, object>, Func<HttpRequestContext, object>> filter)
        => _filters.Add(filter);

    public void Run()
    {
        // 경로 파라미터 패턴 매칭 (예: /hello/{name})
        foreach (var r in _routes)
        {
            Console.WriteLine($"  {r.Method} {r.Pattern}");
        }

        Console.WriteLine("\n=== 요청 1: GET /hello/홍길동 ===");
        Dispatch(new HttpRequestContext("GET", "/hello/홍길동", new()));

        Console.WriteLine("\n=== 요청 2: GET /users/42 ===");
        Dispatch(new HttpRequestContext("GET", "/users/42", new()));
    }

    private void Dispatch(HttpRequestContext request)
    {
        var route = _routes.FirstOrDefault(r => Match(r.Pattern, request.Path, out _));
        if (route is null)
        {
            Console.WriteLine("  404 Not Found");
            return;
        }

        // 필터 체인을 핸들러 주변에 감싼다 (미들웨어 파이프라인)
        Func<HttpRequestContext, object> pipeline = route.Handler;
        foreach (var filter in _filters.AsEnumerable().Reverse())
            pipeline = filter(pipeline);

        var result = pipeline(request);
        Console.WriteLine($"  200 OK -> {result}");
    }

    // 간단한 템플릿 매칭: {name} 같은 부분을 추출
    private static bool Match(string pattern, string path, out Dictionary<string, string> values)
    {
        values = new();
        var pSegments = pattern.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var pathSegments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (pSegments.Length != pathSegments.Length) return false;

        for (int i = 0; i < pSegments.Length; i++)
        {
            if (pSegments[i].StartsWith('{') && pSegments[i].EndsWith('}'))
            {
                var key = pSegments[i].Trim('{', '}');
                values[key] = pathSegments[i];
            }
            else if (pSegments[i] != pathSegments[i])
            {
                return false;
            }
        }
        return true;
    }
}

static class Program
{
    static void Main()
    {
        var app = new WebApp();

        app.MapGet("/", _ => "Hello World");
        app.MapGet("/hello/{name}", ctx => $"안녕, {ctx.RouteValues["name"]}!");
        app.MapGet("/users/{id}", ctx => $"사용자 정보 조회: Id={ctx.RouteValues["id"]}");

        // 필터 추가: 모든 요청에 로깅 + 인증 시뮬레이션
        app.AddEndpointFilter(next => ctx =>
        {
            Console.WriteLine($"  [필터] 요청 수신: {ctx.Method} {ctx.Path}");
            if (ctx.Path.StartsWith("/admin"))
            {
                Console.WriteLine("  [필터] 403 관리자 권한 필요");
                return "403 Forbidden";
            }
            var result = next(ctx);
            Console.WriteLine($"  [필터] 응답 반환");
            return result;
        });

        Console.WriteLine("[등록된 라우트]");
        app.Run();

        Console.WriteLine("\n[참고] 실제 Minimal API는 'dotnet new web' + NuGet으로 동작합니다.");
    }
}

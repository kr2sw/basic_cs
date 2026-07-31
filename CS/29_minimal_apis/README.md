# 29: ASP.NET Core Minimal API — Minimal API

ASP.NET Core의 최소한의 HTTP API 작성 방식인 **Minimal API**를 소개합니다.
라우팅, 필터, 서비스 주입 개념을 익힙니다. 실제 서버는 NuGet이 필요하므로
여기서는 라우팅·필터 파이프라인을 콘솔로 재현하고 실제 코드는 주석으로
보여줍니다.

## 라우팅 개념

경로 템플릿과 HTTP 메서드를 매핑합니다.

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/hello/{name}", (string name) => $"안녕, {name}!");
app.MapPost("/users", (User u) => Results.Created("/users/1", u));

app.Run();
```

## 필터 (Filter) 개념

Minimal API는 엔드포인트 필터를 지원합니다. 요청 처리 전/후에 공통 로직
(로깅, 인증, 밸리데이션)을 적용할 수 있습니다.

```csharp
app.MapGet("/admin", (HttpContext ctx) => "관리자")
   .AddEndpointFilter(async (ctx, next) =>
   {
       Console.WriteLine("인증 확인 중...");
       return await next(ctx);
   });
```

## 실행

```bash
dotnet run
```

## 핵심 요약

- Minimal API는 `WebApplication`으로 파일 하나에 API를 정의합니다.
- 경로 템플릿 `{id}`는 경로 파라미터로 매핑됩니다.
- 필터 체인은 요청 처리 전후에 실행되는 미들웨어 파이프라인입니다.

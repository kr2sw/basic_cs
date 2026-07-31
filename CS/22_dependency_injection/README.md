# 22: 의존성 주입 — Dependency Injection

의존성 주입(DI)은 객체가 필요로 하는 의존성을 내부에서 직접 만들지 않고
외부에서 주입받는 디자인 패턴입니다. 이 장에서는 의존성 주입의 개념을 익히고,
수동으로 간단한 DI 컨테이너를 구현해 봅니다.

## 의존성 문제

아래처럼 클래스 내부에서 직접 생성하면 테스트가 어렵고 교체도 불가능합니다.

```csharp
class EmailNotifier
{
    private readonly SmtpClient _client = new(); // 직접 생성
}
```

인터페이스로 추상화하고 생성자를 통해 주입받으면 목(mock)으로 대체할 수 있습니다.

## 수동 DI 컨테이너

생성자 주입을 자동으로 처리하려면 리플렉션으로 생성자를 찾아 의존성을
재귀적으로 해석하면 됩니다. 실행 코드에는 다음과 같은 구조를 구현합니다.

- `IServiceCollection` — 인터페이스와 구현체의 매핑 저장
- `ServiceProvider` — `GetService<T>()`로 요청 시 그래프를 만들어 반환
- 싱글턴 / 일시적(transient) 생명주기 옵션

```csharp
var services = new ServiceCollection();
services.AddSingleton<IEmailSender, SmtpEmailSender>();
services.AddTransient<OrderService>();

var orderService = services.BuildProvider().GetService<OrderService>();
```

## 생명주기 (Lifetime)

- **Transient** — 요청마다 새 인스턴스
- **Singleton** — 전체 앱에서 단일 인스턴스
- **Scoped** — 범위(요청) 내에서 단일 인스턴스 (ASP.NET Core 기본 개념)

## 실행

```bash
dotnet run
```

## 핵심 요약

- DI는 의존성을 생성자로 주입받아 결합도를 낮추고 테스트를 쉽게 합니다.
- DI 컨테이너는 리플렉션으로 생성자 그래프를 자동 해석합니다.
- ASP.NET Core와 Console 앱(Host)은 모두 같은 패턴을 사용합니다.

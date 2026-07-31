# 35: 의존성 주입 — DI 컨테이너, MVVM 결합

## 학습 목표
- 의존성 주입(DI)과 수명(Singleton/Transient) 개념
- 생성자 주입과 인터페이스 분리
- 순수 BCL로 미니 DI 컨테이너 구현해 원리 이해
- `Microsoft.Extensions.Hosting` 연관 소개

## 왜 DI인가

`MainViewModel`이 `IGreeter`(인터페이스)에 의존하면:
- 구현을 갈아끼우기 쉬움 (테스트용 가짜 주입 가능)
- `Window`/`MessageBox` 같은 구체 타입 의존 제거
- 객체 생성과 수명 관리를 컨테이너에 위임

```csharp
// 안티패턴: 구체 타입 직접 생성
var greeter = new Greeter();

// DI: 인터페이스만 알고, 누가 주입할지 신경 쓰지 않는다
public MainViewModel(IGreeter greeter) => _greeter = greeter;
```

## 미니 컨테이너: 등록

```csharp
var services = new ServiceCollection();
services.AddSingleton<IClock, SystemClock>();
services.AddSingleton<IGreeter, Greeter>();
services.AddTransient<MainViewModel>();

var provider = services.BuildServiceProvider();
DataContext = provider.GetService<MainViewModel>();
```

```vb
Dim services As New ServiceCollection()
services.AddSingleton(Of IClock, SystemClock)()
services.AddTransient(Of MainViewModel)()
```

- **Singleton**: 한 번 생성 후 재사용 (공유 상태)
- **Transient**: 요청할 때마다 새 인스턴스

## 미니 컨테이너: 해석 (생성자 주입)

```csharp
private object Resolve(Type type)
{
    if (!_map.TryGetValue(type, out var descriptor))
        throw new InvalidOperationException($"등록되지 않은 서비스: {type.Name}");

    if (descriptor.Instance is not null)
        return descriptor.Instance;              // Singleton 재사용

    object instance = CreateInstance(descriptor.ImplementationType);

    if (descriptor.Lifetime == ServiceLifetime.Singleton)
        descriptor.Instance = instance;

    return instance;
}

private object CreateInstance(Type type)
{
    var ctor = type.GetConstructors()
        .OrderByDescending(c => c.GetParameters().Length)
        .First();
    var args = ctor.GetParameters()
        .Select(p => Resolve(p.ParameterType))    // 의존성 재귀 해석
        .ToArray();
    return ctor.Invoke(args);
}
```

`Greeter(IClock)`을 만들기 위해 `IClock` → `SystemClock`을 다시 해석하는
**중첩 생성자 주입**이 가능합니다.

## 실무: Microsoft.Extensions.DependencyInjection

이 챕터는 원리 학습용이며, 실무에서는 NuGet 패키지
`Microsoft.Extensions.DependencyInjection`(또는 `Host`)을 사용합니다.
`Host.CreateApplicationBuilder`로 시작하면 앱 수명 주기, 설정, 로깅까지
한 번에 구성됩니다.

```csharp
// 참고: 실무 패턴 (NuGet 필요)
var builder = Host.CreateApplicationBuilder();
builder.Services.AddSingleton<IGreeter, Greeter>();
builder.Services.AddSingleton<MainWindow>();
builder.Services.AddSingleton<MainViewModel>();
```

## 실행

```bash
cd csharp
dotnet run
```

```bash
cd vbnet
dotnet run
```

## 정리

- 인터페이스 의존 + 생성자 주입으로 테스트 가능한 코드
- 컨테이너가 생성·수명·조립을 담당
- 실무는 `Microsoft.Extensions.DependencyInjection` 활용
  (이 예제는 NuGet 없이 원리를 구현한 것)

# 34: 백그라운드 서비스 — Background Services

`IHostedService`는 앱이 실행되는 동안 백그라운드에서 도는 서비스를 만드는
방법입니다. 메시지 큐(채널)를 이용한 생산자-소비자 패턴을 학습합니다.

## IHostedService 개념

ASP.NET Core/Generic Host는 앱 시작 시 `IHostedService` 구현을 시작하고
종료 시 멈춥니다. `BackgroundService`는 그 편의 구현체입니다.

```csharp
public class Worker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            // 주기적인 작업
            await Task.Delay(1000, ct);
        }
    }
}
```

## 채널 큐 (Channel)

`Channel<T>`는 스레드 안전한 생산자-소비자 큐입니다.

- `Channel.CreateUnbounded<T>()` — 무제한 버퍼
- `WriteAsync` — 생산자
- `ReadAllAsync` — 소비자 스트림

## 실행

```bash
dotnet run
```

## 핵심 요약

- `BackgroundService` + `IHostedService`로 앱 수명 주기와 함께하는 작업 실행.
- `Channel<T>`로 생산자-소비자를 안전하게 연결합니다.
- `CancellationToken`으로 앱 종료 시 작업을 정리합니다.

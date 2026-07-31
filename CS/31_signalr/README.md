# 31: SignalR — SignalR

SignalR은 ASP.NET Core에서 **실시간 웹 통신**을 위한 라이브러리입니다.
웹소켓을 기본으로, 사용할 수 없으면 폴링으로 자동 전환(fallback)합니다.

## 허브 (Hub) 개념

서버와 클라이언트가 서로 메서드를 호출하는 중개자입니다.

```csharp
public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
        => await Clients.All.SendAsync("ReceiveMessage", user, message);
}
```

- `Clients.All` — 모든 연결에 브로드캐스트
- `Clients.Group(name)` — 그룹 단위 전송
- `Clients.Client(id)` — 특정 연결에 전송

## 실행 흐름

1. 클라이언트가 허브에 연결 (`new HubConnectionBuilder()...`)
2. `InvokeAsync("메서드명")`로 서버 호출
3. 서버가 `SendAsync("이벤트명", ...)`로 응답 브로드캐스트

이 장에서는 허브의 연결 관리·그룹·브로드캐스트 동작을 콘솔로 재현합니다.

## 실행

```bash
dotnet run
```

## 핵심 요약

- 허브는 실시간 양방향 통신의 중개자입니다.
- 그룹으로 관심 있는 클라이언트만 골라 보낼 수 있습니다.
- 실제 사용은 NuGet(Microsoft.AspNetCore.SignalR)이 필요합니다.

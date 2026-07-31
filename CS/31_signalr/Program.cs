namespace BasicCS.Chapter31;

/*
 * 실제 SignalR 예제 (NuGet: Microsoft.AspNetCore.SignalR):
 *
 * // 서버: 허브 정의
 * public class ChatHub : Hub
 * {
 *     public async Task SendMessage(string user, string message)
 *         => await Clients.All.SendAsync("ReceiveMessage", user, message);
 *
 *     public async Task JoinRoom(string room)
 *         => await Groups.AddToGroupAsync(Context.ConnectionId, room);
 * }
 *
 * // Startup에 등록
 * // builder.Services.AddSignalR();
 * // app.MapHub<ChatHub>("/chat");
 *
 * // 클라이언트 (JS/C# 공통)
 * var connection = new HubConnectionBuilder()
 *     .WithUrl("http://localhost:5000/chat")
 *     .Build();
 * connection.On<string, string>("ReceiveMessage", (user, msg) =>
 *     Console.WriteLine($"{user}: {msg}"));
 * await connection.StartAsync();
 * await connection.InvokeAsync("SendMessage", "홍길동", "안녕!");
 */

// ---- 클라이언트 연결 모델 ----
public class ClientConnection
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public List<string> Groups { get; } = new();
    public override string ToString() => $"{Name}({Id})";
}

// ---- 허브 시뮬레이터 ----
public class ChatHubSimulator
{
    private readonly List<ClientConnection> _clients = new();
    private int _seq;

    public ClientConnection Connect(string name)
    {
        var client = new ClientConnection { Id = $"conn-{++_seq}", Name = name };
        _clients.Add(client);
        Console.WriteLine($"  [연결] {client} 접속 (총 {_clients.Count}명)");
        return client;
    }

    public void Disconnect(ClientConnection client)
    {
        _clients.Remove(client);
        Broadcast("System", $"{client.Name}님이 퇴장했습니다.");
    }

    public void JoinGroup(ClientConnection client, string group)
    {
        client.Groups.Add(group);
        SendToGroup(group, "System", $"{client.Name}님이 {group} 방에 참여했습니다.");
    }

    // Clients.All — 전체 브로드캐스트
    public void Broadcast(string user, string message)
    {
        foreach (var c in _clients)
            Console.WriteLine($"  -> {c.Name} 수신: [{user}] {message}");
    }

    // Clients.Client(id) — 특정 연결에만 전송
    public void SendToClient(ClientConnection target, string user, string message)
        => Console.WriteLine($"  -> {target.Name} 수신(개인): [{user}] {message}");

    // Clients.Group(name) — 그룹에만 전송
    public void SendToGroup(string group, string user, string message)
    {
        foreach (var c in _clients.Where(c => c.Groups.Contains(group)))
            Console.WriteLine($"  -> {c.Name} 수신({group}방): [{user}] {message}");
    }
}

static class Program
{
    static void Main()
    {
        var hub = new ChatHubSimulator();

        Console.WriteLine("[허브 시작: /chat]");
        var alice = hub.Connect("앨리스");
        var bob = hub.Connect("밥");
        var carol = hub.Connect("캐롤");

        Console.WriteLine("\n== 1) 브로드캐스트: 모든 클라이언트 ==");
        hub.Broadcast("앨리스", "모두 안녕!");

        Console.WriteLine("\n== 2) 특정 클라이언트 전송 ==");
        hub.SendToClient(bob, "앨리스", "밥만 봐");

        Console.WriteLine("\n== 3) 그룹: 게임 방 참여 ==");
        hub.JoinGroup(alice, "game");
        hub.JoinGroup(bob, "game");
        hub.SendToGroup("game", "System", "게임 시작!");

        Console.WriteLine("\n== 4) 캐롤은 게임 그룹이 아니라서 수신 안 함 ==");
        hub.SendToGroup("game", "앨리스", "한 턴 더!");

        Console.WriteLine("\n== 5) 연결 종료 ==");
        hub.Disconnect(carol);

        Console.WriteLine("\n[요약] Clients.All / Clients.Client / Clients.Group");
    }
}

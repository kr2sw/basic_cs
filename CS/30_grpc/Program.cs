namespace BasicCS.Chapter30;

/*
 * 실제 gRPC 예제 (NuGet: Grpc.AspNetCore, Google.Protobuf):
 *
 * // greet.proto
 * // syntax = "proto3";
 * // package Greet;
 * // service Greeter {
 * //   rpc SayHello (HelloRequest) returns (HelloReply);
 * //   rpc StreamNumbers (HelloRequest) returns (stream HelloReply);
 * // }
 * // message HelloRequest { string name = 1; }
 * // message HelloReply   { string message = 1; }
 *
 * // 서버 (Startup / Program.cs)
 * public class GreeterService : Greeter.GreeterBase
 * {
 *     public override Task<HelloReply> SayHello(HelloRequest request,
 *                                               ServerCallContext context)
 *         => Task.FromResult(new HelloReply { Message = $"Hello {request.Name}" });
 * }
 *
 * // 클라이언트
 * using var channel = GrpcChannel.ForAddress("https://localhost:5001");
 * var client = new Greeter.GreeterClient(channel);
 * var reply = await client.SayHelloAsync(new HelloRequest { Name = "World" });
 */

// ---- protobuf 필드 번호 개념을 재현하는 계약 타입 ----
public record Message(int FieldNumber, string FieldName, object? Value);

// ---- proto 파일 텍스트 (실행 시 보여줌) ----
public static class ProtoDefinitions
{
    public const string GreeterProto = """
        syntax = "proto3";

        package Greet;

        service Greeter {
          rpc SayHello (HelloRequest) returns (HelloReply);
          rpc StreamNumbers (HelloRequest) returns (stream HelloReply);
        }

        message HelloRequest {
          string name = 1;   // 필드 번호 1: 이름
        }

        message HelloReply {
          string message = 1;
        }
        """;
}

// ---- gRPC 시뮬레이터: 계약을 기반으로 메시지를 직렬화 ----
public static class ProtobufSimulator
{
    // 필드 번호 + wire type(2=length-delimited)을 간단히 흉내낸 인코딩
    public static byte[] EncodeString(int fieldNumber, string value)
    {
        var payload = System.Text.Encoding.UTF8.GetBytes(value);
        var header = new byte[] { (byte)(fieldNumber << 3 | 2), (byte)payload.Length };
        return header.Concat(payload).ToArray();
    }

    public static string DecodeString(byte[] data, out int fieldNumber)
    {
        fieldNumber = data[0] >> 3;
        int length = data[1];
        return System.Text.Encoding.UTF8.GetString(data, 2, length);
    }
}

static class Program
{
    static void Main()
    {
        Console.WriteLine("[proto 파일 정의]");
        Console.WriteLine(ProtoDefinitions.GreeterProto);

        Console.WriteLine("== 유니캐리 RPC (SayHello) ==");
        var request = new Message(1, "name", "홍길동");

        // 메시지 인코딩 -> 전송 -> 디코딩 시뮬레이션
        byte[] wire = ProtobufSimulator.EncodeString(request.FieldNumber, (string)request.Value!);
        Console.WriteLine($"  직렬화된 바이트 ({wire.Length} bytes): {BitConverter.ToString(wire)}");

        string decoded = ProtobufSimulator.DecodeString(wire, out int field);
        Console.WriteLine($"  역직렬화: field={field}, name='{decoded}'");

        string reply = $"Hello, {decoded}!"; // 서버 응답 시뮬레이션
        Console.WriteLine($"  서버 응답: {reply}");

        Console.WriteLine("\n== 서버 스트리밍 RPC (StreamNumbers) ==");
        var replies = new[] { "수신 1", "수신 2", "수신 3" };
        foreach (var r in replies)
        {
            Console.WriteLine($"  (스트림) {r}");
            Thread.Sleep(50); // 스트리밍 지연 시뮬레이션
        }
        Console.WriteLine("  스트림 종료");

        Console.WriteLine("\n== 스트리밍 유형 정리 ==");
        Console.WriteLine("  - Unary            : 요청 1개 -> 응답 1개");
        Console.WriteLine("  - Server streaming : 요청 1개 -> 응답 스트림");
        Console.WriteLine("  - Client streaming : 요청 스트림 -> 응답 1개");
        Console.WriteLine("  - Bidirectional    : 요청/응답 동시 스트림");
    }
}

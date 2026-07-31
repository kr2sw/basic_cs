# 30: gRPC — gRPC

gRPC는 Google이 만든 고성능 RPC 프레임워크입니다. HTTP/2 기반으로 바이너리
프로토콜(Protobuf)을 사용해 REST보다 가볍고 빠른 서비스 간 통신을 제공합니다.

## 프로토콜 버퍼 (Protobuf)

`.proto` 파일로 **메시지 스키마**와 **서비스 계약**을 정의하고, 빌드 시점에
각 언어의 코드를 생성합니다.

```protobuf
syntax = "proto3";

service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
}

message HelloRequest {
  string name = 1;
}
```

## 스트리밍

- **Unary** — 요청 1, 응답 1
- **Server streaming** — 요청 1, 응답 스트림
- **Client streaming** — 요청 스트림, 응답 1
- **Bidirectional streaming** — 양방향 스트림

## HTTP/2 특성

- 바이너리 프레임 → 헤더 압축, 멀티플렉싱
- 단일 연결로 여러 요청 처리
- TLS 기본 사용

## 실행

```bash
dotnet run
```

## 핵심 요약

- gRPC는 proto 파일로 계약을 정의하고 코드를 생성합니다.
- Protobuf는 JSON보다 작고 빠른 바이너리 직렬화입니다.
- 스트리밍 RPC로 실시간 데이터 전송이 가능합니다.

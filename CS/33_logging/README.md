# 33: 로깅/관찰 — Logging & Observability

로깅은 애플리케이션의 동작을 관찰하는 첫걸음입니다. 구조적 로깅(Serilog)의
개념을 익히고, 커스텀 로거를 직접 만들어 보겠습니다.

## 로그 레벨

`Trace < Debug < Information < Warning < Error < Critical` 순으로 상세도가
감소합니다. 환경에 따라 표시할 최소 레벨을 조절합니다.

## Serilog 개념

Serilog는 **구조적 로깅(structured logging)** 을 지원합니다. 메시지 템플릿에
자리 표시자를 넣고, 값들을 JSON 등으로 구조화해 저장할 수 있습니다.

```csharp
Log.Information("주문 {OrderId} 완료, 금액 {Amount:C0}", orderId, amount);
```

파일·콘솔·DB 등 여러 싱크(sink)로 출력할 수 있습니다.

## 커스텀 로거 설계

이 장에서는 `ILogger` 인터페이스, 레벨 필터링, 구조적 메시지 포맷팅,
파일 출력까지 지원하는 미니 로거를 구현합니다.

## 실행

```bash
dotnet run
```

## 핵심 요약

- 로그 레벨 필터링으로 필요할 때만 상세 로그를 남깁니다.
- 구조적 로깅은 검색·집계가 쉬운 키-값 형태로 저장합니다.
- Serilog는 싱크를 추가해 파일/콘솔/원격으로 유연하게 보냅니다.

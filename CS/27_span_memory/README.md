# 27: Span과 Memory — Span & Memory

`Span<T>`는 힙 할당 없이 연속 메모리를 다루는 **ref struct**입니다.
고성능 처리(문자열 파싱, 직렬화, 네트워크 버퍼)의 핵심입니다.

## Span<T> 특징

- **ref struct** — 힙에 올릴 수 없어 boxing·GC 부담 없음
- 스택, 배열, 문자열, 비관리 메모리를 통일된 방식으로 보는 뷰
- `Slice(start, length)`로 추가 할당 없이 부분 범위를 자름

```csharp
ReadOnlySpan<char> span = text.AsSpan();
var slice = span.Slice(2, 4);   // 할당 없음
```

## Memory<T>

`Span`은 `async` 메서드에서 사용할 수 없습니다(await 경계를 지날 수 없음).
`Memory<T>`는 힙에 살 수 있어서 비동기 작업에도 안전하게 넘길 수 있습니다.

```csharp
async Task HandleAsync(Memory<byte> buffer) { ... }
```

## 저할당 코드 예제

- 문자열 분리(`Split`의 Span 버전)
- 숫자 파싱
- `TryParse` 계열

## 실행

```bash
dotnet run
```

## 핵심 요약

- `Span<T>`는 할당 없는 메모리 접근을 가능하게 합니다.
- `async` 메서드에는 `Memory<T>`를 사용합니다.
- 문자열·버퍼 처리에서 GC 압력을 크게 줄일 수 있습니다.

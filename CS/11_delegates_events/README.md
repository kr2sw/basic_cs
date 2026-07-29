# 11 델리게이트(Delegate)와 이벤트(Event)

C#의 델리게이트와 이벤트를 사용한 콜백 및 발행-구독 패턴을 학습합니다.

## 주요 개념

- 델리게이트 선언과 사용 (메서드 참조)
- 멀티캐스트 델리게이트 (`+=` / `-=`)
- 내장 델리게이트: `Func<>`, `Action<>`, `Predicate<>`
- 이벤트 (`event`) — `EventHandler` 패턴
- 무명 메서드 (anonymous method)
- 클로저 (Closure) — 람다에서 외부 변수 캡처

## 예제 코드

```csharp
delegate int Operation(int x, int y);
Operation add = (a, b) => a + b;

Notify notifier = LogToConsole;
notifier += LogToFile;
notifier("Multicast delegate example");

public event EventHandler? Clicked;
button.Clicked += OnButtonClick;
button.SimulateClick();
```

## 실행 방법

```bash
dotnet run --project ../11_delegates_events
```

## 핵심 요약

- 델리게이트는 메서드를 참조하는 타입 안전한 함수 포인터입니다.
- 이벤트는 델리게이트 기반의 발행-구독 패턴입니다.
- `Func<>` / `Action<>` / `Predicate<>`는 대부분의 상황에서 직접 델리게이트 선언을 대체합니다.

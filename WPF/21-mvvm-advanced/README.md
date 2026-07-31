# 21: MVVM 심화 — Messenger, Mediator 패턴

## 학습 목표
- ViewModel 간 결합도를 낮추는 **Mediator(중재자)** 패턴 이해
- `WeakReference` 기반 **Messenger** 구현과 동작 원리
- 메시지 등록/전송/구독 해제의 수명주기 관리
- 이벤트 대신 약한 참조로 메모리 누수 방지하기

## Mediator 패턴이 필요한 이유

MVVM에서 두 ViewModel이 서로 통신해야 할 때가 많습니다.
예) 로그인 ViewModel이 성공하면 주문 ViewModel이 초기화되어야 함.

```csharp
// 나쁜 예: 서로를 직접 참조 → 결합도 증가
orderViewModel.UserName = loginViewModel.UserName;
```

```csharp
// 좋은 예: 중재자를 통한 비동기 통신
Messenger.Instance.Send(new LoginSucceededMessage { UserName = name });
```

직접 참조 대신 메신저가 중간에서 전달하면 각 ViewModel은
"보낼 메시지"와 "받을 메시지"만 알면 됩니다.

## Messenger 설계

핵심은 수신자를 **약한 참조(WeakReference)**로 보관하는 것입니다.

```csharp
public void Register(object recipient, Action<TextMessage> action)
{
    // 수신자를 약한 참조로 저장
    _registrations.Add(new Registration(new WeakReference(recipient), action.Method));
}
```

- 등록된 수신자가 더 이상 사용되지 않으면 GC되어 `Target == null`이 됩니다.
- 메시지 전송 시 죽은 등록을 발견하면 정리(remove)합니다.
- 강한 참조로 보관하면 ViewModel이 폐기되어도 메신저가 붙잡아 **메모리 누수**가 생깁니다.

## 메시지 전송

```csharp
// 보내는 쪽
Messenger.Instance.Send(new TextMessage { Text = MessageText });

// 받는 쪽 (생성자에서 등록)
Messenger.Instance.Register(this, OnTextMessage);

private void OnTextMessage(TextMessage m)
{
    Received = m.Text;
}
```

VB.NET에서도 동일한 구조를 사용합니다.

```vb
Messenger.Instance.Register(Me, AddressOf OnTextMessage)
Messenger.Instance.Send(New TextMessage() With {.Text = MessageText})
```

> 주의: `Register`에는 **메서드 그룹**(`AddressOf`)을 넘겨야 합니다.
> 람다를 넘기면 `action.Method`가 클로저 객체에 속해 약한 참조 대상과 어긋납니다.

## XAML로 두 ViewModel 배치

```xml
<TextBox Text="{Binding Sender.MessageText}"/>
<Button Content="메시지 보내기" Command="{Binding Sender.SendCommand}"/>
<TextBlock Text="{Binding Receiver.Received}"/>
```

메인 창의 `DataContext`를 자기 자신으로 두고 두 ViewModel을 속성으로 노출합니다.

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

| 구성 요소 | 역할 |
|-----------|------|
| `Message` | 전달할 데이터 (불변 권장) |
| `IMessenger` | 등록/전송/해지 인터페이스 |
| `Messenger` | 약한 참조 목록을 가진 싱글턴 구현체 |
| `SenderViewModel` | 메시지 생산자 |
| `ReceiverViewModel` | 메시지 소비자 |

실무에서는 커뮤니티 패키지(예: `Microsoft.Toolkit.Mvvm`의 `WeakReferenceMessenger`)
를 사용하는 것이 일반적이지만, 내부 원리를 이해하면 요구 사항에 맞는
커스텀 메신저를 직접 만들 수 있습니다.

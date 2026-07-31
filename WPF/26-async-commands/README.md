# 26: 비동기 커맨드 — AsyncRelayCommand, 취소

## 학습 목표
- `Func<Task>`를 실행하는 `AsyncRelayCommand` 구현
- 실행 중 재진입 방지(`CanExecute` 제어)
- `CancellationTokenSource`로 협조적 취소(cooperative cancellation)
- UI 스레드 안전한 상태 갱신

## 문제: 동기 커맨드로 비동기 처리

```csharp
// 잘못된 예: ICommand.Execute는 void라 비동기 결과를 기다리지 않습니다.
ExecuteCommand = new RelayCommand(_ =>
{
    Task.Delay(3000);          // 시작 후 즉시 반환 → UI가 반응 없음
    Result = "완료";           // 실제로는 바로 실행됨
});
```

`async void`/`Func<Task>` 기반 커맨드가 필요합니다.

## AsyncRelayCommand

```csharp
public class AsyncRelayCommand : ICommand
{
    private readonly Func<object?, Task> _execute;
    private bool _isRunning;

    public bool IsRunning
    {
        get => _isRunning;
        private set { _isRunning = value; CanExecuteChanged?.Invoke(this, EventArgs.Empty); }
    }

    public bool CanExecute(object? parameter)
        => !IsRunning;  // 실행 중에는 버튼 비활성화

    public async void Execute(object? parameter)
    {
        IsRunning = true;
        try
        {
            await _execute(parameter);
        }
        finally
        {
            IsRunning = false;
        }
    }
}
```

## 취소 지원

`CancellationTokenSource`를 저장해 두고 취소 버튼이 이를 요청합니다.

```csharp
private CancellationTokenSource? _cts;

private async Task RunAsync(object? parameter)
{
    _cts = new CancellationTokenSource();
    var token = _cts.Token;
    for (int i = 0; i <= 100; i += 5)
    {
        token.ThrowIfCancellationRequested();  // 취소 요청 시 예외 발생
        await Task.Delay(120, token);
        Progress = i;
    }
}

// 취소 버튼 커맨드
CancelCommand = new RelayCommand(_ => _cts?.Cancel(), _ => IsRunning);
```

```csharp
try { ... }
catch (OperationCanceledException) { Status = "취소됨"; }
```

VB.NET에서도 동일한 구조입니다.

```vb
Private Async Function RunAsync(p As Object) As Task
    token.ThrowIfCancellationRequested()
    Await Task.Delay(120, token)
End Function
```

## UI 스레드 안전성

WPF UI 스레드에는 `SynchronizationContext`가 설치되어 있어
`await` 이후의 코드가 자동으로 UI 스레드에서 실행됩니다.

```csharp
// 어떤 스레드에서 시작해도 결과는 UI 스레드에서 이어집니다.
await Task.Delay(120, token);
Progress = i;   // 안전 (UI 스레드)
```

## XAML

```xml
<ProgressBar Minimum="0" Maximum="100" Value="{Binding Progress}"/>
<Button Content="작업 시작" Command="{Binding RunCommand}"/>
<Button Content="취소" Command="{Binding CancelCommand}"/>
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

- 실행 중 `CanExecute=false` → 재진입 방지 (AsyncRelayCommand 자체 상태 + VM 상태 동기화)
- 취소는 반드시 **협조적**: 작업 코드가 `CancellationToken`을 확인해야 함
- `async void`는 예외를 잡지 않으면 크래시하므로 `try/finally`로 보호
- 대안: `ICommand`를 커맨드 매니저와 결합하지 않는 한 수동으로 `CanExecuteChanged`를 올려야 함

# 06 - 커맨드 (Commands)

## 학습 목표
- ICommand 인터페이스 이해
- RelayCommand 구현
- CommandBinding과 CommandTarget 사용
- CanExecute로 버튼 활성/비활성 제어

## 커맨드 인터페이스

```csharp
public interface ICommand
{
    event EventHandler? CanExecuteChanged;
    bool CanExecute(object? parameter);
    void Execute(object? parameter);
}
```

RelayCommand는 ICommand를 구현하는 헬퍼 클래스로, WPF MVVM에서 널리 사용됩니다.

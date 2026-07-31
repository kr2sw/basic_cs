using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ch26;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}

// 실행 중에는 CanExecute=false가 되어 재진입을 막는 비동기 커맨드
public class AsyncRelayCommand : ICommand
{
    private readonly Func<object?, Task> _execute;
    private readonly Predicate<object?>? _canExecute;
    private bool _isRunning;

    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            if (_isRunning == value) return;
            _isRunning = value;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public AsyncRelayCommand(Func<object?, Task> execute, Predicate<object?>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
        => !IsRunning && (_canExecute?.Invoke(parameter) ?? true);

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

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
    public void Execute(object? parameter) => _execute(parameter);

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public class MainViewModel : INotifyPropertyChanged
{
    private CancellationTokenSource? _cts;
    private int _progress;
    private string _status = "대기 중";
    private bool _isRunning;

    public int Progress
    {
        get => _progress;
        set { _progress = value; OnPropertyChanged(); }
    }

    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            _isRunning = value;
            OnPropertyChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }
    }

    public ICommand RunCommand { get; }
    public RelayCommand CancelCommand { get; }

    public MainViewModel()
    {
        RunCommand = new AsyncRelayCommand(RunAsync);
        CancelCommand = new RelayCommand(_ => _cts?.Cancel(), _ => IsRunning);
    }

    private async Task RunAsync(object? parameter)
    {
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        IsRunning = true;
        Status = "작업 실행 중...";
        Progress = 0;

        try
        {
            for (int i = 0; i <= 100; i += 5)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(120, token);
                Progress = i;
            }
            Status = "완료!";
        }
        catch (OperationCanceledException)
        {
            Status = "취소됨";
        }
        finally
        {
            IsRunning = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

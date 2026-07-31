using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Input;

namespace Ch34;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}

public class MainViewModel : INotifyPropertyChanged
{
    private readonly AsyncRelayCommand _runCommand;
    private CancellationTokenSource? _cts;
    private int _sum;
    private int _processed;
    private string _status = "대기 중";

    public MainViewModel()
    {
        _runCommand = new AsyncRelayCommand(_ => RunPipelineAsync());
        CancelCommand = new RelayCommand(_ => _cts?.Cancel());
    }

    public AsyncRelayCommand RunCommand => _runCommand;
    public RelayCommand CancelCommand { get; }

    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public int Processed
    {
        get => _processed;
        set { _processed = value; OnPropertyChanged(); }
    }

    public int Sum
    {
        get => _sum;
        set { _sum = value; OnPropertyChanged(); }
    }

    private async Task RunPipelineAsync()
    {
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        _sum = 0;
        Sum = 0;
        Processed = 0;
        Status = "파이프라인 실행 중...";

        // 병렬 실행 블록과 수신 블록 옵션 (취소 토큰 포함)
        var pipelineOptions = new ExecutionDataflowBlockOptions
        {
            CancellationToken = token,
            MaxDegreeOfParallelism = 4
        };
        var actionOptions = new ExecutionDataflowBlockOptions { CancellationToken = token };

        // 결과를 UI 스레드로 안전하게 전달 (Progress<T>가 SynchronizationContext를 캡처)
        var sumProgress = new Progress<int>(s => Sum = s);
        var countProgress = new Progress<int>(c => Processed += c);

        // 파이프라인: 숫자 -> 제곱 -> 합산
        var transform = new TransformBlock<int, int>(async n =>
        {
            await Task.Delay(10, token);
            countProgress.Report(1);
            return n * n;
        }, pipelineOptions);

        var action = new ActionBlock<int>(n =>
        {
            var newSum = Interlocked.Add(ref _sum, n);
            sumProgress.Report(newSum);
        }, actionOptions);

        transform.LinkTo(action, new DataflowLinkOptions { PropagateCompletion = true });

        try
        {
            for (int i = 1; i <= 100; i++)
            {
                await transform.SendAsync(i, token);
            }
            transform.Complete();
            await action.Completion;   // 처리 완료까지 대기

            Status = $"완료 - 합: {Sum:N0}";
        }
        catch (OperationCanceledException)
        {
            Status = "취소됨";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

// 실행 중 재진입을 막는 비동기 커맨드 (26장 참고)
public class AsyncRelayCommand : ICommand
{
    private readonly Func<object?, Task> _execute;
    private bool _isRunning;

    public AsyncRelayCommand(Func<object?, Task> execute) => _execute = execute;

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => !_isRunning;

    public async void Execute(object? parameter)
    {
        _isRunning = true;
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        try
        {
            await _execute(parameter);
        }
        finally
        {
            _isRunning = false;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;

    public RelayCommand(Action<object?> execute) => _execute = execute;

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => _execute(parameter);
}

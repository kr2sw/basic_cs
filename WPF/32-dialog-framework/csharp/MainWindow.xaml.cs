using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Ch32;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}

// 뷰 모델은 Window 대신 인터페이스에만 의존한다 (테스트 가능)
public interface IDialogService
{
    bool? ShowDialog(string title, object content, Window owner);
}

public class DialogService : IDialogService
{
    public bool? ShowDialog(string title, object content, Window owner)
    {
        var dialog = new DialogWindow
        {
            Title = title,
            DataContext = content,
            Owner = owner
        };
        return dialog.ShowDialog();
    }
}

public class NameDialogViewModel : INotifyPropertyChanged
{
    private string _name = "";

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class MainViewModel : INotifyPropertyChanged
{
    private readonly IDialogService _dialogs = new DialogService();

    private string _lastResult = "아직 대화상자를 열지 않았습니다.";

    public string LastResult
    {
        get => _lastResult;
        set { _lastResult = value; OnPropertyChanged(); }
    }

    public RelayCommand ShowDialogCommand { get; }

    public MainViewModel()
    {
        ShowDialogCommand = new RelayCommand(_ =>
        {
            var vm = new NameDialogViewModel();
            var result = _dialogs.ShowDialog("이름 입력", vm, Application.Current.MainWindow);
            LastResult = result == true ? $"확인: {vm.Name}" : "취소됨";
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;

    public RelayCommand(Action<object?> execute) => _execute = execute;

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => _execute(parameter);
}

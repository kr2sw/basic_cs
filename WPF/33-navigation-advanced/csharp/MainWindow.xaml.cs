using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ch33;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var navigation = new FrameNavigationService(frame);
        var vm = new MainViewModel(navigation);
        DataContext = vm;
        vm.NavigateHome();
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        if (frame.CanGoBack) frame.GoBack();
    }

    private void Forward_Click(object sender, RoutedEventArgs e)
    {
        if (frame.CanGoForward) frame.GoForward();
    }
}

// VM은 Page/Frame을 몰라도 된다 - 인터페이스만 의존
public interface INavigationService
{
    void NavigateTo(object viewModel);
}

public class FrameNavigationService : INavigationService
{
    private readonly Frame _frame;

    public FrameNavigationService(Frame frame) => _frame = frame;

    public void NavigateTo(object viewModel)
    {
        // Page.Content에 VM을 넣으면 DataType DataTemplate이 뷰를 그린다
        var page = new Page { Content = viewModel };
        _frame.Navigate(page);
    }
}

public class MainViewModel
{
    private readonly INavigationService _navigation;

    public RelayCommand GoHomeCommand { get; }
    public RelayCommand GoSettingsCommand { get; }

    public MainViewModel(INavigationService navigation)
    {
        _navigation = navigation;
        GoHomeCommand = new RelayCommand(_ => NavigateHome());
        GoSettingsCommand = new RelayCommand(_ => _navigation.NavigateTo(new SettingsViewModel()));
    }

    public void NavigateHome() => _navigation.NavigateTo(new HomeViewModel());
}

public class HomeViewModel
{
    public string Greeting => DateTime.Now.Hour < 12 ? "좋은 아침입니다." : "반갑습니다.";

    public string Description =>
        "Frame은 저널(journal)을 유지하므로 뒤로/앞으로 버튼으로 "
        + "이전 페이지로 이동할 수 있습니다. 페이지 콘텐츠는 VM의 "
        + "DataType DataTemplate으로 렌더링되어 뷰와 뷰 모델이 분리됩니다.";
}

public class SettingsViewModel : INotifyPropertyChanged
{
    private string _name = "홍길동";
    private double _volume = 50;

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    public double Volume
    {
        get => _volume;
        set { _volume = value; OnPropertyChanged(); }
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

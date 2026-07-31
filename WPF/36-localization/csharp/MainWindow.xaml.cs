using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Ch36;

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
    private string _name = "홍길동";
    private string _greeting = "";
    private string _cultureSample = "";

    public MainViewModel()
    {
        GreetCommand = new RelayCommand(_ =>
        {
            var format = (string)Application.Current.Resources["GreetingFormat"];
            Greeting = string.Format(CultureInfo.CurrentUICulture, format, Name);
        });
        SetKoCommand = new RelayCommand(_ => SetLanguage("ko-KR"));
        SetEnCommand = new RelayCommand(_ => SetLanguage("en-US"));
        UpdateCultureSample();
    }

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    public string Greeting
    {
        get => _greeting;
        set { _greeting = value; OnPropertyChanged(); }
    }

    public string CultureSample
    {
        get => _cultureSample;
        set { _cultureSample = value; OnPropertyChanged(); }
    }

    public RelayCommand GreetCommand { get; }
    public RelayCommand SetKoCommand { get; }
    public RelayCommand SetEnCommand { get; }

    private void SetLanguage(string name)
    {
        Localization.SetCulture(name);
        UpdateCultureSample();
    }

    // 현재 문화권 형식으로 숫자/날짜를 다시 표시
    private void UpdateCultureSample()
    {
        var c = CultureInfo.CurrentUICulture;
        CultureSample = $"{1234567.89.ToString("N2", c)} · {DateTime.Now.ToString("d", c)}";
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

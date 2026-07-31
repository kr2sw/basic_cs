using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Ch39;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}

// ---------- 미니 테스트 러너 (NuGet 없이 실행 가능하도록 직접 구현) ----------

public class TestCase
{
    public string Name { get; init; } = "";
    public Action Test { get; init; } = () => { };
}

public class TestResult
{
    public string Name { get; }
    public bool Passed { get; }
    public string Message { get; }

    public TestResult(string name, bool passed, string message)
    {
        Name = name;
        Passed = passed;
        Message = message;
    }
}

public static class Assert
{
    public static void True(bool condition, string message = "")
    {
        if (!condition) throw new Exception(message);
    }

    public static void False(bool condition, string message = "")
    {
        if (condition) throw new Exception(message);
    }
}

public static class MiniTestRunner
{
    public static List<TestResult> Run(IEnumerable<TestCase> cases)
    {
        var results = new List<TestResult>();
        foreach (var test in cases)
        {
            try
            {
                test.Test();
                results.Add(new TestResult(test.Name, true, ""));
            }
            catch (Exception ex)
            {
                results.Add(new TestResult(test.Name, false, ex.Message));
            }
        }
        return results;
    }
}

// ---------- 테스트 대상 뷰 모델 ----------

public class LoginViewModel : INotifyPropertyChanged
{
    private string _name = "";
    private string _password = "";
    private string _status = "";

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    // 순수 로직: Window/Control에 의존하지 않아 테스트하기 쉽다
    public bool IsValid => !string.IsNullOrWhiteSpace(Name) && Password.Length >= 4;

    public RelayCommand LoginCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(_ =>
        {
            Status = IsValid ? $"환영합니다, {Name}님!" : "입력이 올바르지 않습니다.";
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

// ---------- 테스트 케이스 ----------

public static class LoginTests
{
    public static IEnumerable<TestCase> All()
    {
        yield return new TestCase("이름이 비어 있으면 IsValid=false", () =>
        {
            var vm = new LoginViewModel();
            Assert.False(vm.IsValid);
        });

        yield return new TestCase("비밀번호가 4자 미만이면 IsValid=false", () =>
        {
            var vm = new LoginViewModel { Name = "홍길동", Password = "abc" };
            Assert.False(vm.IsValid);
        });

        yield return new TestCase("유효한 입력이면 IsValid=true", () =>
        {
            var vm = new LoginViewModel { Name = "홍길동", Password = "pass1234" };
            Assert.True(vm.IsValid);
        });

        yield return new TestCase("LoginCommand가 성공 상태를 설정", () =>
        {
            var vm = new LoginViewModel { Name = "홍길동", Password = "pass1234" };
            vm.LoginCommand.Execute(null);
            Assert.True(vm.Status.StartsWith("환영합니다"), $"실제: {vm.Status}");
        });
    }
}

// ---------- 앱 뷰 모델 ----------

public class MainViewModel : INotifyPropertyChanged
{
    private string _status = "아직 실행되지 않음";

    public ObservableCollection<TestResult> Results { get; } = new();
    public RelayCommand RunTestsCommand { get; }

    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public MainViewModel()
    {
        RunTestsCommand = new RelayCommand(_ =>
        {
            Results.Clear();
            foreach (var r in MiniTestRunner.Run(LoginTests.All()))
            {
                Results.Add(r);
            }
            int passed = Results.Count(r => r.Passed);
            Status = $"{passed}/{Results.Count} 통과";
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class BoolToResultConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (bool)value ? "통과" : "실패";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;

    public RelayCommand(Action<object?> execute) => _execute = execute;

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => _execute(parameter);
}

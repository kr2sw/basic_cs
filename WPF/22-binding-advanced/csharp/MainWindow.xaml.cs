using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace Ch22;

public partial class MainWindow : Window
{
    public Person Person { get; } = new Person();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        // PriorityBinding 데모: 1초 후 "빠른 값", 3초 후 "느린 값"이 로드된다고 가정합니다.
        var fastTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        fastTimer.Tick += (_, _) =>
        {
            Person.NicknameFast = "번개";
            fastTimer.Stop();
        };
        fastTimer.Start();

        var slowTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
        slowTimer.Tick += (_, _) =>
        {
            Person.NicknameSlow = "천천히";
            slowTimer.Stop();
        };
        slowTimer.Start();
    }

    private void CommitNote_Click(object sender, RoutedEventArgs e)
    {
        // UpdateSourceTrigger=Explicit 바인딩은 코드에서 명시적으로 갱신해야 합니다.
        noteBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
    }
}

// 두 개 이상의 값을 하나로 합치는 컨버터
public class FullNameConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is null || values.Length < 2)
        {
            return "";
        }
        return $"{values[0]} {values[1]}";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class Person : INotifyPropertyChanged
{
    private string _lastName = "홍";
    private string _firstName = "길동";
    private string _note = "";
    private string? _nicknameFast;
    private string? _nicknameSlow;

    public string LastName
    {
        get => _lastName;
        set { _lastName = value; OnPropertyChanged(); }
    }

    public string FirstName
    {
        get => _firstName;
        set { _firstName = value; OnPropertyChanged(); }
    }

    public string Note
    {
        get => _note;
        set { _note = value; OnPropertyChanged(); }
    }

    // PriorityBinding: 처음에는 null(값 없음)이라 아래 바인딩으로 대체됩니다.
    public string? NicknameFast
    {
        get => _nicknameFast;
        set { _nicknameFast = value; OnPropertyChanged(); }
    }

    public string? NicknameSlow
    {
        get => _nicknameSlow;
        set { _nicknameSlow = value; OnPropertyChanged(); }
    }

    // 최종 폴백 값 (읽기 전용)
    public string NicknameFallback => "닉네임 없음";

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

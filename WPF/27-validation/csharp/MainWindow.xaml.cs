using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Ch27;

public partial class MainWindow : Window
{
    private readonly Person _person = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _person;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        resultText.Text = _person.HasErrors
            ? "오류가 있어 저장할 수 없습니다."
            : $"저장됨: {_person.Name}, {_person.Age}세";
    }
}

// 모델 자체가 실시간 유효성 검사를 수행하는 방식 (INotifyDataErrorInfo)
public class Person : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _errors = new();
    private string _name = "";
    private int _age;

    public Person()
    {
        ValidateName();
    }

    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;
            _name = value;
            ValidateName();
            OnPropertyChanged();
        }
    }

    public int Age
    {
        get => _age;
        set
        {
            if (_age == value) return;
            _age = value;
            OnPropertyChanged();
        }
    }

    private void ValidateName()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            SetErrors(nameof(Name), ["이름은 필수 입력입니다."]);
        }
        else
        {
            ClearErrors(nameof(Name));
        }
    }

    private void SetErrors(string propertyName, IEnumerable<string> messages)
    {
        if (_errors.TryGetValue(propertyName, out var list) && list.SequenceEqual(messages))
        {
            return;
        }
        _errors[propertyName] = messages.ToList();
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(HasErrors));
    }

    private void ClearErrors(string propertyName)
    {
        if (_errors.Remove(propertyName))
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }
    }

    public bool HasErrors => _errors.Count > 0;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName is not null && _errors.TryGetValue(propertyName, out var list))
        {
            return list;
        }
        if (propertyName is null)
        {
            return _errors.Values.SelectMany(v => v).ToList();
        }
        return Array.Empty<string>();
    }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

// XAML에서 선언적으로 연결하는 바인딩 검증 규칙
public class AgeRangeRule : ValidationRule
{
    public int Min { get; set; }
    public int Max { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (int.TryParse(value?.ToString(), out int age) && age >= Min && age <= Max)
        {
            return ValidationResult.ValidResult;
        }
        return new ValidationResult(false, $"나이는 {Min}~{Max} 사이여야 합니다.");
    }
}

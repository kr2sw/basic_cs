using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Ch13;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<Employee> _employees = new();
    private int _nextId = 6;

    public MainWindow()
    {
        InitializeComponent();
        _employees.Add(new Employee { Id = 1, Name = "홍길동", Department = "개발팀", Position = "선임", Salary = 5000, IsActive = true });
        _employees.Add(new Employee { Id = 2, Name = "김철수", Department = "개발팀", Position = "주임", Salary = 3500, IsActive = true });
        _employees.Add(new Employee { Id = 3, Name = "이영희", Department = "디자인팀", Position = "과장", Salary = 4500, IsActive = true });
        _employees.Add(new Employee { Id = 4, Name = "박민수", Department = "기획팀", Position = "대리", Salary = 3800, IsActive = false });
        _employees.Add(new Employee { Id = 5, Name = "정수연", Department = "개발팀", Position = "사원", Salary = 2800, IsActive = true });
        dataGrid.ItemsSource = _employees;
        UpdateStatus();
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        _employees.Add(new Employee
        {
            Id = _nextId++,
            Name = "신입사원",
            Department = "개발팀",
            Position = "사원",
            Salary = 2800,
            IsActive = true
        });
        UpdateStatus();
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (dataGrid.SelectedItem is Employee emp)
            _employees.Remove(emp);
        UpdateStatus();
    }

    private void Commit_Click(object sender, RoutedEventArgs e)
    {
        dataGrid.CommitEdit();
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        statusText.Text = $"총 {_employees.Count}명의 직원";
    }
}

public class Employee : INotifyPropertyChanged
{
    private int _id;
    private string _name = "";
    private string _department = "";
    private string _position = "";
    private decimal _salary;
    private bool _isActive;

    public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
    public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
    public string Department { get => _department; set { _department = value; OnPropertyChanged(); } }
    public string Position { get => _position; set { _position = value; OnPropertyChanged(); } }
    public decimal Salary { get => _salary; set { _salary = value; OnPropertyChanged(); } }
    public bool IsActive { get => _isActive; set { _isActive = value; OnPropertyChanged(); } }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

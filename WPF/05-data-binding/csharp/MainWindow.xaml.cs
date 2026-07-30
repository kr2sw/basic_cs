using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Ch05;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new Person
        {
            Name = "홍길동",
            Age = 30,
            Email = "hong@example.com"
        };
    }

    private void ShowInfo_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is Person p)
        {
            MessageBox.Show($"이름: {p.Name}\n나이: {p.Age}\n이메일: {p.Email}", "Person 정보");
        }
    }
}

public class Person : INotifyPropertyChanged
{
    private string _name = "";
    private int _age;
    private string _email = "";

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    public int Age
    {
        get => _age;
        set { _age = value; OnPropertyChanged(); }
    }

    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

using System.Windows;

namespace Ch01;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnGreet_Click(object sender, RoutedEventArgs e)
    {
        var name = string.IsNullOrWhiteSpace(nameBox.Text) ? "World" : nameBox.Text;
        MessageBox.Show($"Hello, {name}!", "WPF");
    }
}

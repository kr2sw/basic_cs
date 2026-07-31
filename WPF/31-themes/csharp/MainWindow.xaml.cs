using System.Windows;

namespace Ch31;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Light_Click(object sender, RoutedEventArgs e)
        => App.ApplyTheme("Themes/Light.xaml");

    private void Dark_Click(object sender, RoutedEventArgs e)
        => App.ApplyTheme("Themes/Dark.xaml");
}

using System.Windows;

namespace Ch19;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    private void LightTheme_Click(object sender, RoutedEventArgs e)
    {
        var dict = new ResourceDictionary { Source = new System.Uri("Themes/Light.xaml", System.UriKind.Relative) };
        Application.Current.Resources.MergedDictionaries[0] = dict;
        statusText.Text = "현재 테마: 라이트";
    }

    private void DarkTheme_Click(object sender, RoutedEventArgs e)
    {
        var dict = new ResourceDictionary { Source = new System.Uri("Themes/Dark.xaml", System.UriKind.Relative) };
        Application.Current.Resources.MergedDictionaries[0] = dict;
        statusText.Text = "현재 테마: 다크";
    }
}

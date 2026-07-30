using System.Windows;
using System.Windows.Controls;

namespace Ch15;

public partial class Page1 : Page
{
    public Page1() => InitializeComponent();

    private void GoToSettings_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Page2());
    }
}

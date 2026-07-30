using System.Windows;
using System.Windows.Controls;

namespace Ch15;

public partial class Page2 : Page
{
    public Page2() => InitializeComponent();

    private void GoToHome_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Page1());
    }
}

using System.Windows;
using System.Windows.Controls;

namespace Ch15;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        mainFrame.Navigate(new Page1());
        UpdateStatus();
    }

    private void Home_Click(object sender, RoutedEventArgs e)
    {
        mainFrame.Navigate(new Page1());
        UpdateStatus();
    }

    private void Settings_Click(object sender, RoutedEventArgs e)
    {
        mainFrame.Navigate(new Page2());
        UpdateStatus();
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        if (mainFrame.CanGoBack)
        {
            mainFrame.GoBack();
            UpdateStatus();
        }
    }

    private void Forward_Click(object sender, RoutedEventArgs e)
    {
        if (mainFrame.CanGoForward)
        {
            mainFrame.GoForward();
            UpdateStatus();
        }
    }

    private void UpdateStatus()
    {
        statusText.Text = $"저널: 뒤로({mainFrame.CanGoBack}) / 앞으로({mainFrame.CanGoForward})";
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ch04;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Border_MouseEnter(object sender, MouseEventArgs e)
    {
        lblHover.Text = "MouseEnter: 마우스가 영역 안에 있습니다.";
    }

    private void Border_MouseLeave(object sender, MouseEventArgs e)
    {
        lblHover.Text = "MouseLeave: 마우스가 영역을 벗어났습니다.";
    }

    private void BtnClick_Click(object sender, RoutedEventArgs e)
    {
        Log($"Button Click: {((Button)sender).Content}");
    }

    private void BtnDouble_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        Log($"DoubleClick: 더블클릭 발생");
    }

    private void Log(string msg)
    {
        lbLog.Items.Add($"[{System.DateTime.Now:HH:mm:ss}] {msg}");
        lbLog.ScrollIntoView(lbLog.Items[^1]);
    }
}

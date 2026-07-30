using System.Windows;

namespace Ch03;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnShow_Click(object sender, RoutedEventArgs e)
    {
        var rb = rbA.IsChecked == true ? "A" : "B";
        MessageBox.Show(
            $"TextBox: {txtInput.Text}\n" +
            $"CheckBox: {chkOption.IsChecked}\n" +
            $"RadioButton: {rb}\n" +
            $"Slider: {sldValue.Value}",
            "컨트롤 값");
    }
}

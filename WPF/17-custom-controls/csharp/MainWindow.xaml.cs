using System.Windows;
using System.Windows.Media;

namespace Ch17;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
    {
        previewBorder.Background = new SolidColorBrush(colorPicker.SelectedColor);
        colorText.Text = $"RGB: {colorPicker.SelectedColor.R}, {colorPicker.SelectedColor.G}, {colorPicker.SelectedColor.B}";
    }
}

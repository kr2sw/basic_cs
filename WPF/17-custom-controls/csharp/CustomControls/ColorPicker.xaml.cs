using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ch17;

public partial class ColorPicker : UserControl
{
    public static readonly RoutedEvent ColorChangedEvent =
        EventManager.RegisterRoutedEvent("ColorChanged", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(ColorPicker));

    public event RoutedEventHandler ColorChanged
    {
        add => AddHandler(ColorChangedEvent, value);
        remove => RemoveHandler(ColorChangedEvent, value);
    }

    public static readonly DependencyProperty SelectedColorProperty =
        DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker),
            new PropertyMetadata(Colors.Black));

    public Color SelectedColor
    {
        get => (Color)GetValue(SelectedColorProperty);
        set => SetValue(SelectedColorProperty, value);
    }

    public ColorPicker()
    {
        InitializeComponent();

        redSlider.ValueChanged += Slider_ValueChanged;
        greenSlider.ValueChanged += Slider_ValueChanged;
        blueSlider.ValueChanged += Slider_ValueChanged;
    }

    private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        var color = Color.FromRgb((byte)redSlider.Value, (byte)greenSlider.Value, (byte)blueSlider.Value);
        SelectedColor = color;
        preview.Background = new SolidColorBrush(color);
        redValue.Text = ((byte)redSlider.Value).ToString();
        greenValue.Text = ((byte)greenSlider.Value).ToString();
        blueValue.Text = ((byte)blueSlider.Value).ToString();
        RaiseEvent(new RoutedEventArgs(ColorChangedEvent));
    }
}

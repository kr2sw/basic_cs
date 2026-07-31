using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Ch28;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}

public class MainViewModel : INotifyPropertyChanged
{
    private bool _active = true;
    private decimal? _price;
    private double _score = 75;

    public bool Active
    {
        get => _active;
        set { _active = value; OnPropertyChanged(); }
    }

    public decimal? Price
    {
        get => _price;
        set { _price = value; OnPropertyChanged(); }
    }

    public double Score
    {
        get => _score;
        set { _score = value; OnPropertyChanged(); }
    }

    public double Min => 0;
    public double Max => 100;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

// bool을 브러시로. ConverterParameter로 "참색,거짓색"을 외부에서 주입.
public class BooleanToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var parts = (parameter as string)?.Split(',');
        var brush = (bool)value ? parts?[0] : parts?[1];
        return brush is null
            ? Brushes.Gray
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString(brush));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

// null 여부로 Visibility 결정. ConverterParameter="true"면 null일 때 표시.
public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool showWhenNull = string.Equals(parameter?.ToString(), "true", StringComparison.OrdinalIgnoreCase);
        bool visible = (value is null) == showWhenNull;
        return visible ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

// 여러 값을 조합해 범위 이탈 경고 표시.
public class RangeWarningConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double v = System.Convert.ToDouble(values[0]);
        double min = System.Convert.ToDouble(values[1]);
        double max = System.Convert.ToDouble(values[2]);
        return (v < min || v > max) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

using System.Windows;
using System.Windows.Controls;

namespace Ch24;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ReadWatermark_Click(object sender, RoutedEventArgs e)
    {
        // 코드에서 첨부 속성 값을 읽습니다. 요소마다 다른 값을 가집니다.
        resultText.Text = string.Join("\n", new[]
        {
            $"nameBox  : {Watermark.GetText(nameBox)}",
            $"emailBox : {Watermark.GetText(emailBox)}",
            $"plainBox : {Watermark.GetText(plainBox)}  (스타일 기본값)",
        });
    }
}

// 첨부 속성(Attached Property) 정의 클래스
public static class Watermark
{
    // RegisterAttached로 첨부 속성을 등록합니다.
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(Watermark),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender));

    public static string GetText(DependencyObject obj)
        => (string)obj.GetValue(TextProperty);

    public static void SetText(DependencyObject obj, string value)
        => obj.SetValue(TextProperty, value);
}

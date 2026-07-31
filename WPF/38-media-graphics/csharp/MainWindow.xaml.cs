using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ch38;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // RenderTargetBitmap으로 화면 밖(오프스크린) 렌더링 후 PNG 인코딩
    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "PNG 이미지|*.png",
            FileName = "drawing.png"
        };

        if (dlg.ShowDialog(this) != true) return;

        var rtb = new RenderTargetBitmap(
            (int)drawingHost.ActualWidth, (int)drawingHost.ActualHeight,
            96, 96, PixelFormats.Pbgra32);
        rtb.Render(drawingHost);

        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(rtb));

        using (var fs = File.Create(dlg.FileName))
        {
            encoder.Save(fs);
        }

        statusText.Text = $"저장됨: {dlg.FileName}";
    }
}

// 시각 요소(Visual)를 직접 관리하는 경량 렌더링 호스트
public class DrawingVisualHost : FrameworkElement
{
    private readonly VisualCollection _children;

    public DrawingVisualHost()
    {
        _children = new VisualCollection(this);
        AddDrawingVisual();
    }

    protected override int VisualChildrenCount => _children.Count;

    protected override Visual GetVisualChild(int index) => _children[index];

    private void AddDrawingVisual()
    {
        var visual = new DrawingVisual();
        using (var dc = visual.RenderOpen())
        {
            // 그라데이션 배경 사각형
            var brush = new LinearGradientBrush(
                Color.FromRgb(30, 60, 120), Color.FromRgb(90, 160, 220), 45);
            dc.DrawRectangle(brush, null, new Rect(10, 10, 180, 160));

            // 원
            dc.DrawEllipse(Brushes.Tomato, null, new Point(300, 90), 70, 70);

            // 선
            dc.DrawLine(new Pen(Brushes.White, 3), new Point(10, 180), new Point(390, 60));

            // 텍스트 (DPI 보정 포함)
            var dpi = VisualTreeHelper.GetDpi(this);
            var text = new FormattedText(
                "DrawingVisual - 벡터 그래픽",
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Malgun Gothic"), 16,
                Brushes.White, dpi.PixelsPerDip);
            dc.DrawText(text, new Point(16, 190));
        }
        _children.Add(visual);
    }
}

using System.Windows;
using System.Windows.Controls;

namespace Ch37;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}

// WrapPanel과 유사: 가로 공간이 부족하면 다음 줄로 이동
public class WrapFlowPanel : Panel
{
    protected override Size MeasureOverride(Size availableSize)
    {
        double width = 0, height = 0, lineWidth = 0, lineHeight = 0;

        foreach (UIElement child in InternalChildren)
        {
            child.Measure(availableSize);
            double w = child.DesiredSize.Width;
            double h = child.DesiredSize.Height;

            if (lineWidth + w > availableSize.Width && lineWidth > 0)
            {
                width = Math.Max(width, lineWidth);
                height += lineHeight;
                lineWidth = w;
                lineHeight = h;
            }
            else
            {
                lineWidth += w;
                lineHeight = Math.Max(lineHeight, h);
            }
        }

        width = Math.Max(width, lineWidth);
        height += lineHeight;

        return double.IsPositiveInfinity(availableSize.Width)
            ? new Size(width, height)
            : new Size(availableSize.Width, height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double x = 0, y = 0, lineHeight = 0;

        foreach (UIElement child in InternalChildren)
        {
            if (x + child.DesiredSize.Width > finalSize.Width && x > 0)
            {
                x = 0;
                y += lineHeight;
                lineHeight = 0;
            }

            child.Arrange(new Rect(x, y, child.DesiredSize.Width, child.DesiredSize.Height));
            x += child.DesiredSize.Width;
            lineHeight = Math.Max(lineHeight, child.DesiredSize.Height);
        }

        return finalSize;
    }
}

// 자식을 원형으로 배치. Radius는 디펜던시 속성이라 슬라이더로 조절 가능.
public class RadialPanel : Panel
{
    public static readonly DependencyProperty RadiusProperty =
        DependencyProperty.Register(
            nameof(Radius), typeof(double), typeof(RadialPanel),
            new FrameworkPropertyMetadata(110.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsArrange));

    public double Radius
    {
        get => (double)GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        double max = 0;
        foreach (UIElement child in InternalChildren)
        {
            child.Measure(availableSize);
            max = Math.Max(max, Math.Max(child.DesiredSize.Width, child.DesiredSize.Height));
        }
        double d = Radius * 2 + max * 2;
        return new Size(d, d);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        int count = InternalChildren.Count;
        if (count == 0) return finalSize;

        var center = new Point(finalSize.Width / 2, finalSize.Height / 2);
        double step = 2 * Math.PI / count;

        for (int i = 0; i < count; i++)
        {
            UIElement child = InternalChildren[i];
            double angle = i * step - Math.PI / 2;   // 12시 방향부터 시계 방향

            var p = new Point(
                center.X + Radius * Math.Cos(angle) - child.DesiredSize.Width / 2,
                center.Y + Radius * Math.Sin(angle) - child.DesiredSize.Height / 2);

            child.Arrange(new Rect(p, child.DesiredSize));
        }

        return finalSize;
    }
}

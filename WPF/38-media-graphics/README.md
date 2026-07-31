# 38: 미디어와 그래픽 — DrawingVisual, RenderTargetBitmap, 효과

## 학습 목표
- `VisualCollection`과 `DrawingVisual`로 경량 벡터 그래픽
- `RenderTargetBitmap`으로 오프스크린 렌더링 후 PNG 저장
- XAML `Effect`(DropShadowEffect) 활용
- DPI 보정(`VisualTreeHelper.GetDpi`)을 고려한 텍스트 그리기

## DrawingVisual로 그리기

컨트롤 트리 대신 시각 트리를 직접 관리하면 렌더링이 가볍습니다.
`FrameworkElement`를 상속해 `VisualChildrenCount`/`GetVisualChild`만 구현합니다.

```csharp
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
        using (var dc = visual.RenderOpen())   // DrawingContext 획득
        {
            dc.DrawRectangle(brush, null, new Rect(10, 10, 180, 160));
            dc.DrawEllipse(Brushes.Tomato, null, new Point(300, 90), 70, 70);
            dc.DrawLine(new Pen(Brushes.White, 3), new Point(10, 180), new Point(390, 60));
        }
        _children.Add(visual);
    }
}
```

VB.NET:

```vb
Using dc = visual.RenderOpen()
    dc.DrawRectangle(brush, Nothing, New Rect(10, 10, 180, 160))
    dc.DrawEllipse(Brushes.Tomato, Nothing, New Point(300, 90), 70, 70)
End Using
```

## 텍스트 그리기 (DPI 보정)

```csharp
var dpi = VisualTreeHelper.GetDpi(this);
var text = new FormattedText(
    "DrawingVisual - 벡터 그래픽",
    CultureInfo.CurrentCulture,
    FlowDirection.LeftToRight,
    new Typeface("Malgun Gothic"), 16,
    Brushes.White, dpi.PixelsPerDip);
dc.DrawText(text, new Point(16, 190));
```

`pixelsPerDip`을 DPI 값으로 넘겨야 고해상도 모니터에서도 선명합니다.

## RenderTargetBitmap으로 저장

화면에 그려진 요소를 비트맵으로 렌더링한 뒤 PNG 인코더로 저장합니다.

```csharp
var rtb = new RenderTargetBitmap(
    (int)drawingHost.ActualWidth, (int)drawingHost.ActualHeight,
    96, 96, PixelFormats.Pbgra32);
rtb.Render(drawingHost);

var encoder = new PngBitmapEncoder();
encoder.Frames.Add(BitmapFrame.Create(rtb));

using (var fs = File.Create(filePath))
{
    encoder.Save(fs);
}
```

VB.NET:

```vb
Dim rtb As New RenderTargetBitmap(CInt(drawingHost.ActualWidth), CInt(drawingHost.ActualHeight), 96, 96, PixelFormats.Pbgra32)
rtb.Render(drawingHost)
Dim encoder As New PngBitmapEncoder()
encoder.Frames.Add(BitmapFrame.Create(rtb))
Using fs = File.Create(dlg.FileName)
    encoder.Save(fs)
End Using
```

## XAML 효과

```xml
<Border.Effect>
    <DropShadowEffect BlurRadius="8" ShadowDepth="3" Opacity="0.5"/>
</Border.Effect>
```

## 정리

- `DrawingVisual` + `DrawingContext`로 고성능 벡터 그래픽
- `RenderTargetBitmap`으로 무엇이든 비트맵으로 추출 가능
- 효과는 `UIElement.Effect`에 `Effect` 파생 객체 지정

# 37: 커스텀 패널 — MeasureOverride / ArrangeOverride

## 학습 목표
- WPF 레이아웃 파이프라인 이해 (Measure → Arrange)
- `Panel` 상속으로 커스텀 패널 구현
- `DependencyProperty`와 `AffectsMeasure`/`AffectsArrange` 플래그
- WrapPanel·원형 배치 패널 직접 만들어보기

## 레이아웃 2단계

WPF 패널은 두 번의 통과(pass)로 자식을 배치합니다.

1. **Measure**: 각 자식의 `Measure(size)` → 자식이 원하는 크기(`DesiredSize`) 반환
2. **Arrange**: 계산된 위치에 `Arrange(rect)` → 실제 화면 배치

```csharp
public class WrapFlowPanel : Panel
{
    protected override Size MeasureOverride(Size availableSize)
    {
        // 자식들의 DesiredSize를 모아 이 패널이 원하는 크기를 반환
        foreach (UIElement child in InternalChildren)
        {
            child.Measure(availableSize);
            // 줄 계산: lineWidth + w > availableSize.Width → 줄바꿈
        }
        return new Size(...);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double x = 0, y = 0;
        foreach (UIElement child in InternalChildren)
        {
            if (x + child.DesiredSize.Width > finalSize.Width && x > 0)
            {
                x = 0;
                y += lineHeight;   // 다음 줄로
            }
            child.Arrange(new Rect(x, y, child.DesiredSize.Width, child.DesiredSize.Height));
            x += child.DesiredSize.Width;
        }
        return finalSize;
    }
}
```

VB.NET:

```vb
Protected Overrides Function MeasureOverride(availableSize As Size) As Size
    For Each child As UIElement In InternalChildren
        child.Measure(availableSize)
    Next
    Return New Size(width, height)
End Function

Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
    child.Arrange(New Rect(x, y, child.DesiredSize.Width, child.DesiredSize.Height))
    Return finalSize
End Function
```

## 의존성 속성으로 레이아웃 옵션 노출

`Radius`를 디펜던시 속성으로 만들면 XAML/Slider 바인딩으로 조절할 수 있습니다.

```csharp
public static readonly DependencyProperty RadiusProperty =
    DependencyProperty.Register(
        nameof(Radius), typeof(double), typeof(RadialPanel),
        new FrameworkPropertyMetadata(110.0,
            FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsArrange));
```

- `AffectsMeasure`: 속성 변경 시 Measure부터 다시 실행
- `AffectsArrange`: 속성 변경 시 Arrange부터 다시 실행

## 원형 배치 패널

```csharp
double step = 2 * Math.PI / count;
for (int i = 0; i < count; i++)
{
    double angle = i * step - Math.PI / 2;   // 12시 방향부터
    var p = new Point(
        center.X + Radius * Math.Cos(angle) - child.DesiredSize.Width / 2,
        center.Y + Radius * Math.Sin(angle) - child.DesiredSize.Height / 2);
    child.Arrange(new Rect(p, child.DesiredSize));
}
```

## XAML에서 사용

```xml
<local:RadialPanel x:Name="radial" Radius="110" Width="320" Height="320">
    <Ellipse Width="44" Height="44" Fill="SteelBlue"/>
    <Ellipse Width="44" Height="44" Fill="Tomato"/>
</local:RadialPanel>

<Slider Minimum="40" Maximum="180" Value="{Binding ElementName=radial, Path=Radius}"/>
```

## 주의점

- `Measure`에서 자식 크기를 모르는 상태로 Arrange에서 가정하지 말 것
- 무한 크기(`double.PositiveInfinity`) 처리: `ScrollViewer` 안에서 유효
- 불필요한 레이아웃 재실행을 막으려면 속성 변경 시점 플래그를 정확히 설정

## 실행

```bash
cd csharp
dotnet run
```

```bash
cd vbnet
dotnet run
```

## 정리

- 커스텀 패널 = `MeasureOverride`(크기 계산) + `ArrangeOverride`(배치)
- 자식은 `InternalChildren`으로 접근
- 옵션은 디펜던시 속성으로 만들고 레이아웃 플래그 지정

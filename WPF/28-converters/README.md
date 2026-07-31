# 28: 컨버터 — IValueConverter, IMultiValueConverter, Parameter

## 학습 목표
- `IValueConverter` 구현과 바인딩 적용
- `ConverterParameter`로 전환 옵션 외부 주입
- `IMultiValueConverter`로 여러 값 조합
- 단방향 컨버터의 `ConvertBack` 금지 패턴

## 왜 컨버터인가

뷰 모델은 데이터를 저장하고, 뷰는 그 데이터를 다르게 **표현**합니다.
불리언 값 하나를 "초록 텍스트/빨간 텍스트"로 바꾸는 건 비즈니스 로직이
아니므로 뷰 모델에 두지 않고 컨버터에 둡니다.

## IValueConverter

```csharp
public class BooleanToBrushConverter : IValueConverter
{
    // "참색,거짓색" 을 ConverterParameter로 받아 재사용성을 높인다
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var parts = (parameter as string)?.Split(',');
        var brush = (bool)value ? parts?[0] : parts?[1];
        return brush is null
            ? Brushes.Gray
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString(brush));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();  // 단방향 전용 명시
}
```

```xml
<Window.Resources>
    <local:BooleanToBrushConverter x:Key="BooleanToBrush"/>
</Window.Resources>

<TextBlock Foreground="{Binding Active,
                       Converter={StaticResource BooleanToBrush},
                       ConverterParameter=#2E7D32,#C62828}"/>
```

VB.NET:

```vb
Public Class BooleanToBrushConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object,
                            culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim parts = CStr(parameter).Split(","c)
        Dim brush = If(DirectCast(value, Boolean), parts(0), parts(1))
        Return New SolidColorBrush(CType(ColorConverter.ConvertFromString(brush), Color))
    End Function
End Class
```

## null 처리: NullToVisibilityConverter

`null`인지에 따라 요소를 보여주거나 숨깁니다. 파라미터로 방향을 선택합니다.

```csharp
public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
{
    bool showWhenNull = string.Equals(parameter?.ToString(), "true", StringComparison.OrdinalIgnoreCase);
    bool visible = (value is null) == showWhenNull;
    return visible ? Visibility.Visible : Visibility.Collapsed;
}
```

```xml
<TextBlock Text="가격 미정"
           Visibility="{Binding Price, Converter={StaticResource NullToVisibility},
                                 ConverterParameter=true}"/>
```

## IMultiValueConverter

여러 바인딩 값을 `values[]`로 받습니다. 점수·최소·최대를 조합해
범위 이탈 경고를 표시하는 예:

```csharp
public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
{
    double v = System.Convert.ToDouble(values[0]);
    double min = System.Convert.ToDouble(values[1]);
    double max = System.Convert.ToDouble(values[2]);
    return (v < min || v > max) ? Visibility.Visible : Visibility.Collapsed;
}
```

```xml
<TextBlock.Visibility>
    <MultiBinding Converter="{StaticResource RangeWarning}">
        <Binding Path="Score"/>
        <Binding Path="Min"/>
        <Binding Path="Max"/>
    </MultiBinding>
</TextBlock.Visibility>
```

## 규칙

- 컨버터는 **상태 없이** 순수하게 작성 (입력 → 출력)
- `ConverterParameter`는 문자열이므로 파싱이 필요할 수 있음
- 예외를 던지기보다 `DependencyProperty.UnsetValue`를 반환하면 바인딩이 "무시"함
- 자주 쓰는 컨버터는 `App.xaml` 리소스로 전역 등록
- 표준 컨버터: `BooleanToVisibilityConverter`, `StringFormat`(컨버터 대체) 활용 우선

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

- 단일 값 → `IValueConverter`, 다중 값 → `IMultiValueConverter`
- 파라미터 주입으로 하나의 컨버터를 여러 곳에서 재사용
- 단방향일 땐 `ConvertBack` 예외로 의도를 명확히

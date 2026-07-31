# 31: 동적 리소스와 테마 — ResourceDictionary, 다크/라이트 테마

## 학습 목표
- `StaticResource`와 `DynamicResource`의 차이
- `MergedDictionaries`로 실행 중 테마 교체
- 테마에 맞게 자동으로 다시 그려지는 스타일 구성

## StaticResource vs DynamicResource

| 구분 | 찾는 시점 | 값 변경 반영 | 용도 |
|------|-----------|--------------|------|
| `StaticResource` | XAML 로드 시 1회 | 없음 | 불변 값, 성능 우선 |
| `DynamicResource` | 참조될 때마다 | 있음 | 테마, 실행 중 변경 |

테마 전환처럼 **실행 중 바뀌는 값은 반드시 `DynamicResource`**여야 합니다.

## 테마 리소스 사전

```xml
<!-- Themes/Light.xaml -->
<ResourceDictionary xmlns="..." xmlns:x="...">
    <SolidColorBrush x:Key="WindowBackgroundBrush" Color="#FFF5F5F5"/>
    <SolidColorBrush x:Key="WindowForegroundBrush" Color="#FF212121"/>
    <SolidColorBrush x:Key="AccentBrush" Color="#FF1976D2"/>
    <SolidColorBrush x:Key="ControlBackgroundBrush" Color="#FFFFFFFF"/>
    <SolidColorBrush x:Key="ControlBorderBrush" Color="#FFCCCCCC"/>
    <SolidColorBrush x:Key="MutedBrush" Color="#FF757575"/>
</ResourceDictionary>
```

다크 테마는 같은 `x:Key`에 다른 색만 넣습니다. 키 이름이 같아야
스타일을 건드리지 않고 색만 바꿀 수 있습니다.

## 전환 로직

`Application.Resources.MergedDictionaries`를 통째로 교체합니다.

```csharp
public static void ApplyTheme(string path)
{
    Resources.MergedDictionaries.Clear();
    Resources.MergedDictionaries.Add(new ResourceDictionary
    {
        Source = new Uri(path, UriKind.Relative)
    });
}
```

```vb
Public Shared Sub ApplyTheme(path As String)
    Resources.MergedDictionaries.Clear()
    Resources.MergedDictionaries.Add(New ResourceDictionary With {
        .Source = New Uri(path, UriKind.Relative)
    })
End Sub
```

## 요소에 적용

```xml
<Window.Background>
    <DynamicResource ResourceKey="WindowBackgroundBrush"/>
</Window.Background>

<Border Background="{DynamicResource ControlBackgroundBrush}"
        BorderBrush="{DynamicResource ControlBorderBrush}"/>
```

## 전역 스타일과 결합

`App.xaml`의 암시적 스타일도 DynamicResource를 사용하면
테마 전환 시 버튼/텍스트박스까지 자동으로 다시 그려집니다.

```xml
<Style TargetType="Button">
    <Setter Property="Background" Value="{DynamicResource ControlBackgroundBrush}"/>
    <Setter Property="Foreground" Value="{DynamicResource WindowForegroundBrush}"/>
</Style>
```

## 정리

- 테마 = 같은 키를 가진 리소스 세트를 교체하는 것
- 바뀌어야 하는 값 → `DynamicResource`
- 대안: `ResourceDictionary`를 앱에 여러 개 두고 설정 저장(40장에서 활용)

## 실행

```bash
cd csharp
dotnet run
```

```bash
cd vbnet
dotnet run
```

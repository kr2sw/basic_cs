# 24: 첨부 속성 — AttachedProperty, 컨테이너별 값

## 학습 목표
- `DependencyProperty.RegisterAttached`로 첨부 속성 정의하기
- 같은 형식의 요소라도 **요소(컨테이너)별로 다른 값** 저장
- 스타일/트리거/템플릿에서 첨부 속성 사용하기
- `GetText` / `SetText` 헬퍼 메서드 규칙

## 첨부 속성이란?

기존 컨트롤에 정의되어 있지 않은 속성을 마치 속성이 있는 것처럼 붙일 수 있는
WPF의 확장 메커니즘입니다. 대표적인 예: `Grid.Row`, `DockPanel.Dock`, `ToolTipService.ToolTip`.

```csharp
public static class Watermark
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(Watermark),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender));

    public static string GetText(DependencyObject obj) => (string)obj.GetValue(TextProperty);
    public static void SetText(DependencyObject obj, string value) => obj.SetValue(TextProperty, value);
}
```

## XAML에서 사용

`local:` 네임스페이스의 클래스 이름으로 접근합니다.

```xml
<TextBox local:Watermark.Text="이름을 입력하세요"/>
<TextBox local:Watermark.Text="example@mail.com"/>
```

각 TextBox는 **자신만의 저장소**에 값을 보관하므로 요소마다 다른 값이 유지됩니다.
이것이 "컨테이너별 값"입니다.

## 바인딩 경로에서 읽기

템플릿/바인딩에서는 **괄호**로 감싸서 접근합니다.

```xml
<TextBlock Text="{Binding RelativeSource={RelativeSource TemplatedParent},
                          Path=(local:Watermark.Text)}"/>
```

## 스타일 + 트리거와 조합

이 예제에서는 내용이 비어 있을 때 워터마크 템플릿으로 교체합니다.

```xml
<Style TargetType="TextBox">
    <Setter Property="local:Watermark.Text" Value="기본 입력란"/>
    <Style.Triggers>
        <Trigger Property="Text" Value="">
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="TextBox">
                        <Border>
                            <Grid>
                                <TextBlock Text="{Binding ... Path=(local:Watermark.Text)}"/>
                                <ScrollViewer x:Name="PART_ContentHost"/>
                            </Grid>
                        </Border>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Trigger>
    </Style.Triggers>
</Style>
```

## 코드에서 읽기/쓰기

```csharp
string wm = Watermark.GetText(nameBox);   // "이름을 입력하세요"
Watermark.SetText(emailBox, "새 값");
```

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

- 첨부 속성은 `static class` + `RegisterAttached` + `Get/Set` 헬퍼 셋이 규칙입니다.
- 각 요소는 값이 없으면 기본값을, 있으면 자신의 값을 반환합니다.
- 부모 요소에 붙이면 자식 요소들이 상속받도록 `FrameworkPropertyMetadataOptions.Inherits`를 줄 수도 있습니다.
- `Path=(owner.Prop)` 구문 덕분에 바인딩과 템플릿에서도 자유롭게 사용할 수 있습니다.

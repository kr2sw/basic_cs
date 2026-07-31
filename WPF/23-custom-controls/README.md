# 23: 커스텀 컨트롤 — ControlTemplate, parts, ThemeInfo

## 학습 목표
- `Control` 상속 + `ControlTemplate`의 커스텀 컨트롤 구조
- `[TemplatePart]`로 템플릿 계약(PART) 선언하기
- `OnApplyTemplate`에서 PART 요소 찾고 이벤트 연결하기
- `ThemeInfo`가 `Themes/Generic.xaml`을 찾는 원리

## 커스텀 컨트롤 vs UserControl

| | UserControl | 커스텀 컨트롤 |
|---|---|---|
| 구조 | XAML + 코드 결합 | 코드 + 외부 템플릿 |
| 템플릿 재정의 | 제한적 | 완전 자유 (`ControlTemplate`) |
| 스타일 테마 | 부분만 | 완전 지원 |
| 용도 | 화면 단위 조합 | 재사용 원자적 컨트롤 |

## 기본 구조

`Control`을 상속하면 기본 스타일 키가 필요합니다.
`Themes/Generic.xaml`의 `Style`이 자동으로 적용됩니다.

```csharp
static StarRatingControl()
{
    // 기본 스타일 키를 자신의 타입으로 지정
    DefaultStyleKeyProperty.OverrideMetadata(typeof(StarRatingControl),
        new FrameworkPropertyMetadata(typeof(StarRatingControl)));
}
```

## TemplatePart 계약

템플릿 작성자에게 "이 이름의 요소를 넣어야 한다"를 문서화합니다.

```csharp
[TemplatePart(Name = "PART_Stars", Type = typeof(StackPanel))]
[TemplatePart(Name = "PART_Text", Type = typeof(TextBlock))]
public class StarRatingControl : Control
```

```xml
<ControlTemplate TargetType="{x:Type local:StarRatingControl}">
    <StackPanel>
        <StackPanel x:Name="PART_Stars" Orientation="Horizontal"/>
        <TextBlock x:Name="PART_Text"/>
    </StackPanel>
</ControlTemplate>
```

## OnApplyTemplate에서 PART 연결

템플릿이 적용된 후 호출되는 시점에 요소를 찾아 이벤트를 연결합니다.

```csharp
public override void OnApplyTemplate()
{
    base.OnApplyTemplate();
    var stars = GetTemplateChild("PART_Stars") as StackPanel;
    if (stars is null) return;

    for (int i = 1; i <= MaxRating; i++)
    {
        int index = i;
        var star = new Button { Content = "★", Tag = index };
        star.Click += (_, _) => Rating = index; // 클릭 시 별점 반영
        stars.Children.Add(star);
    }
}
```

## ThemeInfo

`ThemeInfo`는 WPF에게 리소스 딕셔너리 위치를 알려줍니다.

```csharp
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]
```

- 첫 번째 인자: 테마 전용 딕셔너리 위치 (`None`)
- 두 번째 인자: 일반 딕셔너리(`Themes/Generic.xaml`) 위치 (`SourceAssembly`)
- `Themes/Generic.xaml`은 **폴더 이름이 반드시 `Themes`** 여야 합니다.

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

- `DependencyProperty`로 상태(별점)를 정의하면 바인딩/애니메이션/스타일 트리거에 모두 노출됩니다.
- `DefaultStyleKey` + `Generic.xaml` + `ThemeInfo`의 조합이 커스텀 컨트롤의 뼈대입니다.
- `[TemplatePart]`는 선택 사항이지만 협업과 재템플릿팅을 위해 필수에 가깝습니다.

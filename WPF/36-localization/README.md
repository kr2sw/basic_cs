# 36: 지역화 — 리소스, CultureInfo, 다국어

## 학습 목표
- 문화권별 리소스 사전 구성 (ko-KR / en-US)
- `DynamicResource`로 언어 전환 즉시 반영
- `CultureInfo.CurrentCulture`/`CurrentUICulture` 갱신
- 숫자·날짜의 문화권별 포맷

## 문화권별 리소스 사전

```xml
<!-- Resources/ko-KR.xaml -->
<ResourceDictionary ...>
    <sys:String x:Key="WindowTitle">36 - 지역화 (ko-KR)</sys:String>
    <sys:String x:Key="EnterName">이름 입력</sys:String>
    <sys:String x:Key="GreetingFormat">안녕하세요, {0}님!</sys:String>
</ResourceDictionary>
```

같은 `x:Key`에 언어별 문자열만 다르게 넣습니다.

## 문화권 전환 서비스

```csharp
public static void SetCulture(string name)
{
    var culture = CultureInfo.GetCultureInfo(name);
    CultureInfo.CurrentCulture = culture;
    CultureInfo.CurrentUICulture = culture;

    Application.Current.Resources.MergedDictionaries.Clear();
    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
    {
        Source = new Uri($"Resources/{name}.xaml", UriKind.Relative)
    });
}
```

VB.NET:

```vb
Public Module Localization
    Public Sub SetCulture(name As String)
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(name)
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(name)
        Application.Current.Resources.MergedDictionaries.Clear()
        Application.Current.Resources.MergedDictionaries.Add(New ResourceDictionary With {
            .Source = New Uri($"Resources/{name}.xaml", UriKind.Relative)
        })
    End Sub
End Module
```

## XAML에서 사용

```xml
<TextBlock Text="{DynamicResource EnterName}"/>
<Button Content="{DynamicResource GreetButton}"/>
```

`DynamicResource`이므로 사전이 교체되는 순간 화면이 다시 그려집니다.

## 코드에서 문화권 문자열 가져오기

```csharp
var format = (string)Application.Current.Resources["GreetingFormat"];
Greeting = string.Format(CultureInfo.CurrentUICulture, format, Name);
```

## 숫자/날짜 문화권 포맷

`CurrentUICulture`를 전달하면 로케일에 맞는 형식이 적용됩니다.

```csharp
var c = CultureInfo.CurrentUICulture;
CultureSample = $"{1234567.89.ToString("N2", c)} · {DateTime.Now.ToString("d", c)}";
// ko-KR: 1,234,567.89 · 2026-07-31
// en-US: 1,234,567.89 · 7/31/2026
```

## 주의점

- 바인딩에 연결된 `StringFormat`은 언어 변경 후 **새로 바인딩할 때**만
  다시 평가됨 → 실시간 반영이 필요하면 컨버터 + `UpdateTarget()`
- `CurrentCulture`(숫자/날짜)와 `CurrentUICulture`(문자열)는 역할이 다름
- 실무에서는 resx + satellite assembly를 주로 사용
  (이 챕터는 NuGet 없이 사전 방식으로 구현)

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

- 문자열 → 문화권별 ResourceDictionary + DynamicResource
- 로케일 설정 → `CurrentCulture`/`CurrentUICulture`
- 포맷 → 문화권을 명시해 `ToString(format, culture)`

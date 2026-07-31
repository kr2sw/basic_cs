# 33: 고급 내비게이션 — Frame/Page, MVVM 내비게이션

## 학습 목표
- `Frame` + `Page` 기반 내비게이션
- 저널(journal)과 뒤로/앞으로
- `INavigationService` 인터페이스로 Window/Frame 의존성 제거
- `DataType` DataTemplate으로 페이지 콘텐츠 자동 렌더링

## Frame과 Page

`Frame`은 콘텐츠를 바꿔가며 보여주는 컨트롤이고, `Page`는 페이지 단위
콘텐츠입니다. `Frame.Navigate(page)`로 이동하며 **저널이 자동으로 쌓여**
뒤로/앞으로 이동이 가능합니다.

```xml
<Border BorderBrush="#CCCCCC" BorderThickness="1">
    <Frame x:Name="frame"/>
</Border>
```

```csharp
private void Back_Click(object sender, RoutedEventArgs e)
{
    if (frame.CanGoBack) frame.GoBack();
}
```

## MVVM 내비게이션 서비스

VM이 `Frame`을 직접 만지지 않도록 인터페이스로 감쌉니다.

```csharp
public interface INavigationService
{
    void NavigateTo(object viewModel);
}

public class FrameNavigationService : INavigationService
{
    private readonly Frame _frame;

    public FrameNavigationService(Frame frame) => _frame = frame;

    public void NavigateTo(object viewModel)
    {
        var page = new Page { Content = viewModel };
        _frame.Navigate(page);
    }
}
```

VB.NET:

```vb
Public Sub NavigateTo(viewModel As Object) Implements INavigationService.NavigateTo
    Dim page As New Page() With {.Content = viewModel}
    _frame.Navigate(page)
End Sub
```

`Page.Content`에 VM을 넣으면 `DataType` 템플릿이 자동으로 뷰를 선택합니다.

## VM 타입별 페이지 콘텐츠 (App.xaml)

```xml
<DataTemplate DataType="{x:Type local:HomeViewModel}">
    <StackPanel Margin="24">
        <TextBlock Text="홈 페이지" FontSize="22" FontWeight="Bold"/>
        <TextBlock Text="{Binding Greeting}" Foreground="SteelBlue"/>
    </StackPanel>
</DataTemplate>

<DataTemplate DataType="{x:Type local:SettingsViewModel}">
    <StackPanel Margin="24" Width="380">
        <TextBox Text="{Binding Name, UpdateSourceTrigger=PropertyChanged}"/>
        <Slider Minimum="0" Maximum="100" Value="{Binding Volume}"/>
    </StackPanel>
</DataTemplate>
```

## 커맨드로 이동

```csharp
public MainViewModel(INavigationService navigation)
{
    _navigation = navigation;
    GoHomeCommand = new RelayCommand(_ => NavigateHome());
    GoSettingsCommand = new RelayCommand(_ => _navigation.NavigateTo(new SettingsViewModel()));
}
```

## 주의점

- 각 이동이 새 `Page`/VM을 만들면 이전 상태는 저널에 남음
- 화면별 VM을 재사용하려면 `NavigationService`가 인스턴스를 캐시
- `KeepAlive` 설정 없이도 DataTemplate 기반 콘텐츠는 재사용 가능

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

- `Frame.Navigate` + 저널로 기본 뒤로/앞으로 확보
- VM → `INavigationService` 의존, 뷰 선택은 DataTemplate
- 페이지 확장(탭, 타이틀 변경)은 서비스 확장으로 해결

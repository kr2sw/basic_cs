# 32: 대화상자 프레임워크 — 커스텀 다이얼로그, MVVM 친화적

## 학습 목표
- 재사용 가능한 `DialogWindow` 호스트 구현
- `IDialogService`로 Window 의존성 분리
- `DataTemplate`(DataType 매핑)으로 다이얼로그 내용 주입
- `IsDefault`/`IsCancel` 버튼과 `DialogResult`

## 문제: VM에서 Window를 쓰면 테스트가 어렵다

```csharp
// 안티패턴: VM이 구체 Window를 직접 new
var dialog = new NameDialog { Owner = Application.Current.MainWindow };
if (dialog.ShowDialog() == true) { ... }
```

`ShowDialog`를 테스트하려면 VM 대신 인터페이스에 의존시킵니다.

## IDialogService

```csharp
public interface IDialogService
{
    bool? ShowDialog(string title, object content, Window owner);
}

public class DialogService : IDialogService
{
    public bool? ShowDialog(string title, object content, Window owner)
    {
        var dialog = new DialogWindow
        {
            Title = title,
            DataContext = content,
            Owner = owner
        };
        return dialog.ShowDialog();
    }
}
```

## DialogWindow: 내용은 DataTemplate이 결정

호스트 창은 확인/취소 버튼만 갖고, 내용은 `ContentPresenter`가 표시합니다.
`DataContext`가 곧 다이얼로그 VM이므로 `DataType` 템플릿이 자동 적용됩니다.

```xml
<ContentPresenter Content="{Binding}" Margin="24"/>
<Button Content="확인" IsDefault="True"/>
<Button Content="취소" IsCancel="True"/>
```

- `IsDefault=True` → Enter 키로 `DialogResult=true`
- `IsCancel=True` → Esc 키로 `DialogResult=false`

## 내용 뷰 매핑 (App.xaml)

```xml
<Application.Resources>
    <DataTemplate DataType="{x:Type local:NameDialogViewModel}">
        <StackPanel Width="320">
            <TextBlock Text="이름을 입력하세요." FontWeight="Bold"/>
            <TextBox Text="{Binding Name, UpdateSourceTrigger=PropertyChanged}"/>
        </StackPanel>
    </DataTemplate>
</Application.Resources>
```

## VM 사용 예

```csharp
ShowDialogCommand = new RelayCommand(_ =>
{
    var vm = new NameDialogViewModel();
    var result = _dialogs.ShowDialog("이름 입력", vm, Application.Current.MainWindow);
    LastResult = result == true ? $"확인: {vm.Name}" : "취소됨";
});
```

VB.NET:

```vb
Dim result = _dialogs.ShowDialog("이름 입력", vm, Application.Current.MainWindow)
LastResult = If(result = True, $"확인: {vm.Name}", "취소됨")
```

## 확장 아이디어

- `IDialogService.ShowDialog<TViewModel>(...)` 제네릭 버전
- 다이얼로그마다 다른 버튼 세트를 노출하려면 `DialogButtons` 열거형 파라미터
- `OK` 비활성화는 다이얼로그 VM의 `CanClose` 프로퍼티로 `DialogWindow`가 구독
- 대안 라이브러리: MahApps.Metro, HandyControl의 `MetroWindow`/`DialogHost`
  (이 챕터는 순수 WPF로 구현)

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

- VM → `IDialogService` 인터페이스에만 의존
- `DialogWindow`(쉘) + `DataTemplate`(내용) 분리
- `IsDefault`/`IsCancel`로 키보드 동작까지 기본 제공

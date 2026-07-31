# 26: WPF + VB — MVVM 패턴, INotifyPropertyChanged

## 소개

WPF(Windows Presentation Foundation) 애플리케이션의 표준 구조인 MVVM(Model-View-ViewModel) 패턴을 다룹니다. WPF는 UI 스레드(WPF 어셈블리)가 필요하므로, 예제에서는 `INotifyPropertyChanged`와 `ObservableCollection` 같은 바인딩의 핵심 메커니즘을 메모리로 재현합니다.

## 주요 개념

### 1. MVVM 3계층

- **Model**: 순수 데이터와 비즈니스 규칙
- **View**: XAML로 만든 화면 (버튼, 목록)
- **ViewModel**: View가 필요로 하는 데이터/명령/상태 (`View.DataContext`)

View는 ViewModel을 관찰(구독)하고, ViewModel은 View를 직접 참조하지 않습니다.

### 2. INotifyPropertyChanged — 속성 변경 알림

ViewModel이 속성을 바꾸면 UI가 그 사실을 알아야 갱신됩니다. 이를 알리는 인터페이스가 `INotifyPropertyChanged`입니다.

```vb
Public Class BindableBase
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub SetProperty(Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional name As String = Nothing)
        storage = value
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub
End Class
```

### 3. ObservableCollection — 컬렉션 변경 알림

목록의 추가/제거를 UI에 알립니다. `CollectionChanged` 이벤트가 발생합니다.

```vb
Public ReadOnly Property TodoItems As ObservableCollection(Of TodoItem)
```

### 4. 명령(ICommand)과 XAML 바인딩

버튼 클릭 같은 동작은 ViewModel의 명령으로 노출합니다. XAML에서는 `{Binding}`으로 연결합니다.

```xml
<Button Content="추가" Command="{Binding AddCommand}" />
<ListBox ItemsSource="{Binding TodoItems}" />
```

실제 WPF 프로젝트에서는 `<UseWPF>true</UseWPF>`를 설정해야 하며, ICommand는 WPF 어셈블리에 포함되어 있습니다.

## 실행

```bash
dotnet run
```

## 정리

- MVVM은 Model/View/ViewModel을 분리해 테스트 가능성과 유지보수를 높입니다.
- `INotifyPropertyChanged`는 단일 속성, `ObservableCollection`은 컬렉션의 변화를 알립니다.
- ViewModel은 View를 참조하지 않으므로 단위 테스트가 쉽습니다.
- 메모리 예제로 바인딩의 이벤트 흐름을 그대로 관찰할 수 있습니다.

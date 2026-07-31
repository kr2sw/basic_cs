# 27: WinForms 고급 — 사용자 컨트롤, 데이터 바인딩

## 소개

Windows Forms에서 자주 사용하는 고급 기법인 데이터 바인딩(BindingList/BindingSource)과 사용자 컨트롤(UserControl) 개념을 다룹니다. 실제 폼/WPF 어셈블리는 없으므로, 바인딩이 동작하는 이벤트 흐름을 메모리로 재현합니다.

## 주요 개념

### 1. BindingList(Of T) — 양방향 컬렉션 바인딩

`List(Of T)`가 아니라 `BindingList(Of T)`를 DataGridView의 `DataSource`에 지정하면 목록의 추가/제거가 그리드에 실시간 반영됩니다. `ListChanged` 이벤트가 알림의 원천입니다.

```vb
Dim people As New BindingList(Of Person)()
grid.DataSource = people
```

### 2. INotifyPropertyChanged — 셀 값 갱신

행 내부의 속성이 바뀌어도 그리드가 갱신되려면 항목 클래스가 `INotifyPropertyChanged`를 구현해야 합니다. BindingList가 이 이벤트를 전파합니다.

```vb
Public Class Person
    Implements INotifyPropertyChanged

    Public Property Age As Integer
        Get
            Return _age
        End Get
        Set(value As Integer)
            _age = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Age)))
        End Set
    End Property
End Class
```

### 3. BindingSource — 필터/정렬/현재 위치

폼과 데이터 사이의 브리지 역할을 합니다. `Filter`, `Sort`, `Current` 속성으로 데이터를 다룹니다.

```vb
Dim source As New BindingSource()
source.DataSource = people
source.Filter = "Age >= 30"
```

### 4. 사용자 컨트롤(UserControl)

여러 기본 컨트롤을 조합해 재사용 가능한 컨트롤을 만듭니다. 도구 상자에 나타나 폼에서 끌어다 쓸 수 있고, 자체 이벤트를 정의할 수 있습니다.

```vb
Public Class NumericLabel
    Inherits UserControl
    ' 라벨 + 텍스트박스 조합, 자체 ValueChanged 이벤트 정의
End Class
```

## 실행

```bash
dotnet run
```

## 정리

- `BindingList(Of T)` + `INotifyPropertyChanged`가 양방향 바인딩의 핵심입니다.
- `BindingSource`는 필터/정렬/현재 위치 관리를 담당합니다.
- `UserControl`로 재사용 가능한 컨트롤을 만들 수 있습니다.
- 메모리 예제로 그리드가 받는 이벤트를 그대로 확인할 수 있습니다.

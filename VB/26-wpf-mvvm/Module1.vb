Imports System
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Linq
Imports System.Runtime.CompilerServices

Module Program
    Sub Main()
        ' WPF + VB MVVM 패턴의 핵심을 메모리로 재현
        ' 실제 XAML 뷰는 별도 파일에서 정의 (README 참고)

        ' 1. ViewModel 생성 (WPF에서 View의 DataContext로 지정됨)
        Dim vm As New TodoViewModel()
        vm.PropertyChanged += Sub(sender, e) Console.WriteLine($"  [View] 속성 변경: {e.PropertyName}")

        ' 2. View가 컬렉션 변화를 구독 (ListBox 등이 하는 일)
        vm.TodoItems.CollectionChanged += Sub(sender, e) Console.WriteLine($"  [View] 컬렉션 변경: {e.Action}")

        ' 3. 사용자 동작 (버튼 클릭에 해당)
        Console.WriteLine("=== 할일 추가 ===")
        vm.AddTodo("리포트 작성")
        vm.AddTodo("회의 준비")
        vm.AddTodo("이메일 답장")

        ' 4. 항목 상태 토글 → INotifyPropertyChanged 발생 → UI 갱신
        Console.WriteLine()
        Console.WriteLine("=== 항목 완료 처리 ===")
        vm.TodoItems(0).IsDone = True

        ' 5. 완료 항목 제거
        Console.WriteLine()
        Console.WriteLine("=== 완료 항목 제거 ===")
        vm.RemoveCompleted()

        Console.WriteLine()
        Console.WriteLine($"남은 할일: {vm.RemainingCount}개")
        For Each item In vm.TodoItems
            Console.WriteLine($"  [{(If(item.IsDone, "X", " "))}] {item.Text}")
        Next
    End Sub
End Module

' 모든 ViewModel의 기반: 속성 변경 알림
Public Class BindableBase
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub

    Protected Sub SetProperty(Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional name As String = Nothing)
        If Equals(storage, value) Then Return
        storage = value
        OnPropertyChanged(name)
    End Sub
End Class

' Model 겸 ViewModel 항목
Public Class TodoItem
    Inherits BindableBase

    Private _text As String
    Private _isDone As Boolean

    Public Property Text As String
        Get
            Return _text
        End Get
        Set(value As String)
            SetProperty(_text, value)
        End Set
    End Property

    Public Property IsDone As Boolean
        Get
            Return _isDone
        End Get
        Set(value As Boolean)
            SetProperty(_isDone, value)
        End Set
    End Property
End Class

' ViewModel: View가 바인딩하는 대상
Public Class TodoViewModel
    Inherits BindableBase

    Public ReadOnly Property TodoItems As ObservableCollection(Of TodoItem)

    Public Sub New()
        TodoItems = New ObservableCollection(Of TodoItem)()
    End Sub

    ' WPF에서는 ICommand로 버튼과 연결: Button Command="{Binding AddCommand}"
    Public Sub AddTodo(text As String)
        TodoItems.Add(New TodoItem() With {.Text = text})
        OnPropertyChanged(NameOf(RemainingCount))
    End Sub

    Public Sub RemoveCompleted()
        Dim done = TodoItems.Where(Function(i) i.IsDone).ToList()
        For Each item In done
            TodoItems.Remove(item)
        Next
        OnPropertyChanged(NameOf(RemainingCount))
    End Sub

    Public ReadOnly Property RemainingCount As Integer
        Get
            Return TodoItems.Count(Function(i) Not i.IsDone)
        End Get
    End Property
End Class

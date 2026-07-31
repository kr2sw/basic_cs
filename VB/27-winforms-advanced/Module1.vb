Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Runtime.CompilerServices

Module Program
    Sub Main()
        Console.WriteLine("=== 1. BindingList + INotifyPropertyChanged ===")

        ' DataGridView.DataSource로 쓰는 대표 컬렉션
        Dim people As New BindingList(Of Person)()
        people.Add(New Person() With {.Name = "홍길동", .Age = 30})
        people.Add(New Person() With {.Name = "김영희", .Age = 25})

        ' DataGridView가 리슨하는 것과 같은 ListChanged 이벤트를 구독
        AddHandler people.ListChanged, Sub(sender, e) Console.WriteLine($"  [Grid] 항목 변경: {e.ListChangedType}")

        Console.WriteLine()
        Console.WriteLine("=== 행 추가 ===")
        people.Add(New Person() With {.Name = "이철수", .Age = 41})

        Console.WriteLine()
        Console.WriteLine("=== 셀 값 수정 (INotifyPropertyChanged) ===")
        people(0).Age = 31

        Console.WriteLine()
        Console.WriteLine("=== 2. BindingSource 개념 (필터) ===")
        Dim source As New MemoryBindingSource(people)
        source.Filter = Function(p) p.Age >= 30
        For Each p As Person In source.CurrentItems
            Console.WriteLine($"  필터 결과: {p.Name} ({p.Age}세)")
        Next

        Console.WriteLine()
        Console.WriteLine("=== 3. 사용자 컨트롤 개념 (자체 이벤트) ===")
        Dim label As New FakeLabel("버튼")
        AddHandler label.Clicked, Sub() Console.WriteLine("  [UserControl] 클릭 이벤트 발생")
        label.PerformClick()
    End Sub
End Module

' DataGridView 셀 바인딩 대상 (INotifyPropertyChanged 필수)
Public Class Person
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _name As String
    Private _age As Integer

    Public Property Name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
            RaisePropertyChanged()
        End Set
    End Property

    Public Property Age As Integer
        Get
            Return _age
        End Get
        Set(value As Integer)
            If _age = value Then Return
            _age = value
            RaisePropertyChanged()
        End Set
    End Property

    Private Sub RaisePropertyChanged(<CallerMemberName> Optional name As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub
End Class

' BindingSource의 Filter/Current 역할을 하는 메모리 구현
Public Class MemoryBindingSource
    Private ReadOnly _list As BindingList(Of Person)

    Public Sub New(list As BindingList(Of Person))
        _list = list
    End Sub

    Public Property Filter As Func(Of Person, Boolean)

    Public ReadOnly Property CurrentItems As IEnumerable(Of Person)
        Get
            If Filter Is Nothing Then Return _list
            Return _list.Where(Filter)
        End Get
    End Property
End Class

' 사용자 컨트롤 시뮬레이션: 텍스트 + 자체 이벤트를 가진 컨트롤
Public Class FakeLabel
    Public Event Clicked()

    Public ReadOnly Property Text As String

    Public Sub New(text As String)
        Me.Text = text
    End Sub

    Public Sub PerformClick()
        RaiseEvent Clicked()
    End Sub
End Class

Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main()
        Console.WriteLine("=== 1. 제네릭 클래스 심화 (다중 제약) ===")

        Dim repo As New Repository(Of Product)()
        repo.Add(New Product(1, "노트북", 1500000))
        repo.Add(New Product(2, "마우스", 20000))
        repo.Add(New Product(3, "키보드", 45000))

        Console.WriteLine($"보관 개수: {repo.Count}")
        For Each p In repo.FindBy(Function(p) p.Price >= 100000)
            Console.WriteLine($"  고가 상품: {p.Name} ({p.Price:N0}원)")
        Next

        Try
            repo.GetById(99)
        Catch ex As Repository(Of Product).NotFoundException
            Console.WriteLine($"중첩 예외 타입 사용: {ex.Message}")
        End Try

        Console.WriteLine()
        Console.WriteLine("=== 2. 중첩 타입 (Nested Type) ===")

        Dim matrix As New Matrix(2, 2)
        matrix.SetCell(0, 0, 1.0)
        matrix.SetCell(1, 1, 2.5)
        Dim cell = matrix.GetCell(1, 1)
        Console.WriteLine($"Matrix.Cell: row={cell.Row}, col={cell.Col}, value={cell.Value}")

        Console.WriteLine()
        Console.WriteLine("=== 3. 이벤트 상속 ===")

        Dim dog As New Dog("멍멍이")
        AddHandler dog.StateChanged, Sub(prev, curr) Console.WriteLine($"  [{dog.Name}] 상태: {prev} → {curr}")
        dog.SetName("바둑이")

        Dim bird As New Bird("짹짹이")
        AddHandler bird.StateChanged, Sub(prev, curr) Console.WriteLine($"  [{bird.Name}] 상태: {prev} → {curr}")
        bird.SetName("새새")
    End Sub
End Module

' --- 제네릭 클래스 심화: 다중 제약 + 중첩 예외 타입 ---
Public Interface IEntity
    ReadOnly Property Id As Integer
End Interface

Public Class Repository(Of T As {Class, IEntity})
    ' 중첩 타입: 이 저장소 전용 예외
    Public Class NotFoundException
        Inherits Exception

        Public Sub New(id As Integer)
            MyBase.New($"ID {id} 항목을 찾을 수 없습니다.")
        End Sub
    End Class

    Private ReadOnly _items As New List(Of T)

    Public Sub Add(item As T)
        _items.Add(item)
    End Sub

    Public ReadOnly Property Count As Integer
        Get
            Return _items.Count
        End Get
    End Property

    Public Function GetById(id As Integer) As T
        For Each item In _items
            If item.Id = id Then Return item
        Next
        Throw New NotFoundException(id)
    End Function

    Public Function FindBy(predicate As Func(Of T, Boolean)) As IEnumerable(Of T)
        Return _items.Where(predicate)
    End Function
End Class

Public Class Product
    Implements IEntity

    Public Property Id As Integer
    Public Property Name As String
    Public Property Price As Decimal

    Public Sub New(id As Integer, name As String, price As Decimal)
        Me.Id = id
        Me.Name = name
        Me.Price = price
    End Sub
End Class

' --- 중첩 타입 ---
Public Class Matrix
    Public Structure Cell
        Public Row As Integer
        Public Col As Integer
        Public Value As Double

        Public Sub New(row As Integer, col As Integer, value As Double)
            Me.Row = row
            Me.Col = col
            Me.Value = value
        End Sub
    End Structure

    Private _data(,) As Double

    Public Sub New(rows As Integer, cols As Integer)
        ReDim _data(rows - 1, cols - 1)
    End Sub

    Public Sub SetCell(row As Integer, col As Integer, value As Double)
        _data(row, col) = value
    End Sub

    Public Function GetCell(row As Integer, col As Integer) As Cell
        Return New Cell(row, col, _data(row, col))
    End Function
End Class

' --- 이벤트 상속 패턴 ---
Public Class Animal
    Public Event StateChanged(previous As String, current As String)

    Protected Overridable Sub OnStateChanged(prev As String, curr As String)
        RaiseEvent StateChanged(prev, curr)
    End Sub
End Class

Public Class Dog
    Inherits Animal

    Private _name As String

    Public Sub New(name As String)
        _name = name
    End Sub

    Public ReadOnly Property Name As String
        Get
            Return _name
        End Get
    End Property

    Public Sub SetName(value As String)
        Dim prev = _name
        _name = value
        ' 기반 클래스의 보호된 발생 메서드를 상속받아 사용
        OnStateChanged(prev, value)
    End Sub

    Protected Overrides Sub OnStateChanged(prev As String, curr As String)
        Console.WriteLine("  (개 이름 변경 감지)")
        MyBase.OnStateChanged(prev, curr)
    End Sub
End Class

Public Class Bird
    Inherits Animal

    Private _name As String

    Public Sub New(name As String)
        _name = name
    End Sub

    Public ReadOnly Property Name As String
        Get
            Return _name
        End Get
    End Property

    Public Sub SetName(value As String)
        Dim prev = _name
        _name = value
        OnStateChanged(prev, value)
    End Sub
End Class

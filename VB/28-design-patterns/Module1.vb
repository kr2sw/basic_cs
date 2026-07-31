Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main()
        ' 1. 싱글턴 (Singleton)
        Console.WriteLine("=== Singleton ===")
        Logger.Instance.Log("앱 시작")
        Logger.Instance.Log("사용자 로그인")
        Console.WriteLine($"  동일 인스턴스? {ReferenceEquals(Logger.Instance, Logger.Instance)}")

        ' 2. 팩토리 (Factory)
        Console.WriteLine()
        Console.WriteLine("=== Factory ===")
        Dim shapes As New List(Of IShape)()
        shapes.Add(ShapeFactory.Create("circle", 5))
        shapes.Add(ShapeFactory.Create("square", 4))
        For Each s In shapes
            Console.WriteLine($"  {s.Name} 넓이: {s.Area():F2}")
        Next

        ' 3. 전략 (Strategy)
        Console.WriteLine()
        Console.WriteLine("=== Strategy ===")
        Dim data() As Integer = {5, 1, 4, 2, 3}
        Dim sorter As New Sorter()
        sorter.SetStrategy(New BubbleSortStrategy())
        Console.WriteLine($"  버블: {String.Join(",", sorter.Sort(data))}")
        sorter.SetStrategy(New QuickSortStrategy())
        Console.WriteLine($"  퀵:   {String.Join(",", sorter.Sort(data))}")

        ' 4. 옵저버 (Observer) — 이벤트 기반
        Console.WriteLine()
        Console.WriteLine("=== Observer ===")
        Dim stock As New Stock("삼성전자", 70000)
        AddHandler stock.PriceChanged, AddressOf PrintAlert1
        AddHandler stock.PriceChanged, AddressOf PrintAlert2
        stock.UpdatePrice(72000)
        stock.UpdatePrice(68500)
    End Sub

    Private Sub PrintAlert1(price As Decimal)
        Console.WriteLine($"  [구독자1] 가격 알림: {price:N0}원")
    End Sub

    Private Sub PrintAlert2(price As Decimal)
        If price < 70000 Then Console.WriteLine($"  [구독자2] 하락 경고: {price:N0}원")
    End Sub
End Module

' --- Singleton ---
Public NotInheritable Class Logger
    Private Shared ReadOnly _lock As New Object()
    Private Shared _instance As Logger

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property Instance As Logger
        Get
            SyncLock _lock
                If _instance Is Nothing Then
                    _instance = New Logger()
                End If
            End SyncLock
            Return _instance
        End Get
    End Property

    Public Sub Log(message As String)
        Console.WriteLine($"  [LOG] {DateTime.Now:HH:mm:ss} {message}")
    End Sub
End Class

' --- Factory ---
Public Interface IShape
    ReadOnly Property Name As String
    Function Area() As Double
End Interface

Public Class Circle
    Implements IShape
    Private ReadOnly _radius As Double

    Public Sub New(radius As Double)
        _radius = radius
    End Sub

    Public ReadOnly Property Name As String Implements IShape.Name
        Get
            Return "원"
        End Get
    End Property

    Public Function Area() As Double Implements IShape.Area
        Return Math.PI * _radius * _radius
    End Function
End Class

Public Class Square
    Implements IShape
    Private ReadOnly _side As Double

    Public Sub New(side As Double)
        _side = side
    End Sub

    Public ReadOnly Property Name As String Implements IShape.Name
        Get
            Return "사각형"
        End Get
    End Property

    Public Function Area() As Double Implements IShape.Area
        Return _side * _side
    End Function
End Class

Public Class ShapeFactory
    Public Shared Function Create(kind As String, Optional param As Double = 0) As IShape
        Select Case kind
            Case "circle" : Return New Circle(param)
            Case "square" : Return New Square(param)
            Case Else : Throw New ArgumentException($"알 수 없는 도형: {kind}")
        End Select
    End Function
End Class

' --- Strategy ---
Public Interface ISortStrategy
    Function Sort(data As Integer()) As Integer()
End Interface

Public Class BubbleSortStrategy
    Implements ISortStrategy

    Public Function Sort(data As Integer()) As Integer() Implements ISortStrategy.Sort
        Dim arr = data.ToArray()
        For i = 0 To arr.Length - 2
            For j = 0 To arr.Length - i - 2
                If arr(j) > arr(j + 1) Then
                    Dim t = arr(j) : arr(j) = arr(j + 1) : arr(j + 1) = t
                End If
            Next
        Next
        Return arr
    End Function
End Class

Public Class QuickSortStrategy
    Implements ISortStrategy

    Public Function Sort(data As Integer()) As Integer() Implements ISortStrategy.Sort
        Dim arr = data.ToArray()
        Array.Sort(arr)
        Return arr
    End Function
End Class

Public Class Sorter
    Private _strategy As ISortStrategy

    Public Sub SetStrategy(strategy As ISortStrategy)
        _strategy = strategy
    End Sub

    Public Function Sort(data As Integer()) As Integer()
        If _strategy Is Nothing Then Throw New InvalidOperationException("전략이 설정되지 않았습니다")
        Return _strategy.Sort(data)
    End Function
End Class

' --- Observer ---
Public Class Stock
    Public Event PriceChanged(price As Decimal)

    Public ReadOnly Property Symbol As String
    Private _price As Decimal

    Public Sub New(symbol As String, price As Decimal)
        Me.Symbol = symbol
        Me._price = price
    End Sub

    Public Sub UpdatePrice(newPrice As Decimal)
        _price = newPrice
        RaiseEvent PriceChanged(_price)
    End Sub
End Class

Imports System
Imports System.Collections.Generic

Module Program
    Sub Main()
        ' 제네릭 클래스
        Dim stack As New Stack(Of Integer)
        stack.Push(10)
        stack.Push(20)
        stack.Push(30)
        Console.WriteLine($"Pop: {stack.Pop()}")
        Console.WriteLine($"Peek: {stack.Peek()}")

        Dim strStack As New Stack(Of String)
        strStack.Push("Hello")
        strStack.Push("World")

        ' 제네릭 메서드
        Console.WriteLine($"Max(3, 7) = {GetMax(3, 7)}")
        Console.WriteLine($"Max(3.14, 2.71) = {GetMax(3.14, 2.71)}")

        ' Nullable
        Dim nullableInt As Integer? = Nothing
        Console.WriteLine($"HasValue: {nullableInt.HasValue}")
        nullableInt = 42
        Console.WriteLine($"Value: {nullableInt.Value}")

        ' 구조체 제약 예제
        Dim result = Add(3, 5)
        Console.WriteLine($"Add (generic): {result}")
    End Sub

    ' 제네릭 메서드 (IComparable 제약)
    Function GetMax(Of T As IComparable)(a As T, b As T) As T
        If a.CompareTo(b) > 0 Then Return a
        Return b
    End Function

    ' 구조체 제약
    Function Add(Of T As Structure)(a As T, b As T) As T
        Dim ad As Decimal = CDec(a) + CDec(b)
        Return CType(Convert.ChangeType(ad, GetType(T)), T)
    End Function
End Module

' 제네릭 클래스
Public Class Stack(Of T)
    Private items As New List(Of T)

    Public Sub Push(item As T)
        items.Add(item)
    End Sub

    Public Function Pop() As T
        If items.Count = 0 Then
            Throw New InvalidOperationException("Stack is empty")
        End If
        Dim lastIndex = items.Count - 1
        Dim item = items(lastIndex)
        items.RemoveAt(lastIndex)
        Return item
    End Function

    Public Function Peek() As T
        If items.Count = 0 Then
            Throw New InvalidOperationException("Stack is empty")
        End If
        Return items(items.Count - 1)
    End Function

    Public ReadOnly Property Count As Integer
        Get
            Return items.Count
        End Get
    End Property
End Class

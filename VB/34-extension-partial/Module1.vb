Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices

Module Program
    Sub Main()
        Console.WriteLine("=== 1. 확장 메서드 ===")
        Dim text = "Hello Visual Basic .NET"
        Console.WriteLine($"  단어 수: {text.WordCount()}")
        Console.WriteLine($"  줄임말: {text.Shorten(5)}")

        Dim n = 42
        Console.WriteLine($"  {n}은(는) 짝수? {n.IsEven()}")

        Dim today = Date.Today
        Console.WriteLine($"  오늘은 주말? {today.IsWeekend()}")

        Dim items = {"apple", "banana", "kiwi"}
        Console.WriteLine($"  가장 긴 단어: {items.Longest()}")

        Console.WriteLine()
        Console.WriteLine("=== 2. Partial 클래스 (여러 블록으로 분할) ===")
        Dim person As New Person()
        person.Name = "홍길동"
        person.Age = 30
        Console.WriteLine($"  {person.Name}: {person.Age}세")
        Console.WriteLine($"  {person.SayHello()}")

        Console.WriteLine()
        Console.WriteLine("=== 3. Partial 메서드 ===")
        person.Name = "김영희"
        person.Name = "이철수"
    End Sub
End Module

' --- 확장 메서드는 Module에 정의 ---
Module StringExtensions
    <Extension>
    Public Function WordCount(str As String) As Integer
        Return str.Split(" "c, StringSplitOptions.RemoveEmptyEntries).Length
    End Function

    <Extension>
    Public Function Shorten(str As String, maxLength As Integer) As String
        If str.Length <= maxLength Then Return str
        Return str.Substring(0, maxLength) & "..."
    End Function
End Module

Module IntegerExtensions
    <Extension>
    Public Function IsEven(n As Integer) As Boolean
        Return n Mod 2 = 0
    End Function
End Module

Module DateTimeExtensions
    <Extension>
    Public Function IsWeekend(d As Date) As Boolean
        Return d.DayOfWeek = DayOfWeek.Saturday OrElse d.DayOfWeek = DayOfWeek.Sunday
    End Function
End Module

Module EnumerableExtensions
    <Extension>
    Public Function Longest(Of T)(source As IEnumerable(Of T), Optional selector As Func(Of T, String) = Nothing) As T
        Dim result As T = Nothing
        Dim maxLen = -1
        For Each item In source
            Dim str = If(selector Is Nothing, item.ToString(), selector(item))
            If str.Length > maxLen Then
                maxLen = str.Length
                result = item
            End If
        Next
        Return result
    End Function
End Module

' --- Partial 클래스: 선언/구현 분할 (실제로는 파일 단위) ---
Partial Public Class Person
    Private _name As String

    Public Property Name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
            OnNameChanged()
        End Set
    End Property

    Public Property Age As Integer

    Public Sub New()
        Console.WriteLine("  [Person 생성자] 객체 생성됨")
    End Sub
End Class

Partial Public Class Person
    Public Function SayHello() As String
        Return $"안녕하세요, 저는 {Name}입니다"
    End Function
End Class

' Partial 메서드: 선언 부분
Partial Public Class Person
    Private Partial Sub OnNameChanged()
End Class

' Partial 메서드: 구현 부분
Partial Public Class Person
    Private Sub OnNameChanged()
        Console.WriteLine($"  [Partial 메서드] 이름 변경됨: {Name}")
    End Sub
End Class

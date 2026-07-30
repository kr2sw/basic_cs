Imports System

Module Program
    Sub Main()
        Console.WriteLine($"Add(3, 5) = {Add(3, 5)}")
        Console.WriteLine($"Factorial(10) = {Factorial(10)}")

        ' ByRef 테스트
        Dim x As Integer = 10
        Increment(x)
        Console.WriteLine($"Increment 후: {x}")

        ' Optional 매개변수
        Greet("Alice")
        Greet("Bob", "안녕")

        ' ParamArray
        Console.WriteLine($"Sum(1..5) = {Sum(1, 2, 3, 4, 5)}")

        ' Function 리턴값
        Dim result = Divide(10, 3)
        Console.WriteLine($"10 / 3 = {result.Quotient}, 나머지 = {result.Remainder}")

        ' 지역 함수 (Visual Basic 15.3+)
        Dim addTen As Func(Of Integer, Integer) = Function(n) n + 10
        Console.WriteLine($"addTen(20) = {addTen(20)}")
    End Sub

    ' Function
    Function Add(a As Integer, b As Integer) As Integer
        Return a + b
    End Function

    ' 재귀 Function
    Function Factorial(n As Integer) As Integer
        If n <= 1 Then Return 1
        Return n * Factorial(n - 1)
    End Function

    ' Sub (반환값 없음)
    Sub Increment(ByRef value As Integer)
        value += 1
    End Sub

    ' Optional 매개변수
    Sub Greet(name As String, Optional greeting As String = "Hello")
        Console.WriteLine($"{greeting}, {name}!")
    End Sub

    ' ParamArray (가변 인자)
    Function Sum(ParamArray values As Integer()) As Integer
        Dim total As Integer = 0
        For Each v In values
            total += v
        Next
        Return total
    End Function

    ' 튜플 반환
    Function Divide(a As Integer, b As Integer) As (Quotient As Integer, Remainder As Integer)
        Return (a \ b, a Mod b)
    End Function
End Module

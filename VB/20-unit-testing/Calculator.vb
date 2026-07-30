Public Class Calculator
    Public Function Add(a As Integer, b As Integer) As Integer
        Return a + b
    End Function

    Public Function Subtract(a As Integer, b As Integer) As Integer
        Return a - b
    End Function

    Public Function Multiply(a As Integer, b As Integer) As Integer
        Return a * b
    End Function

    Public Function Divide(a As Integer, b As Integer) As Integer
        If b = 0 Then
            Throw New DivideByZeroException("0으로 나눌 수 없습니다.")
        End If
        Return a \ b
    End Function

    Public Function IsPrime(n As Integer) As Boolean
        If n < 2 Then Return False
        For i As Integer = 2 To CInt(Math.Sqrt(n))
            If n Mod i = 0 Then Return False
        Next
        Return True
    End Function
End Class

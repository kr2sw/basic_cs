Imports System
Imports System.IO

Module Program
    Sub Main()
        ' 기본 예외 처리
        Try
            Dim a As Integer = 10
            Dim b As Integer = 0
            Dim result = a \ b
            Console.WriteLine(result)
        Catch ex As DivideByZeroException
            Console.WriteLine($"0으로 나눌 수 없습니다: {ex.Message}")
        Catch ex As Exception
            Console.WriteLine($"일반 예외: {ex.Message}")
        Finally
            Console.WriteLine("Finally 블록은 항상 실행됩니다.")
        End Try

        ' Catch When
        Try
            Console.Write("숫자를 입력하세요: ")
            Dim input As String = Console.ReadLine()
            Dim num As Integer = Integer.Parse(input)
            Console.WriteLine($"입력값: {num}")
        Catch ex As FormatException When input.Length = 0
            Console.WriteLine("입력값이 비어있습니다.")
        Catch ex As FormatException
            Console.WriteLine("올바른 숫자 형식이 아닙니다.")
        Catch ex As OverflowException
            Console.WriteLine("숫자가 너무 크거나 작습니다.")
        End Try

        ' Throw 예제
        Try
            ValidateAge(-5)
        Catch ex As ArgumentException
            Console.WriteLine($"유효성 검사 오류: {ex.Message}")
        End Try

        ' 사용자 정의 예외
        Try
            Throw New CustomException("사용자 정의 예외 발생!", 1001)
        Catch ex As CustomException
            Console.WriteLine($"[{ex.ErrorCode}] {ex.Message}")
        End Try

        ' Using 문 (자원 자동 해제)
        Using writer As New StreamWriter("test.txt")
            writer.WriteLine("Hello, Using!")
        End Using
        Console.WriteLine("파일이 자동으로 닫혔습니다.")
    End Sub

    Sub ValidateAge(age As Integer)
        If age < 0 Then
            Throw New ArgumentException("나이는 음수일 수 없습니다.", NameOf(age))
        End If
    End Sub
End Module

' 사용자 정의 예외
Public Class CustomException
    Inherits Exception

    Public Property ErrorCode As Integer

    Public Sub New(message As String, errorCode As Integer)
        MyBase.New(message)
        Me.ErrorCode = errorCode
    End Sub
End Class

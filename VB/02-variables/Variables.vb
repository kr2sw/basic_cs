Option Explicit On
Option Strict On

Imports System

Module Program
    Sub Main()
        ' 변수 선언
        Dim number As Integer = 42
        Dim price As Double = 19.99
        Dim name As String = "Alice"
        Dim isActive As Boolean = True
        Dim initial As Char = "A"c
        Dim today As Date = Date.Now

        ' 타입 추론 (Option Infer On이 기본)
        Dim inferred = "타입 추론됨"

        ' 상수
        Const PI As Double = 3.1415926535
        Const MAX_SIZE As Integer = 100

        ' 출력
        Console.WriteLine($"정수: {number}")
        Console.WriteLine($"실수: {price:F2}")
        Console.WriteLine($"문자열: {name}")
        Console.WriteLine($"부울: {isActive}")
        Console.WriteLine($"문자: {initial}")
        Console.WriteLine($"날짜: {today}")
        Console.WriteLine($"PI = {PI}")

        ' 형변환
        Dim dbl As Double = 123.456
        Dim intVal As Integer = CInt(dbl)
        Console.WriteLine($"CInt({dbl}) = {intVal}")

        ' 문자열 → 숫자
        Dim strNum As String = "456"
        Dim parsed As Integer = Integer.Parse(strNum)
        Console.WriteLine($"Integer.Parse(""{strNum}"") = {parsed}")

        ' TryParse
        Dim input As String = "789"
        Dim result As Integer
        If Integer.TryParse(input, result) Then
            Console.WriteLine($"TryParse 성공: {result}")
        End If

        ' Nothing (null)
        Dim nullable As String = Nothing
        Console.WriteLine($"Nothing: '{nullable}'")
    End Sub
End Module

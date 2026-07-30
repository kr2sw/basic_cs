Imports System

' 한 줄 주석
'
' 여러 줄 주석은
' 각 줄마다 ' 사용

Module Program
    Sub Main()
        ' 기본 출력
        Console.WriteLine("Hello, World!")
        Console.WriteLine("VB.NET 공부 시작!")

        ' 여러 값 출력
        Console.WriteLine("이름: {0}, 나이: {1}", "홍길동", 25)

        ' 문자열 보간 (Visual Basic 14+)
        Dim name As String = "VB.NET"
        Console.WriteLine($"환영합니다, {name}님!")

        ' 입력
        Console.Write("이름을 입력하세요: ")
        Dim input As String = Console.ReadLine()
        Console.WriteLine($"안녕하세요, {input}님!")

        ' 키 입력 대기
        Console.WriteLine("아무 키나 누르세요...")
        Console.ReadKey()
    End Sub
End Module

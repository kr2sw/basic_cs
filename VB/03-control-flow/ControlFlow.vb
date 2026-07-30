Imports System

Module Program
    Sub Main()
        ' If/ElseIf/Else
        Dim score As Integer = 85
        If score >= 90 Then
            Console.WriteLine("등급: A")
        ElseIf score >= 80 Then
            Console.WriteLine("등급: B")
        ElseIf score >= 70 Then
            Console.WriteLine("등급: C")
        Else
            Console.WriteLine("등급: F")
        End If

        ' 한 줄 If
        Dim age As Integer = 20
        Dim status As String = If(age >= 18, "성인", "미성년자")
        Console.WriteLine($"상태: {status}")

        ' Select Case
        Dim day As Integer = 3
        Select Case day
            Case 1
                Console.WriteLine("월요일")
            Case 2
                Console.WriteLine("화요일")
            Case 3
                Console.WriteLine("수요일")
            Case 4, 5
                Console.WriteLine("목/금요일")
            Case 6 To 7
                Console.WriteLine("주말")
            Case Else
                Console.WriteLine("잘못된 요일")
        End Select

        ' For/Next
        Console.Write("For: ")
        For i As Integer = 1 To 5
            Console.Write($"{i} ")
        Next
        Console.WriteLine()

        ' Step
        Console.Write("짝수: ")
        For i As Integer = 0 To 10 Step 2
            Console.Write($"{i} ")
        Next
        Console.WriteLine()

        ' For Each
        Dim fruits As String() = {"사과", "바나나", "체리"}
        Console.Write("과일: ")
        For Each fruit As String In fruits
            Console.Write($"{fruit} ")
        Next
        Console.WriteLine()

        ' Do While
        Dim count As Integer = 0
        Do While count < 3
            Console.WriteLine($"Do While: {count}")
            count += 1
        Loop

        ' Do Until
        Dim x As Integer = 0
        Do
            Console.WriteLine($"Do Until: {x}")
            x += 1
        Loop Until x >= 3
    End Sub
End Module

Imports System
Imports System.Text

Module Program
    Sub Main()
        ' 문자열 연결
        Dim hello As String = "Hello"
        Dim world As String = "World"
        Console.WriteLine(hello & ", " & world & "!")
        Console.WriteLine($"{hello}, {world}!")

        ' 문자열 함수
        Dim text As String = "  Visual Basic .NET  "
        Console.WriteLine($"원본: '{text}'")
        Console.WriteLine($"길이: {text.Length}")
        Console.WriteLine($"Trim: '{text.Trim()}'")
        Console.WriteLine($"ToUpper: '{text.Trim().ToUpper()}'")
        Console.WriteLine($"ToLower: '{text.Trim().ToLower()}'")

        ' 검색
        Dim sentence As String = "The quick brown fox jumps over the lazy dog"
        Console.WriteLine($"'fox' 위치: {sentence.IndexOf("fox")}")
        Console.WriteLine($"Contains 'dog': {sentence.Contains("dog")}")
        Console.WriteLine($"StartsWith 'The': {sentence.StartsWith("The")}")

        ' 추출
        Console.WriteLine($"Substring(4, 5): '{sentence.Substring(4, 5)}'")

        ' 분할
        Dim parts As String() = sentence.Split(" "c)
        Console.WriteLine($"단어 개수: {parts.Length}")
        For Each p In parts
            Console.Write($"[{p}] ")
        Next
        Console.WriteLine()

        ' 교체
        Dim replaced As String = sentence.Replace("fox", "cat")
        Console.WriteLine($"Replace: {replaced}")

        ' StringBuilder
        Dim sb As New StringBuilder()
        sb.Append("Hello")
        sb.Append(", ")
        sb.Append("World")
        sb.Append("!")
        Console.WriteLine($"StringBuilder: {sb.ToString()}")

        ' Format
        Console.WriteLine(String.Format("{0} + {1} = {2}", 10, 20, 10 + 20))

        ' String.Join
        Dim words As String() = {"Apple", "Banana", "Cherry"}
        Console.WriteLine($"Join: {String.Join(", ", words)}")
    End Sub
End Module

Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main()
        ' 배열
        Dim numbers() As Integer = {10, 20, 30, 40, 50}
        Console.WriteLine($"배열[0]: {numbers(0)}")
        Console.WriteLine($"배열 길이: {numbers.Length}")

        ' 배열 순회
        For i As Integer = 0 To numbers.Length - 1
            Console.Write($"{numbers(i)} ")
        Next
        Console.WriteLine()

        ' ReDim Preserve (배열 크기 변경)
        ReDim Preserve numbers(7)
        numbers(5) = 60
        numbers(6) = 70
        numbers(7) = 80
        Console.WriteLine($"ReDim 후 길이: {numbers.Length}")

        ' 다차원 배열
        Dim matrix(,) As Integer = {{1, 2}, {3, 4}, {5, 6}}
        Console.WriteLine($"matrix(1,0) = {matrix(1, 0)}")

        ' List(Of T)
        Dim names As New List(Of String) From {"Alice", "Bob", "Charlie"}
        names.Add("Diana")
        names.Remove("Bob")
        Console.WriteLine($"List contains 'Alice': {names.Contains("Alice")}")
        For Each name In names
            Console.Write($"{name} ")
        Next
        Console.WriteLine()

        ' Dictionary(Of K, V)
        Dim scores As New Dictionary(Of String, Integer) From {
            {"Alice", 95},
            {"Bob", 87}
        }
        scores("Charlie") = 92
        Console.WriteLine($"Alice score: {scores("Alice")}")

        For Each kvp In scores
            Console.WriteLine($"{kvp.Key}: {kvp.Value}")
        Next

        ' LINQ (Imports System.Linq 필요)
        Dim evenNumbers = numbers.Where(Function(n) n > 0 AndAlso n Mod 2 = 0).ToArray()
        Console.WriteLine($"짝수 개수: {evenNumbers.Length}")
    End Sub
End Module

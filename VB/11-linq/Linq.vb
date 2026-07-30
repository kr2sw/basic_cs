Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main()
        Dim numbers As Integer() = {5, 2, 8, 1, 9, 3, 6, 4, 7}

        ' 쿼리 구문
        Dim evens = From n In numbers
                    Where n Mod 2 = 0
                    Order By n
                    Select n

        Console.Write("짝수 정렬: ")
        For Each n In evens
            Console.Write($"{n} ")
        Next
        Console.WriteLine()

        ' 메서드 구문
        Dim bigNumbers = numbers.Where(Function(n) n > 5)
                                .OrderByDescending(Function(n) n)
                                .Select(Function(n) n * 10)

        Console.Write("5 초과 * 10: ")
        For Each n In bigNumbers
            Console.Write($"{n} ")
        Next
        Console.WriteLine()

        ' 객체 컬렉션 LINQ
        Dim people As New List(Of Person) From {
            New Person("Alice", 25, "Seoul"),
            New Person("Bob", 17, "Busan"),
            New Person("Charlie", 30, "Seoul"),
            New Person("Diana", 22, "Incheon")
        }

        ' 서울 거주 성인만
        Dim query = From p In people
                    Where p.Age >= 19 AndAlso p.City = "Seoul"
                    Order By p.Age Descending
                    Select p

        Console.WriteLine("서울 거주 성인:")
        For Each p In query
            Console.WriteLine($"  {p.Name}, {p.Age}세")
        Next

        ' 그룹화
        Dim grouped = From p In people
                      Group By p.City Into Count()

        Console.WriteLine("도시별 인원:")
        For Each g In grouped
            Console.WriteLine($"  {g.City}: {g.Count}명")
        Next

        ' Aggregate
        Dim sum = Aggregate n In numbers Into Sum(n)
        Dim avg = Aggregate n In numbers Into Average(n)
        Dim max = Aggregate n In numbers Into Max(n)
        Console.WriteLine($"Sum={sum}, Avg={avg:F1}, Max={max}")
    End Sub
End Module

Public Class Person
    Public Property Name As String
    Public Property Age As Integer
    Public Property City As String

    Public Sub New(name As String, age As Integer, city As String)
        Me.Name = name
        Me.Age = age
        Me.City = city
    End Sub
End Class

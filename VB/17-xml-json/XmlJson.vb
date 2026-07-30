Imports System
Imports System.Text.Json
Imports System.Xml.Linq

Module Program
    Sub Main()
        ' === XML ===
        ' XML 생성
        Dim xml As New XElement("books",
            New XElement("book",
                New XAttribute("id", 1),
                New XElement("title", "VB.NET Programming"),
                New XElement("author", "John Doe"),
                New XElement("price", 29.99)
            ),
            New XElement("book",
                New XAttribute("id", 2),
                New XElement("title", "LINQ in Action"),
                New XElement("author", "Jane Smith"),
                New XElement("price", 39.99)
            )
        )

        Console.WriteLine("--- XML 출력 ---")
        Console.WriteLine(xml)

        ' XML 쿼리
        Dim titles = From b In xml.Elements("book")
                     Select b.Element("title").Value

        Console.WriteLine("책 제목:")
        For Each t In titles
            Console.WriteLine($"  - {t}")
        Next

        ' XML 파일 저장
        xml.Save("books.xml")
        Console.WriteLine("XML 파일 저장 완료")

        ' XML 파일 로드
        Dim loaded = XDocument.Load("books.xml")
        Console.WriteLine($"로드된 XML 루트: {loaded.Root.Name}")

        ' === JSON ===
        Dim books As New List(Of Book) From {
            New Book() With {.Id = 1, .Title = "VB.NET", .Author = "Alice", .Price = 29.99},
            New Book() With {.Id = 2, .Title = "C#", .Author = "Bob", .Price = 34.99}
        }

        ' 직렬화
        Dim json = JsonSerializer.Serialize(books, New JsonSerializerOptions() With {
            .WriteIndented = True
        })
        Console.WriteLine("--- JSON 출력 ---")
        Console.WriteLine(json)

        ' 역직렬화
        Dim jsonStr = "[{""Id"":1,""Title"":""Book A"",""Author"":""Author A"",""Price"":19.99}]"
        Dim deserialized = JsonSerializer.Deserialize(Of List(Of Book))(jsonStr)
        For Each b In deserialized
            Console.WriteLine($"  {b.Title} by {b.Author}")
        Next
    End Sub
End Module

Public Class Book
    Public Property Id As Integer
    Public Property Title As String
    Public Property Author As String
    Public Property Price As Decimal
End Class

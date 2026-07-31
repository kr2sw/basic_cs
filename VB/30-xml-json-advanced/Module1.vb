Imports System
Imports System.Linq
Imports System.Text.Json
Imports System.Text.Json.Nodes
Imports System.Xml.Linq

Module Program
    Sub Main()
        ' === 1. LINQ to XML ===
        Console.WriteLine("=== 1. LINQ to XML 생성/조회 ===")

        Dim doc As New XDocument(
            New XElement("inventory",
                New XElement("item",
                    New XAttribute("id", 1),
                    New XElement("name", "노트북"),
                    New XElement("price", 1500000)),
                New XElement("item",
                    New XAttribute("id", 2),
                    New XElement("name", "마우스"),
                    New XElement("price", 20000))))

        ' LINQ 쿼리로 XML 요소 검색
        Dim expensive = From el In doc.Descendants("item")
                        Where CInt(el.Element("price").Value) > 50000
                        Select el.Element("name").Value
        Console.WriteLine($"고가 상품: {String.Join(", ", expensive)}")

        ' XML 문자열로 변환
        Console.WriteLine()
        Console.WriteLine(doc.ToString())

        ' === 2. System.Text.Json 직렬화/역직렬화 ===
        Console.WriteLine()
        Console.WriteLine("=== 2. System.Text.Json ===")

        Dim options As New JsonSerializerOptions() With {.WriteIndented = True}
        Dim person As New PersonDto() With {
            .Name = "홍길동",
            .Age = 30,
            .Tags = {"개발", "교육"}
        }
        Dim json = JsonSerializer.Serialize(person, options)
        Console.WriteLine(json)

        Dim back = JsonSerializer.Deserialize(Of PersonDto)(json)
        Console.WriteLine($"역직렬화: {back.Name}, {back.Age}세, 태그 {back.Tags.Length}개")

        ' === 3. JsonNode 동적 DOM 접근 ===
        Console.WriteLine()
        Console.WriteLine("=== 3. JsonNode (DOM) ===")
        Dim node = JsonNode.Parse(json)
        node("age") = 31
        node("extra") = "동적 추가"
        Console.WriteLine(node.ToJsonString(New JsonSerializerOptions() With {.WriteIndented = True}))
    End Sub
End Module

Public Class PersonDto
    Public Property Name As String
    Public Property Age As Integer
    Public Property Tags As String()
End Class

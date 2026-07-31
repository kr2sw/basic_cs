Imports System
Imports System.Collections.Generic
Imports System.Linq

Module Program
    Sub Main()
        ' EF Core Code First의 핵심: POCO 엔티티 + DbContext
        ' EF는 NuGet 패키지가 필요하므로, 여기서는 메모리 구현으로 개념 학습

        Using db As New AppDbContext()
            ' DbSet에 해당하는 메모리 컬렉션에 데이터 추가
            db.Categories.Add(New Category() With {.Id = 1, .Name = "전자제품"})
            db.Categories.Add(New Category() With {.Id = 2, .Name = "문구"})
            db.SaveChanges()

            db.Products.Add(New Product() With {.Id = 1, .Name = "노트북", .Price = 1500000, .CategoryId = 1})
            db.Products.Add(New Product() With {.Id = 2, .Name = "모니터", .Price = 350000, .CategoryId = 1})
            db.Products.Add(New Product() With {.Id = 3, .Name = "노트", .Price = 3000, .CategoryId = 2})
            db.SaveChanges()

            ' LINQ로 DbSet처럼 쿼리
            ' 실제 EF: IQueryable → SQL 변환 (SELECT * FROM Products WHERE Price < 100000)
            Console.WriteLine("=== 저가 상품 (< 100,000원) ===")
            Dim cheap = db.Products.Where(Function(p) p.Price < 100000)
            For Each p In cheap
                Console.WriteLine($"  {p.Name} {p.Price:N0}원")
            Next

            ' Join 개념 (EF에서는 Navigation Property로 더 간단하게)
            Console.WriteLine()
            Console.WriteLine("=== 상품 - 카테고리 조인 ===")
            Dim query = From p In db.Products
                        Join c In db.Categories On p.CategoryId Equals c.Id
                        Select New With {.Product = p.Name, .Category = c.Name}

            For Each q In query
                Console.WriteLine($"  {q.Product} → {q.Category}")
            Next

            ' EF가 변환하는 SQL의 개념
            ' SELECT p.Name, c.Name FROM Products p JOIN Categories c ON p.CategoryId = c.Id
        End Using
        Console.WriteLine()
        Console.WriteLine("완료")
    End Sub
End Module

' POCO 엔티티 클래스 (Code First)
Public Class Category
    Public Property Id As Integer
    Public Property Name As String
End Class

Public Class Product
    Public Property Id As Integer
    Public Property Name As String
    Public Property Price As Decimal
    Public Property CategoryId As Integer
End Class

' DbContext 역할을 하는 메모리 구현
Public Class AppDbContext
    Implements IDisposable

    ' 실제 EF: Public Property Products As DbSet(Of Product)
    Public ReadOnly Property Products As List(Of Product)
    Public ReadOnly Property Categories As List(Of Category)

    Public Sub New()
        Products = New List(Of Product)()
        Categories = New List(Of Category)()
    End Sub

    Public Sub SaveChanges()
        ' 실제 EF: 변경 추적(ChangeTracker)을 거쳐 DB에 반영
        Console.WriteLine($"[SaveChanges] 저장됨 (상품 {Products.Count}개, 카테고리 {Categories.Count}개)")
    End Sub

    Public Sub Dispose()
        ' 실제 EF: 연결 해제 및 리소스 정리
        Console.WriteLine("[Dispose] DbContext 리소스 정리")
    End Sub
End Class

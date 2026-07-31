Imports System
Imports System.Linq
Imports System.Linq.Expressions

Module Program
    Sub Main()
        Console.WriteLine("=== 1. GroupJoin: 그룹 조인 ===")
        Dim customers = {
            New Customer(1, "홍길동"),
            New Customer(2, "김영희"),
            New Customer(3, "이철수")
        }
        Dim orders = {
            New Order(101, 1, 25000),
            New Order(102, 1, 3000),
            New Order(103, 2, 15000),
            New Order(104, 3, 99000),
            New Order(105, 3, 1200)
        }

        ' 쿼리 구문의 Group Join: 모든 고객 + 그룹으로 묶인 주문
        Dim grouped = From c In customers
                      Group Join o In orders On c.Id Equals o.CustomerId Into OrderList = Group
                      Select New With {.Customer = c, .OrderList = OrderList}

        For Each g In grouped
            Console.WriteLine($"{g.Customer.Name} 주문 {g.OrderList.Count()}건")
            For Each o In g.OrderList
                Console.WriteLine($"  - #{o.Id} {o.Amount:N0}원")
            Next
        Next

        Console.WriteLine()
        Console.WriteLine("=== 2. Aggregate: 누적 집계 ===")
        Dim numbers = {5, 3, 8, 2, 9}
        Dim sum = numbers.Aggregate(0, Function(acc, n) acc + n)
        Dim product = numbers.Aggregate(Function(acc, n) acc * n)
        Dim max = numbers.Aggregate(Function(acc, n) If(n > acc, n, acc))
        Console.WriteLine($"Sum={sum}, Product={product}, Max={max}")

        Console.WriteLine()
        Console.WriteLine("=== 3. Expression (식 트리) 개념 ===")
        ' 람다를 실행 코드가 아니라 데이터(트리)로 보관
        Dim expr As Expression(Of Func(Of Integer, Integer)) = Function(x) x * 2 + 1
        Console.WriteLine($"식 트리 표현: {expr}")

        Dim compiled = expr.Compile()
        Console.WriteLine($"컴파일 후 f(5) = {compiled(5)}")

        ' IQueryable은 이 식 트리를 SQL 등으로 변환하는 원리
        ' (예: EF Core에서 .Where(Function(x) x.Price > 100) → WHERE [Price] > 100)

        Console.WriteLine()
        Console.WriteLine("=== 4. Zip / SelectMany ===")
        Dim names = {"가", "나", "다"}
        Dim values = {1, 2, 3}
        Dim zipped = names.Zip(values, Function(n, v) $"{n}={v}")
        Console.WriteLine($"Zip: {String.Join(", ", zipped)}")

        Dim sentences = {"Hello World", "VB.NET LINQ"}
        Dim words = sentences.SelectMany(Function(s) s.Split(" "c))
        Console.WriteLine($"SelectMany 단어: {String.Join(", ", words)}")

        Console.WriteLine()
        Console.WriteLine("=== 5. ToLookup ===")
        Dim byCustomer = orders.ToLookup(Function(o) o.CustomerId)
        Console.WriteLine($"고객 2의 주문: {byCustomer(2).Count()}건")
        Console.WriteLine($"고객 3의 주문: {byCustomer(3).Count()}건")
    End Sub
End Module

Public Class Customer
    Public Property Id As Integer
    Public Property Name As String

    Public Sub New(id As Integer, name As String)
        Me.Id = id
        Me.Name = name
    End Sub
End Class

Public Class Order
    Public Property Id As Integer
    Public Property CustomerId As Integer
    Public Property Amount As Decimal

    Public Sub New(id As Integer, customerId As Integer, amount As Decimal)
        Me.Id = id
        Me.CustomerId = customerId
        Me.Amount = amount
    End Sub
End Class

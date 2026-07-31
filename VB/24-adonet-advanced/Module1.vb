Imports System
Imports System.Data

Module Program
    Sub Main()
        Console.WriteLine("=== 1. DataSet / DataTable / DataRelation ===")

        ' DataSet: 여러 DataTable + 관계를 담는 메모리상 데이터베이스
        Dim ds As New DataSet("Shop")

        ' Customers 테이블
        Dim customers = ds.Tables.Add("Customers")
        customers.Columns.Add("Id", GetType(Integer))
        customers.Columns.Add("Name", GetType(String))
        customers.PrimaryKey = {customers.Columns("Id")}

        ' Orders 테이블
        Dim orders = ds.Tables.Add("Orders")
        orders.Columns.Add("Id", GetType(Integer))
        orders.Columns.Add("CustomerId", GetType(Integer))
        orders.Columns.Add("Amount", GetType(Decimal))

        customers.Rows.Add(1, "홍길동")
        customers.Rows.Add(2, "김영희")
        orders.Rows.Add(101, 1, 25000)
        orders.Rows.Add(102, 1, 3000)
        orders.Rows.Add(103, 2, 15000)

        ' DataRelation: 부모-자식 관계 정의
        Dim rel = ds.Relations.Add("CustOrders", customers.Columns("Id"), orders.Columns("CustomerId"))

        ' 부모 행에서 자식 행 찾기 (관계 탐색)
        Dim row = customers.Rows.Find(1)
        Dim childRows = row.GetChildRows(rel)
        Console.WriteLine($"{row("Name")}의 주문: {childRows.Length}건")

        Console.WriteLine()
        Console.WriteLine("=== 2. DataAdapter Fill/Update (메모리) ===")

        ' 실제로는 SqlDataAdapter가 SELECT로 Fill, 변경 행으로 Update
        Dim adapter As New MemoryDataAdapter(customers)
        Dim clientTable = customers.Clone()          ' 스키마(열 구조) 복사
        adapter.Fill(clientTable)                    ' DB(원본) → 클라이언트 DataTable
        Console.WriteLine($"클라이언트 테이블: {clientTable.Rows.Count}행")

        clientTable.Rows.Add(3, "이철수")             ' 클라이언트에서 데이터 추가
        adapter.Update(clientTable)                  ' 변경 → DB(원본) 반영
        Console.WriteLine($"Update 반영 후 Customers 행 수: {customers.Rows.Count}")

        Console.WriteLine()
        Console.WriteLine("=== 3. DataView 필터/정렬 ===")
        Dim view As New DataView(customers)
        view.Sort = "Name DESC"
        For Each r As DataRowView In view
            Console.WriteLine($"  {r("Id")} - {r("Name")}")
        Next

        Console.WriteLine()
        Console.WriteLine("=== 4. 트랜잭션 시뮬레이션 (Commit/Rollback) ===")
        Dim account As New BankAccount(10000)
        Try
            account.TransferTo(3000)                 ' 정상 이체 → Commit
            account.TransferTo(999999)               ' 잔액 부족 → Rollback
        Catch ex As Exception
            Console.WriteLine($"오류: {ex.Message}")
        End Try
        Console.WriteLine($"최종 잔액: {account.Balance:C0}")
    End Sub
End Module

' DataAdapter의 Fill/Update 역할을 하는 메모리 구현
Public Class MemoryDataAdapter
    Private _source As DataTable

    Public Sub New(source As DataTable)
        _source = source
    End Sub

    Public Sub Fill(target As DataTable)
        ' 실제로는: SELECT ... (SqlDataAdapter.Fill)
        For Each row In _source.Rows
            target.ImportRow(row)
        Next
    End Sub

    Public Sub Update(changes As DataTable)
        ' 실제로는: 변경 상태에 따라 INSERT/UPDATE/DELETE (SqlDataAdapter.Update)
        For Each row In changes.Rows
            If row.RowState = DataRowState.Added Then
                _source.Rows.Add(row.ItemArray)
            End If
        Next
        changes.AcceptChanges()
    End Sub
End Class

' 트랜잭션(원자성)을 메모리로 재현한 계좌 클래스
Public Class BankAccount
    Public Property Balance As Decimal

    Public Sub New(balance As Decimal)
        Me.Balance = balance
    End Sub

    Public Sub TransferTo(amount As Decimal)
        Console.WriteLine($"  이체 시도: {amount:C0}")
        ' 실제 트랜잭션: BEGIN TRAN → UPDATE ... → COMMIT / ROLLBACK
        If amount > Balance Then
            Throw New InvalidOperationException("잔액 부족, 트랜잭션 롤백")
        End If
        Balance -= amount
        Console.WriteLine($"  이체 성공 (잔액: {Balance:C0})")
    End Sub
End Class

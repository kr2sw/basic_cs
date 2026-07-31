# 24: ADO.NET 고급 — DataAdapter, DataSet, 트랜잭션

## 소개

기초 챕터의 SqlConnection/SqlCommand/SqlDataReader를 넘어, 연결을 유지하지 않는 오프라인(disconnected) 방식인 `DataSet`/`DataAdapter`와 트랜잭션 개념을 다룹니다. 데이터베이스 서버가 없는 환경에서도 학습할 수 있도록 메모리 구현으로 재현했습니다.

## 주요 개념

### 1. DataSet / DataTable / DataRelation

`DataSet`은 여러 `DataTable`과 그 사이의 관계를 담는 **메모리상의 데이터베이스**입니다. 실제 `SqlDataAdapter.Fill`을 거치면 DB 조회 결과가 여기에 채워집니다.

```vb
Dim ds As New DataSet("Shop")
Dim customers = ds.Tables.Add("Customers")
customers.Columns.Add("Id", GetType(Integer))
customers.PrimaryKey = {customers.Columns("Id")}

Dim rel = ds.Relations.Add("CustOrders", customers.Columns("Id"), orders.Columns("CustomerId"))
Dim childRows = row.GetChildRows(rel)   ' 부모 → 자식 행 탐색
```

### 2. DataAdapter — Fill/Update 사이클

DataAdapter는 SQL과 DataTable을 연결하는 브리지입니다.

- `Fill()`: DB(SELECT) → DataTable
- `Update()`: DataTable의 변경(추가/수정/삭제) → DB(INSERT/UPDATE/DELETE)

```vb
Dim adapter As New SqlDataAdapter("SELECT * FROM Customers", conn)
adapter.Fill(table)          ' DB → 메모리
Dim builder As New SqlCommandBuilder(adapter)
adapter.Update(table)        ' 메모리 변경 → DB
```

실제 SQL Server 코드는 NuGet 패키지(`Microsoft.Data.SqlClient`)가 필요하므로, 예제에서는 같은 동작을 메모리로 재현한 `MemoryDataAdapter`를 사용합니다.

### 3. DataView — 필터/정렬 뷰

같은 데이터를 목적에 따라 다른 정렬/필터로 보여줍니다. WinForms/WPF 바인딩에서도 사용됩니다.

```vb
Dim view As New DataView(customers)
view.Sort = "Name DESC"
For Each r As DataRowView In view
    ...
Next
```

### 4. 트랜잭션 — 원자성 보장

여러 쿼리를 하나의 작업으로 묶어 모두 성공하거나 모두 실패(롤백)하도록 보장합니다.

```vb
Using tx As SqlTransaction = conn.BeginTransaction()
    Try
        ' INSERT ... (1)
        ' UPDATE ... (2)
        tx.Commit()          ' 전부 반영
    Catch
        tx.Rollback()        ' 전부 되돌림
    End Try
End Using
```

예제에서는 잔액 부족 시 롤백되는 계좌 이체를 메모리로 시뮬레이션합니다.

## 실행

```bash
dotnet run
```

## 정리

- `DataSet`은 오프라인 데이터 처리의 핵심 구조입니다.
- `DataAdapter.Fill/Update`가 DB와 메모리 데이터를 동기화합니다.
- `DataView`로 데이터를 필터/정렬합니다.
- 트랜잭션은 Commit/Rollback으로 원자성을 보장합니다.

# 25: EF Core — Code First, DbContext, 마이그레이션

## 소개

EF Core(Entity Framework Core)는 객체(Entity)를 데이터베이스 테이블에 자동으로 매핑하는 ORM입니다. Code First 방식은 클래스 코드를 먼저 작성하고, 마이그레이션으로 데이터베이스 스키마를 만듭니다. EF는 NuGet 패키지가 필요하므로 예제에서는 같은 구조를 메모리로 재현했습니다.

## 주요 개념

### 1. POCO 엔티티 클래스

특별한 상속이나 프레임워크 참조 없이 순수한 속성만 가진 클래스입니다.

```vb
Public Class Product
    Public Property Id As Integer
    Public Property Name As String
    Public Property Price As Decimal
    Public Property CategoryId As Integer
End Class
```

### 2. DbContext — 단위 작업(Unit of Work)

`DbContext`는 엔티티 집합(`DbSet`)과 변경 추적(ChangeTracker), SaveChanges를 담당합니다. EF의 핵심입니다.

```vb
Public Class AppDbContext
    Inherits DbContext

    Public Property Products As DbSet(Of Product)
    Public Property Categories As DbSet(Of Category)

    Public Overrides Sub SaveChanges()
        ' 변경 추적된 엔티티들을 INSERT/UPDATE/DELETE로 변환해 실행
    End Sub
End Class
```

DbContext는 가볍고 일회성으로 사용하므로 `Using` 블록 안에서 생성합니다.

### 3. LINQ → SQL 변환

`IQueryable(Of T)`는 LINQ 식 트리를 SQL로 변환합니다. 클라이언트 메모리가 아니라 **데이터베이스에서** 필터링이 일어납니다.

```vb
Dim cheap = db.Products.Where(Function(p) p.Price < 100000)
' → SELECT * FROM Products WHERE Price < 100000
```

### 4. 마이그레이션

코드 변경을 스키마 변경으로 전파합니다.

```bash
dotnet ef migrations add AddProductTable
dotnet ef database update
```

마이그레이션 파일은 순서대로 기록되어 이력 관리와 롤백이 가능합니다.

### 5. 탐색 속성(Navigation Property)

외래 키 대신 객체 참조로 관계를 표현합니다.

```vb
Public Class Product
    Public Property Category As Category    ' 1:N 관계
End Class
```

## 실행

```bash
dotnet run
```

## 정리

- EF Core는 POCO 엔티티 + DbContext로 객체-관계 매핑을 자동화합니다.
- `DbSet(Of T)` 조회는 SQL로 번역되어 DB에서 실행됩니다.
- 마이그레이션으로 스키마를 버전 관리합니다.
- 메모리 예제는 실제 EF 구조를 그대로 흉내 냅니다.

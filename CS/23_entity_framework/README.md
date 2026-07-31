# 23: EF Core — Entity Framework Core

EF Core는 C# 객체를 데이터베이스 테이블에 매핑해 주는 **ORM(Object-Relational
Mapping)** 입니다. 이 장에서는 ORM 개념과 EF Core의 주요 기능을 익힙니다.
NuGet 없이 동작하도록 **인메모리 리포지토리**로 동일한 패턴을 구현하고,
실제 EF Core 사용법은 주석으로 보여줍니다.

## ORM 개념

ORM은 클래스와 테이블 사이의 불일치를 해소합니다.

- 클래스(엔티티) → 테이블
- 프로퍼티 → 컬럼
- 관계(Navigation Property) → 외래 키

```csharp
public class Product
{
    public int Id { get; set; }        // PK
    public string Name { get; set; }   // 컬럼
    public Category Category { get; set; }  // 관계
}
```

## EF Core 실제 사용법 (주석)

```csharp
// var options = new DbContextOptionsBuilder<AppDbContext>()
//     .UseSqlite("Data Source=app.db").Options;
// var db = new AppDbContext(options);
// db.Products.Add(new Product { Name = "노트북" });
// await db.SaveChangesAsync();
```

이 장에서는 리포지토리 인터페이스로 데이터 접근을 추상화하고,
인메모리 구현으로 실행 흐름을 확인합니다.

## 리포지토리 패턴

- `IRepository<T>` — CRUD 인터페이스
- `InMemoryRepository<T>` — `List<T>` 기반 구현
- 서비스 계층이 리포지토리를 통해 데이터에 접근

## 실행

```bash
dotnet run
```

## 핵심 요약

- EF Core는 엔티티 ↔ 테이블 매핑을 제공하는 ORM입니다.
- DbContext는 작업 단위(Unit of Work)로 변경 사항을 추적합니다.
- 리포지토리 패턴으로 데이터 계층을 교체할 수 있습니다.

# 17: JDBC — 데이터베이스 연결

## JDBC (Java Database Connectivity)

Java에서 데이터베이스에 접근하기 위한 표준 API입니다.

## 주요 인터페이스

| 인터페이스 | 설명 |
|-----------|------|
| `DriverManager` | 드라이버 관리, Connection 생성 |
| `Connection` | DB 연결 |
| `Statement` | SQL 실행 (정적) |
| `PreparedStatement` | SQL 실행 (동적, 파라미터 바인딩, SQL Injection 방지) |
| `ResultSet` | 쿼리 결과 조회 |

## JDBC 사용 단계

1. 드라이버 로드 (`Class.forName()` / 자동 로드)
2. `DriverManager.getConnection()`으로 연결
3. `Statement` 또는 `PreparedStatement` 생성
4. SQL 실행 (`executeQuery()` / `executeUpdate()`)
5. `ResultSet` 처리
6. 자원 반환 (`close()`)

## H2 Database (Embedded)

```xml
<!-- pom.xml -->
<dependency>
    <groupId>com.h2database</groupId>
    <artifactId>h2</artifactId>
    <version>2.2.224</version>
</dependency>
```

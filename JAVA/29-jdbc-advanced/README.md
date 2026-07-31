# 29: JDBC Advanced — JDBC 고급

## 트랜잭션 (Transaction)

여러 SQL 을 하나의 작업 단위로 묶는 기능입니다.

```java
conn.setAutoCommit(false);
try {
    stmt.executeUpdate("UPDATE account SET balance = balance - 500 WHERE id = 1");
    stmt.executeUpdate("UPDATE account SET balance = balance + 500 WHERE id = 2");
    conn.commit();          // 모두 성공해야 커밋
} catch (SQLException e) {
    conn.rollback();        // 하나라도 실패하면 롤백
}
```

- `commit()`, `rollback()`, `setSavepoint("sp1")` / `rollback("sp1")`
- ACID: 원자성, 일관성, 고립성, 지속성

## 배치 (Batch)

여러 SQL 을 한 번에 모아서 실행해 성능을 개선합니다.

```java
PreparedStatement ps = conn.prepareStatement("INSERT INTO t VALUES (?)");
for (int i = 1; i <= 1000; i++) {
    ps.setInt(1, i);
    ps.addBatch();          // 버퍼에 쌓기
}
int[] results = ps.executeBatch();   // 한 번에 실행
```

## DAO / Repository 패턴

DB 접근 코드를 인터페이스 뒤에 숨겨 구조화합니다.

| 패턴 | 특징 |
|------|------|
| DAO | 데이터 접근을 담당하는 객체, 인터페이스+구현 |
| Repository | 도메인 중심 추상화, 컬렉션처럼 다룸 |

```java
public interface UserRepository {
    Optional<User> findById(Long id);
    List<User> findAll();
    User save(User user);
}
```

## 커넥션 풀 (Connection Pool)

- `DriverManager.getConnection` 은 매번 연결을 새로 만들어 비쌈
- HikariCP, Apache DBCP 가 풀을 관리해 연결을 재사용
- Spring Boot 는 기본으로 HikariCP 사용

## 실행

```bash
cd JAVA/29-jdbc-advanced
javac Main.java && java Main
```

> 실제 DB 연결 없이 실행해도 됩니다. JDBC 예제는 드라이버가 없으면
> "연결 없음" 메시지를 출력하고, 인메모리 저장소로 동작합니다.

# 15: Database (PDO) — 데이터베이스

## PDO (PHP Data Objects)

데이터베이스 접근을 위한 PHP 확장입니다.

### 연결

```php
$dsn = 'mysql:host=localhost;dbname=testdb;charset=utf8mb4';
$pdo = new PDO($dsn, $user, $password);
```

### CRUD

- **INSERT**: `INSERT INTO users (name, email) VALUES (?, ?)`
- **SELECT**: `SELECT * FROM users WHERE id = ?`
- **UPDATE**: `UPDATE users SET name = ? WHERE id = ?`
- **DELETE**: `DELETE FROM users WHERE id = ?`

### Prepared Statements

SQL Injection을 방지하기 위해 Prepared Statement 사용을 권장합니다.

### 트랜잭션

```php
$pdo->beginTransaction();
// ... SQL 실행 ...
$pdo->commit();  // 또는 $pdo->rollBack();
```

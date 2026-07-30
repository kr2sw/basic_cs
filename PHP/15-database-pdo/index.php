<?php
// SQLite 인메모리 DB (별도 설치 불필요)
$dsn = 'sqlite::memory:';

try {
    $pdo = new PDO($dsn, null, null, [
        PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
        PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
        PDO::ATTR_EMULATE_PREPARES => false,
    ]);
    echo "DB 연결 성공!\n";

    // 테이블 생성
    $pdo->exec("
        CREATE TABLE users (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            email TEXT UNIQUE,
            age INTEGER
        )
    ");
    echo "테이블 생성 완료\n";

    // INSERT (Prepared Statement)
    $stmt = $pdo->prepare("INSERT INTO users (name, email, age) VALUES (?, ?, ?)");
    
    $users = [
        ['Alice', 'alice@example.com', 25],
        ['Bob', 'bob@example.com', 30],
        ['Charlie', 'charlie@example.com', 35],
    ];

    foreach ($users as $user) {
        $stmt->execute($user);
    }
    echo "데이터 삽입 완료\n";

    // SELECT (모든 컬럼)
    echo "\n=== 전체 사용자 ===\n";
    $stmt = $pdo->query("SELECT * FROM users");
    while ($row = $stmt->fetch()) {
        echo "{$row['id']} | {$row['name']} | {$row['email']} | {$row['age']}\n";
    }

    // SELECT with WHERE
    echo "\n=== 나이 28 초과 ===\n";
    $stmt = $pdo->prepare("SELECT * FROM users WHERE age > ?");
    $stmt->execute([28]);
    foreach ($stmt as $row) {
        echo "{$row['name']} ({$row['age']})\n";
    }

    // UPDATE
    $stmt = $pdo->prepare("UPDATE users SET age = ? WHERE name = ?");
    $stmt->execute([26, 'Alice']);
    echo "\nAlice 나이 업데이트 완료\n";

    // FETCH_CLASS 예제
    echo "\n=== FETCH_CLASS ===\n";
    $stmt = $pdo->prepare("SELECT * FROM users WHERE name = ?");
    $stmt->execute(['Alice']);
    $user = $stmt->fetchObject();
    echo "{$user->name} ({$user->age}세)\n";

    // rowCount
    $stmt = $pdo->query("SELECT COUNT(*) as cnt FROM users");
    $count = $stmt->fetchColumn();
    echo "\n전체 사용자 수: $count\n";

    // 트랜잭션
    echo "\n=== 트랜잭션 ===\n";
    try {
        $pdo->beginTransaction();
        
        $pdo->exec("INSERT INTO users (name, email, age) VALUES ('David', 'david@example.com', 28)");
        $pdo->exec("INSERT INTO users (name, email, age) VALUES ('Eve', 'eve@example.com', 22)");
        
        $pdo->commit();
        echo "트랜잭션 커밋 완료\n";
    } catch (PDOException $e) {
        $pdo->rollBack();
        echo "트랜잭션 롤백: " . $e->getMessage() . "\n";
    }

    // lastInsertId
    $lastId = $pdo->lastInsertId();
    echo "마지막 삽입 ID: $lastId\n";

} catch (PDOException $e) {
    echo "DB 오류: " . $e->getMessage() . "\n";
}

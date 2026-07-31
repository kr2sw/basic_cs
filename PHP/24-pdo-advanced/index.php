<?php
// --- 고급 PDO: 트랜잭션, prepared statement, Repository 패턴 ---
// SQLite 인메모리 DB를 사용하므로 외부 DB가 필요 없습니다.

echo "=== 1. 연결 설정 ===\n";

$pdo = new PDO('sqlite::memory:');
$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
$pdo->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_ASSOC);

$pdo->exec("CREATE TABLE users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    email TEXT NOT NULL UNIQUE
)");

$pdo->exec("CREATE TABLE orders (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    amount REAL NOT NULL
)");

echo "테이블 생성 완료\n\n";

echo "=== 2. Prepared Statement ===\n";

// 위치 기반 (?) 플레이스홀더
$stmt = $pdo->prepare('INSERT INTO users (name, email) VALUES (?, ?)');
$stmt->execute(['Alice', 'alice@example.com']);
$aliceId = (int)$pdo->lastInsertId();

// 이름 기반 (:name) 플레이스홀더
$stmt = $pdo->prepare('INSERT INTO users (name, email) VALUES (:name, :email)');
$stmt->execute(['name' => 'Bob', 'email' => 'bob@example.com']);
$bobId = (int)$pdo->lastInsertId();

echo "Alice(id=$aliceId), Bob(id=$bobId) 삽입 완료\n";

// SQL 인젝션 방어: %는 메타문자 취급
$search = "%";  // 만약 문자열 연결이었다면 위험했을 값
$stmt = $pdo->prepare('SELECT COUNT(*) FROM users WHERE email LIKE ?');
$stmt->execute([$search]);
echo "바인딩된 LIKE 검색 결과: " . (int)$stmt->fetchColumn() . "명\n\n";

echo "=== 3. fetch 모드 ===\n";

$all = $pdo->query('SELECT * FROM users ORDER BY id')->fetchAll();
echo "fetchAll (FETCH_ASSOC):\n";
echo json_encode($all, JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT) . "\n";

$row = $pdo->query('SELECT * FROM users WHERE id = 1')->fetch();
echo "fetch 단일 행: name = {$row['name']}\n";

// fetchColumn: 단일 값
$count = (int)$pdo->query('SELECT COUNT(*) FROM users')->fetchColumn();
echo "fetchColumn: 총 사용자 $count명\n\n";

echo "=== 4. 트랜잭션 ===\n";

// [성공 케이스] 잔액이 충분하면 커밋
$pdo->beginTransaction();
try {
    $stmt = $pdo->prepare('INSERT INTO orders (user_id, amount) VALUES (?, ?)');
    $stmt->execute([$aliceId, 15000]);
    $stmt->execute([$aliceId, 30000]);
    $pdo->commit();
    echo "  주문 2건 커밋 완료\n";
} catch (Throwable $e) {
    $pdo->rollBack();
    echo "  롤백: " . $e->getMessage() . "\n";
}

// [실패 케이스] 잔액 부족 → 전부 롤백
$balance = 10000;
$pdo->beginTransaction();
try {
    $stmt = $pdo->prepare('INSERT INTO orders (user_id, amount) VALUES (?, ?)');
    $stmt->execute([$bobId, 8000]);
    $stmt->execute([$bobId, 9000]);   // 합계가 잔액 초과

    if (9000 + 8000 > $balance) {
        throw new RuntimeException("잔액 부족 (보유 {$balance}원)");
    }
    $pdo->commit();
    echo "  커밋됨 (실행되지 않음)\n";
} catch (Throwable $e) {
    $pdo->rollBack();
    echo "  롤백: " . $e->getMessage() . "\n";
}

$orderCount = (int)$pdo->query('SELECT COUNT(*) FROM orders')->fetchColumn();
echo "  최종 주문 수: $orderCount (성공 케이스 2건만 반영됨)\n\n";

echo "=== 5. Repository 패턴 ===\n";

interface UserRepositoryInterface {
    public function findById(int $id): ?array;
    public function findByEmail(string $email): ?array;
    public function create(string $name, string $email): int;
    public function update(int $id, array $data): bool;
    public function delete(int $id): bool;
    public function all(): array;
}

class PdoUserRepository implements UserRepositoryInterface {
    public function __construct(private PDO $pdo) {}

    public function findById(int $id): ?array {
        $stmt = $this->pdo->prepare('SELECT * FROM users WHERE id = ?');
        $stmt->execute([$id]);
        $row = $stmt->fetch();
        return $row ?: null;
    }

    public function findByEmail(string $email): ?array {
        $stmt = $this->pdo->prepare('SELECT * FROM users WHERE email = ?');
        $stmt->execute([$email]);
        $row = $stmt->fetch();
        return $row ?: null;
    }

    public function create(string $name, string $email): int {
        $stmt = $this->pdo->prepare('INSERT INTO users (name, email) VALUES (?, ?)');
        $stmt->execute([$name, $email]);
        return (int)$this->pdo->lastInsertId();
    }

    public function update(int $id, array $data): bool {
        $stmt = $this->pdo->prepare('UPDATE users SET name = ?, email = ? WHERE id = ?');
        $stmt->execute([$data['name'], $data['email'], $id]);
        return $stmt->rowCount() > 0;
    }

    public function delete(int $id): bool {
        $stmt = $this->pdo->prepare('DELETE FROM users WHERE id = ?');
        $stmt->execute([$id]);
        return $stmt->rowCount() > 0;
    }

    public function all(): array {
        return $this->pdo->query('SELECT * FROM users ORDER BY id')->fetchAll();
    }
}

class UserService {
    public function __construct(private UserRepositoryInterface $users) {}

    public function register(string $name, string $email): array {
        if ($this->users->findByEmail($email) !== null) {
            throw new RuntimeException("이미 등록된 이메일입니다: $email");
        }
        $id = $this->users->create($name, $email);
        return $this->users->findById($id);
    }
}

$repo = new PdoUserRepository($pdo);
$service = new UserService($repo);

$user = $service->register('Carol', 'carol@example.com');
echo "  신규 가입: #{$user['id']} {$user['name']} ({$user['email']})\n";

try {
    $service->register('중복', 'carol@example.com');
} catch (RuntimeException $e) {
    echo "  중복 가입 차단: " . $e->getMessage() . "\n";
}

$repo->update($aliceId, ['name' => 'Alicia', 'email' => 'alicia@example.com']);
echo "  Alice → Alicia 이름 변경\n";

$repo->delete($bobId);
echo "  Bob 계정 삭제\n";

echo "  최종 사용자 목록:\n";
foreach ($repo->all() as $u) {
    echo "    - #{$u['id']} {$u['name']} ({$u['email']})\n";
}

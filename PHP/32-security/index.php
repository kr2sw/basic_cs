<?php
// --- 보안: password_hash, CSRF, XSS, SQL 인젝션 방어 ---

echo "=== 1. 비밀번호 해싱 ===\n";

$hash = password_hash('my-password', PASSWORD_BCRYPT, ['cost' => 12]);
echo "해시: $hash\n";
echo "올바른 비밀번호 검증: " . (password_verify('my-password', $hash) ? '성공' : '실패') . "\n";
echo "틀린 비밀번호 검증: " . (password_verify('wrong', $hash) ? '성공' : '실패') . "\n";
echo "cost 정보: " . password_get_info($hash)['cost'] . "\n";
echo "재해싱 필요 여부: " . (password_needs_rehash($hash, PASSWORD_BCRYPT, ['cost' => 12]) ? 'true' : 'false') . "\n\n";

echo "=== 2. CSRF (Cross-Site Request Forgery) 방어 ===\n";

class Csrf {
    private static string $token = '';

    // 토큰 생성 (세션에 저장됨)
    public static function token(): string {
        if (self::$token === '') {
            self::$token = bin2hex(random_bytes(32));
        }
        return self::$token;
    }

    // 폼용 hidden 필드 생성
    public static function field(): string {
        return '<input type="hidden" name="_token" value="' . self::$token() . '">';
    }

    // 제출된 토큰 검증 — hash_equals로 타이밍 공격 방어
    public static function verify(?string $submitted): bool {
        return $submitted !== null && hash_equals(self::$token(), $submitted);
    }
}

$token = Csrf::token();
echo "발급 토큰: $token\n";
echo "폼 필드: " . Csrf::field() . "\n";
echo "정상 토큰 검증: " . (Csrf::verify($token) ? '통과' : '차단') . "\n";
echo "공격자 토큰 검증: " . (Csrf::verify('attacker-token') ? '통과' : '차단') . "\n";
echo "누락 검증: " . (Csrf::verify(null) ? '통과' : '차단') . "\n\n";

echo "=== 3. XSS (Cross-Site Scripting) 방어 ===\n";

// 공격자가 게시판에 심은 악성 스크립트
$userComment = "<script>alert('해킹!')</script><b>안녕하세요</b>";

echo "취약한 출력 (원본 그대로):\n";
echo "  " . $userComment . "\n";

echo "안전한 출력 (htmlspecialchars):\n";
echo "  " . htmlspecialchars($userComment, ENT_QUOTES, 'UTF-8') . "\n";
echo "  (ENT_QUOTES: 작은/큰 따옴표 모두 인코딩)\n\n";

echo "=== 4. SQL 인젝션 방어 ===\n";

$pdo = new PDO('sqlite::memory:');
$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
$pdo->exec("CREATE TABLE users (id INTEGER PRIMARY KEY AUTOINCREMENT, email TEXT NOT NULL)");
$pdo->exec("INSERT INTO users (email) VALUES ('alice@example.com'), ('bob@example.com')");

// 공격자 입력: ' OR '1'='1  → 모든 행을 반환시키는 주입
$input = "' OR '1'='1";

echo "취약한 코드 (문자열 연결):\n";
$sql = "SELECT * FROM users WHERE email = '$input'";
echo "  실행 SQL: $sql\n";
$rows = $pdo->query($sql)->fetchAll();
echo "  결과: " . count($rows) . "명 반환 (전체 데이터 노출!)\n";

echo "안전한 코드 (prepared statement):\n";
$stmt = $pdo->prepare('SELECT * FROM users WHERE email = ?');
$stmt->execute([$input]);
$rows = $stmt->fetchAll();
echo "  결과: " . count($rows) . "명 반환 (0명이 정상)\n\n";

echo "=== 5. 입력 검증 ===\n";

function validateEmail(string $email): bool {
    return filter_var($email, FILTER_VALIDATE_EMAIL) !== false;
}

function validateInteger(mixed $value): bool {
    return filter_var($value, FILTER_VALIDATE_INT) !== false;
}

echo "  alice@example.com: " . (validateEmail('alice@example.com') ? '유효' : '무효') . "\n";
echo "  not-an-email: " . (validateEmail('not-an-email') ? '유효' : '무효') . "\n";
echo "  '42' (숫자): " . (validateInteger('42') ? '유효' : '무효') . "\n";
echo "  '12abc' (숫자): " . (validateInteger('12abc') ? '유효' : '무효') . "\n";

<?php
// --- 인증: 세션 인증, Sanctum/JWT 토큰 개념 ---

echo "=== 1. 비밀번호 해싱 ===\n";

class Password {
    public static function hash(string $plain, int $cost = 12): string {
        return password_hash($plain, PASSWORD_BCRYPT, ['cost' => $cost]);
    }

    public static function verify(string $plain, string $hash): bool {
        return password_verify($plain, $hash);
    }
}

$hash = Password::hash('secret123');
echo "해시: $hash\n";
echo "올바른 비밀번호: " . (Password::verify('secret123', $hash) ? '통과' : '실패') . "\n";
echo "틀린 비밀번호: " . (Password::verify('wrong', $hash) ? '통과' : '실패') . "\n\n";

echo "=== 2. 세션 인증 ===\n";

// 세션 저장소 시뮬레이션 (실제로는 $_SESSION)
class Session {
    private static array $data = [];

    public static function set(string $key, mixed $value): void {
        self::$data[$key] = $value;
    }

    public static function get(string $key): mixed {
        return self::$data[$key] ?? null;
    }

    public static function forget(string $key): void {
        unset(self::$data[$key]);
    }
}

class UserRepository {
    private static array $users = [
        1 => ['id' => 1, 'email' => 'alice@example.com', 'password' => null, 'name' => 'Alice'],
    ];

    public static function seed(): void {
        self::$users[1]['password'] = Password::hash('password');
    }

    public static function findByEmail(string $email): ?array {
        foreach (self::$users as $user) {
            if ($user['email'] === $email) {
                return $user;
            }
        }
        return null;
    }

    public static function findById(int $id): ?array {
        return self::$users[$id] ?? null;
    }
}

class Auth {
    public static function attempt(string $email, string $password): bool {
        $user = UserRepository::findByEmail($email);
        if ($user && Password::verify($password, $user['password'])) {
            Session::set('user_id', $user['id']);
            return true;
        }
        return false;
    }

    public static function check(): bool {
        return Session::get('user_id') !== null;
    }

    public static function user(): ?array {
        $id = Session::get('user_id');
        return $id !== null ? UserRepository::findById($id) : null;
    }

    public static function logout(): void {
        Session::forget('user_id');
    }
}

UserRepository::seed();

echo "로그인(alice@example.com / password): "
    . (Auth::attempt('alice@example.com', 'password') ? '성공' : '실패') . "\n";
echo "Auth::check(): " . (Auth::check() ? '로그인 상태' : '비로그인') . "\n";
echo "Auth::user(): " . (Auth::user()['name'] ?? '없음') . "\n";
echo "잘못된 비밀번호 시도: "
    . (Auth::attempt('alice@example.com', 'wrong') ? '성공' : '실패') . "\n";
Auth::logout();
echo "로그아웃 후 Auth::check(): " . (Auth::check() ? '로그인 상태' : '비로그인') . "\n\n";

echo "=== 3. API 토큰 (Sanctum) ===\n";

// 개인 액세스 토큰 저장소 시뮬레이션
class ApiTokenStore {
    private static array $tokens = [];

    public static function issue(int $userId, string $name): string {
        $plain = bin2hex(random_bytes(32));
        self::$tokens[] = [
            'user_id' => $userId,
            'name' => $name,
            'hash' => hash('sha256', $plain),   // 원본이 아닌 해시만 저장
            'expires_at' => time() + 3600,
        ];
        return $plain;
    }

    public static function verify(string $plainToken): ?array {
        $hash = hash('sha256', $plainToken);
        foreach (self::$tokens as $token) {
            if (hash_equals($token['hash'], $hash)) {
                if ($token['expires_at'] < time()) {
                    return null;   // 만료
                }
                return UserRepository::findById($token['user_id']);
            }
        }
        return null;
    }
}

$token = ApiTokenStore::issue(1, 'mobile-app');
echo "발급 토큰: ..." . substr($token, -8) . " (모바일 앱)\n";
echo "DB 저장값(해시): " . hash('sha256', $token) . "\n";
$user = ApiTokenStore::verify($token);
echo "토큰 검증 → 사용자: " . ($user['name'] ?? '없음') . "\n";
echo "가짜 토큰 검증: " . var_export(ApiTokenStore::verify('fake-token-123'), true) . "\n\n";

echo "=== 4. JWT (JSON Web Token) ===\n";

function base64UrlEncode(string $data): string {
    return rtrim(strtr(base64_encode($data), '+/', '-_'), '=');
}

function base64UrlDecode(string $data): string {
    return base64_decode(strtr($data, '-_', '+/'));
}

function createJwt(array $payload, string $secret): string {
    $header = base64UrlEncode(json_encode(['alg' => 'HS256', 'typ' => 'JWT']));
    $payloadEncoded = base64UrlEncode(json_encode($payload, JSON_UNESCAPED_UNICODE));
    $signature = hash_hmac('sha256', "$header.$payloadEncoded", $secret, true);
    return "$header.$payloadEncoded." . base64UrlEncode($signature);
}

function verifyJwt(string $token, string $secret): ?array {
    $parts = explode('.', $token);
    if (count($parts) !== 3) {
        return null;
    }
    [$header, $payload, $signature] = $parts;

    // 서명 검증 (변조 여부)
    $expected = base64UrlEncode(hash_hmac('sha256', "$header.$payload", $secret, true));
    if (!hash_equals($expected, $signature)) {
        return null;
    }

    // 만료 검사
    $data = json_decode(base64UrlDecode($payload), true);
    if (isset($data['exp']) && time() > $data['exp']) {
        return null;
    }

    return $data;
}

$secret = 'server-secret-key';
$jwt = createJwt([
    'sub' => 1,
    'name' => 'Alice',
    'role' => 'admin',
    'iat' => time(),
    'exp' => time() + 3600,
]);

echo "JWT: $jwt\n\n";
[$h, $p, $s] = explode('.', $jwt);
echo "헤더(디코딩): " . base64UrlDecode($h) . "\n";
echo "페이로드(디코딩): " . base64UrlDecode($p) . "\n\n";

$decoded = verifyJwt($jwt, $secret);
echo "올바른 토큰 검증 → sub={$decoded['sub']}, name={$decoded['name']}, role={$decoded['role']}\n";
echo "변조된 토큰 검증: " . var_export(verifyJwt($jwt . 'x', $secret), true) . "\n";
echo "다른 서버 키로 검증: " . var_export(verifyJwt($jwt, 'wrong-key'), true) . "\n";
echo "만료된 토큰 검증: " . var_export(verifyJwt(createJwt(['sub' => 1, 'exp' => time() - 10], $secret), $secret), true) . "\n";

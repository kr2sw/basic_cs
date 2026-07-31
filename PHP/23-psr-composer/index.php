<?php
// --- PSR과 Composer: 오토로딩, PSR-4, 버전 제약 ---

// [1] PSR-4 오토로더 시뮬레이션
// 네임스페이스 \App\Models\User  →  src/Models/User.php
echo "=== 1. PSR-4 오토로딩 시뮬레이션 ===\n";

$psr4Map = [
    'App\\' => 'src/',
    'Vendor\\Package\\' => 'vendor/package/src/',
];

function psr4Path(string $class, array $map): ?string {
    foreach ($map as $prefix => $baseDir) {
        if (str_starts_with($class, $prefix)) {
            $relative = substr($class, strlen($prefix));
            return rtrim($baseDir, '/') . '/' . str_replace('\\', '/', $relative) . '.php';
        }
    }
    return null;
}

foreach (['App\\Models\\User', 'App\\Controllers\\HomeController', 'Vendor\\Package\\File'] as $class) {
    echo "  {$class}\n";
    echo "    → " . psr4Path($class, $psr4Map) . "\n";
}
echo "\n";

// [2] 실제 PSR-4 브라켓 네임스페이스 (챕터 19 방식)
namespace App\Models {
    class User {
        public function __construct(
            public string $name,
            public string $email
        ) {}

        public function describe(): string {
            return "User: {$this->name} ({$this->email})";
        }
    }
}

namespace App\Repositories {
    class UserRepository {
        public function findAll(): array {
            return [
                new \App\Models\User('Alice', 'alice@example.com'),
                new \App\Models\User('Bob', 'bob@example.com'),
            ];
        }
    }
}

namespace {
    echo "=== 2. 오토로딩된 클래스 사용 ===\n";
    $repo = new \App\Repositories\UserRepository();
    foreach ($repo->findAll() as $user) {
        echo "  " . $user->describe() . "\n";
    }
    echo "\n";

    // [3] composer.json 자동 생성 형태
    echo "=== 3. composer.json ===\n";
    echo json_encode([
        'name' => 'user/basic-php',
        'description' => 'PHP 중급 과정 예제',
        'type' => 'library',
        'require' => [
            'php' => '>=8.1',
            'monolog/monolog' => '^3.0',
        ],
        'autoload' => [
            'psr-4' => ['App\\' => 'src/'],
        ],
        'scripts' => [
            'test' => 'phpunit',
        ],
    ], JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES);
    echo "\n\n";

    // [4] 버전 제약 계산 시뮬레이션
    echo "=== 4. 버전 제약 (Version Constraints) ===\n";

    function evaluateConstraint(string $constraint, string $version): bool {
        // 예: "^1.2" → >=1.2 <2.0
        $normalized = str_starts_with($version, 'v') ? substr($version, 1) : $version;
        $major = (int)explode('.', $normalized)[0];

        if ($constraint === '*') return true;

        if (str_starts_with($constraint, '^')) {
            $min = substr($constraint, 1);
            $nextMajor = (int)explode('.', $min)[0] + 1;
            return version_compare($normalized, $min, '>=') && $major < $nextMajor;
        }

        if (str_starts_with($constraint, '~')) {
            $min = substr($constraint, 1);
            if (substr_count($min, '.') >= 2) {
                // ~1.2.3 → >=1.2.3 <1.3.0
                $parts = explode('.', $min);
                $nextMinor = $parts[0] . '.' . ((int)$parts[1] + 1);
                return version_compare($normalized, $min, '>=') && version_compare($normalized, $nextMinor, '<');
            }
            // ~1.2 → >=1.2 <2.0
            $nextMajor = (int)explode('.', $min)[0] + 1;
            return version_compare($normalized, $min, '>=') && $major < $nextMajor;
        }

        return version_compare($normalized, $constraint, '==');
    }

    // 실용적인 데모: 몇 가지 간단한 규칙만 수동 검증
    $cases = [
        ['^1.2', '1.9.0', true],
        ['^1.2', '2.0.0', false],
        ['~1.2.3', '1.2.9', true],
        ['~1.2.3', '1.3.0', false],
        ['*', '99.0.0', true],
    ];

    echo str_pad('제약', 14) . str_pad('버전', 12) . str_pad('통과', 8) . "수동\n";
    echo str_repeat('-', 44) . "\n";
    foreach ($cases as [$constraint, $version, $expected]) {
        $pass = evaluateConstraint($constraint, $version);
        $manual = $pass === $expected ? 'O' : 'X';
        printf("%-14s %-12s %-8s %s\n", $constraint, $version, $pass ? 'yes' : 'no', $manual);
    }
    echo "\n";

    // [5] 유의적 버전 (Semantic Versioning)
    echo "=== 5. 유의적 버전 ===\n";
    echo "  라이브러리: my-package 1.4.2\n";
    echo "  MAJOR 1  → 하위 호환 안 되는 변경 (2.x는 새 MAJOR)\n";
    echo "  MINOR 4  → 하위 호환 기능 추가\n";
    echo "  PATCH 2  → 버그 수정\n";
    echo "\n설치 전 패키지 상태 (composer.lock):\n";
    echo "  라이브러리   현재    최신    제약\n";
    echo "  monolog    2.9.2   3.5.0   ^2.0 (2.x 유지)\n";
    echo "  guzzle     7.8.1   7.9.0   ~7.8.1 (패치만)\n";
}

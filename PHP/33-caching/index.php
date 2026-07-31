<?php
// --- 캐싱: OPcache, 파일 캐시, Redis 개념 ---

echo "=== 1. OPcache ===\n";
echo "  PHP 코드를 메모리에 저장해 매 요청의 파싱/컴파일을 생략\n";
echo "  php.ini 설정 예시:\n";
echo "    opcache.enable=1\n";
echo "    opcache.memory_consumption=128\n";
echo "    opcache.max_accelerated_files=10000\n";
echo "    opcache.validate_timestamps=1\n";
echo "    opcache.revalidate_freq=2\n";

if (function_exists('opcache_get_status')) {
    $status = opcache_get_status(false);
    echo "  현재 OPcache: " . ($status ? '활성화됨' : '비활성화됨') . "\n";
} else {
    echo "  (이 CLI 환경에서는 opcache 확장이 없습니다)\n";
}
echo "\n";

echo "=== 2. 파일 캐시 ===\n";

class FileCache {
    private string $dir;
    private int $hits = 0;
    private int $misses = 0;

    public function __construct(?string $dir = null) {
        $this->dir = $dir ?? sys_get_temp_dir() . '/php-cache-demo';
        if (!is_dir($this->dir)) {
            mkdir($this->dir, 0777, true);
        }
    }

    public function getDir(): string {
        return $this->dir;
    }

    private function path(string $key): string {
        return $this->dir . '/' . hash('sha256', $key) . '.cache';
    }

    // TTL 초과 시 만료 처리
    public function set(string $key, mixed $value, int $ttl = 60): bool {
        $data = [
            'expires' => time() + $ttl,
            'value' => serialize($value),
        ];
        return file_put_contents($this->path($key), serialize($data)) !== false;
    }

    public function get(string $key, mixed $default = null): mixed {
        $file = $this->path($key);
        if (!is_file($file)) {
            $this->misses++;
            return $default;
        }

        $data = unserialize((string)file_get_contents($file));
        if ($data['expires'] < time()) {
            unlink($file);   // 만료 → 삭제
            $this->misses++;
            return $default;
        }

        $this->hits++;
        return unserialize($data['value']);
    }

    public function delete(string $key): bool {
        $file = $this->path($key);
        return is_file($file) ? unlink($file) : false;
    }

    public function clear(): void {
        foreach (glob($this->dir . '/*.cache') as $file) {
            unlink($file);
        }
    }

    public function stats(): array {
        return ['hits' => $this->hits, 'misses' => $this->misses];
    }
}

$cache = new FileCache();
$cache->clear();

// 첫 조회는 항상 미스 → DB 조회 후 캐시에 저장
$user = $cache->get('user:1');
if ($user === null) {
    echo "  [MISS] DB에서 조회\n";
    $user = ['id' => 1, 'name' => 'Alice', 'role' => 'admin'];
    $cache->set('user:1', $user, 60);
}

// 이후 조회는 히트
$user = $cache->get('user:1');
echo "  [HIT] 캐시에서 조회: " . json_encode($user, JSON_UNESCAPED_UNICODE) . "\n";
$user = $cache->get('user:1');
echo "  [HIT] 다시 조회\n";
$cache->get('no-such-key');
echo "  [MISS] 없는 키 조회\n";

echo "  통계: " . json_encode($cache->stats()) . "\n";
echo "  캐시 파일 위치: " . $cache->getDir() . "\n\n";

echo "=== 3. Redis 개념 (시뮬레이션) ===\n";

class RedisSim {
    private array $data = [];

    public function set(string $key, mixed $value, int $ttl = 0): void {
        $this->data[$key] = [
            'value' => $value,
            'expires' => $ttl > 0 ? time() + $ttl : null,
        ];
    }

    public function get(string $key): mixed {
        if (!isset($this->data[$key])) {
            return null;
        }
        if ($this->data[$key]['expires'] !== null && $this->data[$key]['expires'] < time()) {
            unset($this->data[$key]);   // 만료 자동 정리
            return null;
        }
        return $this->data[$key]['value'];
    }

    public function exists(string $key): bool {
        return $this->get($key) !== null;
    }

    public function delete(string $key): bool {
        if (isset($this->data[$key])) {
            unset($this->data[$key]);
            return true;
        }
        return false;
    }

    public function increment(string $key): int {
        $value = (int)($this->get($key) ?? 0) + 1;
        $this->set($key, $value);
        return $value;
    }

    public function keys(): array {
        return array_keys($this->data);
    }
}

$redis = new RedisSim();

$redis->set('user:1:name', 'Alice');
$redis->set('session:abc123', ['user_id' => 1], 30);  // 30초 TTL
$redis->increment('counter:visits');
$redis->increment('counter:visits');
$redis->increment('counter:visits');

echo "  GET user:1:name → " . $redis->get('user:1:name') . "\n";
echo "  EXISTS session:abc123 → " . ($redis->exists('session:abc123') ? 'true' : 'false') . "\n";
echo "  GET counter:visits → " . $redis->get('counter:visits') . "\n";
echo "  KEYS → " . implode(', ', $redis->keys()) . "\n";
echo "  DELETE user:1:name → " . ($redis->delete('user:1:name') ? 'true' : 'false') . "\n\n";

echo "=== 4. Cache-Aside 패턴 ===\n";

function getProduct(int $id, RedisSim $redis, array &$db): array {
    // 1) 캐시 먼저 확인
    $cached = $redis->get("product:$id");
    if ($cached !== null) {
        return ['source' => 'cache', 'product' => $cached];
    }

    // 2) 캐시 미스 → DB 조회
    $product = $db[$id] ?? null;

    // 3) 결과를 캐시에 저장 (60초)
    if ($product !== null) {
        $redis->set("product:$id", $product, 60);
    }

    return ['source' => 'db', 'product' => $product];
}

$db = [
    1 => ['id' => 1, 'title' => 'PHP 중급 과정', 'price' => 39000],
    2 => ['id' => 2, 'title' => '디자인 패턴', 'price' => 45000],
];

foreach ([1, 1, 2, 1, 99] as $id) {
    $r = getProduct($id, $redis, $db);
    printf("  product:%-2d 출처: %-6s %s\n",
        $id,
        strtoupper($r['source']),
        $r['product'] ? $r['product']['title'] : '(없음)'
    );
}

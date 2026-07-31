<?php
// --- 성능 최적화: opcache, 지연 로딩, 프로파일링 ---

echo "=== 1. 마이크로벤치마크 (hrtime) ===\n\n";

function benchmark(callable $fn, int $iterations): float {
    $start = hrtime(true);
    for ($i = 0; $i < $iterations; $i++) {
        $fn();
    }
    return (hrtime(true) - $start) / 1e6;   // 밀리초
}

// 문자열 반복 결합 vs 배열 + implode
$concatTime = benchmark(function () {
    $s = '';
    for ($i = 0; $i < 100; $i++) {
        $s .= 'x';
    }
}, 10000);

$implodeTime = benchmark(function () {
    $parts = [];
    for ($i = 0; $i < 100; $i++) {
        $parts[] = 'x';
    }
    $s = implode('', $parts);
}, 10000);

printf("  문자열 결합(.=)   : %8.2f ms\n", $concatTime);
printf("  배열 + implode    : %8.2f ms\n", $implodeTime);

// $arr[] = vs array_push
$bracketTime = benchmark(function () {
    $arr = [];
    for ($i = 0; $i < 1000; $i++) {
        $arr[] = $i;
    }
}, 1000);

$pushTime = benchmark(function () {
    $arr = [];
    for ($i = 0; $i < 1000; $i++) {
        array_push($arr, $i);
    }
}, 1000);

printf("  \$arr[] =  추가    : %8.2f ms\n", $bracketTime);
printf("  array_push 추가   : %8.2f ms\n", $pushTime);
echo "\n";

echo "=== 2. 프로파일러 ===\n\n";

class Profiler {
    private array $marks = [];

    public function mark(string $label): void {
        $this->marks[] = [
            'label' => $label,
            'time' => hrtime(true),
            'memory' => memory_get_usage(),
        ];
    }

    public function report(): void {
        $prevTime = null;
        $prevMem = null;

        echo str_pad('단계', 26) . str_pad('소요시간', 13) . str_pad('메모리', 13) . "Δ메모리\n";
        echo str_repeat('-', 65) . "\n";

        foreach ($this->marks as $mark) {
            $deltaTime = $prevTime !== null ? ($mark['time'] - $prevTime) / 1e6 : 0.0;
            $deltaMem = $prevMem !== null ? $mark['memory'] - $prevMem : 0;

            printf("%-26s %9.3f ms %9.2f KB %+9.2f KB\n",
                $mark['label'],
                $deltaTime,
                $mark['memory'] / 1024,
                $deltaMem / 1024
            );

            $prevTime = $mark['time'];
            $prevMem = $mark['memory'];
        }
    }
}

$profiler = new Profiler();
$profiler->mark('시작');

// 데이터 생성 시뮬레이션
$users = [];
for ($i = 0; $i < 5000; $i++) {
    $users[] = ['id' => $i, 'name' => "User{$i}", 'email' => "user{$i}@example.com"];
}
$profiler->mark('사용자 5천 건 생성');

$filtered = array_filter($users, fn($u) => $u['id'] % 2 === 0);
$profiler->mark('짝수 id 필터링');

usort($users, fn($a, $b) => $b['id'] <=> $a['id']);
$profiler->mark('내림차순 정렬');

$profiler->report();
echo "\n";

echo "=== 3. 지연 로딩 (Lazy Loading) ===\n\n";

class HeavyService {
    private string $data;
    private float $initMs;

    public function __construct() {
        $start = hrtime(true);
        $this->data = str_repeat('X', 100000);   // 무거운 초기화 시뮬레이션
        $this->initMs = (hrtime(true) - $start) / 1e6;
    }

    public function getInitTime(): float {
        return $this->initMs;
    }
}

class App {
    // 사용할 때까지 생성하지 않음
    private ?HeavyService $service = null;

    public function getService(): HeavyService {
        return $this->service ??= new HeavyService();
    }
}

$app = new App();
$before = memory_get_usage();
echo "  App 생성 직후: " . round($before / 1024) . " KB (HeavyService 미생성)\n";

$service = $app->getService();
$after = memory_get_usage();
printf("  getService() 호출 후: %d KB (+%d KB, 초기화 %.3f ms)\n",
    round($after / 1024),
    round(($after - $before) / 1024),
    $service->getInitTime()
);
echo "  같은 인스턴스 재사용: " . ($app->getService() === $service ? 'true' : 'false') . "\n\n";

echo "=== 4. 메모리 관리 ===\n\n";

echo "  PHP 메모리 제한: " . ini_get('memory_limit') . "\n";
$base = memory_get_usage();

$big = [];
for ($i = 0; $i < 10000; $i++) {
    $big[] = ['id' => $i, 'payload' => str_repeat('a', 50)];
}
echo "  1만 건 배열 생성 후: " . round((memory_get_usage() - $base) / 1024) . " KB 증가\n";

unset($big);   // 참조 해제
echo "  unset() 후: " . round((memory_get_usage() - $base) / 1024) . " KB 증가 (거의 회복)\n\n";

echo "=== 5. OPcache (프로덕션 권장 설정) ===\n";
echo "  opcache.enable=1\n";
echo "  opcache.memory_consumption=128\n";
echo "  opcache.max_accelerated_files=10000\n";
echo "  opcache.validate_timestamps=0    (배포 시 캐시 재시작)\n";
echo "  opcache.preload=/var/www/preload.php   (PHP 7.4+ 프리로드)\n";

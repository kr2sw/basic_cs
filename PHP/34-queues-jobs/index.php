<?php
// --- 큐와 잡: 작업 큐 패턴, 워커 시뮬레이션 ---

echo "=== 1. 잡(Job) 정의 ===\n";

interface Job {
    public function handle(): void;
    public function attempts(): int;
    public function retryLimit(): int;
    public function incrementAttempt(): void;
}

abstract class BaseJob implements Job {
    protected int $attempts = 0;
    protected int $limit = 3;

    public function attempts(): int {
        return $this->attempts;
    }

    public function retryLimit(): int {
        return $this->limit;
    }

    public function incrementAttempt(): void {
        $this->attempts++;
    }
}

// 이메일 발송 잡 — "slow" 도메인이면 일시적 실패를 흉내
class SendWelcomeEmailJob extends BaseJob {
    public function __construct(private string $email) {}

    public function handle(): void {
        if (str_contains($this->email, 'slow')) {
            throw new RuntimeException("메일 서버 응답 없음: {$this->email}");
        }
        echo "    {$this->email} → 환영 이메일 전송 완료\n";
    }
}

// 결제 처리 잡
class ProcessPaymentJob extends BaseJob {
    public function __construct(
        private int $orderId,
        private int $amount
    ) {}

    public function handle(): void {
        echo "    주문 #{$this->orderId} 결제 {$this->amount}원 처리 완료\n";
    }
}

echo "잡 클래스 2개 정의 완료\n\n";

echo "=== 2. 큐 (Queue) ===\n\n";

class Queue {
    private array $jobs = [];
    private array $delayed = [];

    // 잡을 큐에 넣음 (프로듀서)
    public function push(Job $job): void {
        $this->jobs[] = $job;
        echo "  [Producer] push: " . $job::class . "\n";
    }

    // 지연 예약 (재시도용)
    public function pushWithDelay(Job $job, int $seconds): void {
        $this->delayed[] = ['job' => $job, 'available_at' => time() + $seconds];
        echo "  [Producer] {$seconds}초 후 재시도 예약: " . $job::class . "\n";
    }

    // 처리할 잡을 하나 꺼냄 (컨슈머)
    public function pop(): ?Job {
        foreach ($this->delayed as $i => $item) {
            if ($item['available_at'] <= time()) {
                $this->jobs[] = $item['job'];
                array_splice($this->delayed, $i, 1);
            }
        }
        return array_shift($this->jobs);
    }

    public function size(): int {
        return count($this->jobs) + count($this->delayed);
    }

    // 데모용: 시간이 흐른 척 (대기 중인 지연 잡을 준비시킴)
    public function simulateTime(int $seconds): void {
        foreach ($this->delayed as &$item) {
            $item['available_at'] -= $seconds;
        }
        unset($item);
    }
}

echo "=== 3. 워커 (Worker) ===\n\n";

class Worker {
    private array $failed = [];

    public function process(Queue $queue, int $maxJobs = 5): void {
        $processed = 0;

        while ($processed < $maxJobs && ($job = $queue->pop()) !== null) {
            $job->incrementAttempt();
            echo "  [Worker] 처리 시작: " . $job::class
                . " (시도 {$job->attempts()}/{$job->retryLimit()})\n";

            try {
                $job->handle();
                echo "  [Worker] 성공 ✔\n";
            } catch (Throwable $e) {
                if ($job->attempts() >= $job->retryLimit()) {
                    echo "  [Worker] 재시도 초과 → 실패 큐 이동: {$e->getMessage()}\n";
                    $this->failed[] = $job;
                } else {
                    $delay = $job->attempts() * 2;   // 지수 백오프: 2, 4, 8...
                    echo "  [Worker] 실패 → {$delay}초 후 재시도: {$e->getMessage()}\n";
                    $queue->pushWithDelay($job, $delay);
                }
            }
            $processed++;
        }
    }

    public function failedJobs(): array {
        return $this->failed;
    }
}

echo "=== 4. 프로듀서-컨슈머 데모 ===\n\n";

$queue = new Queue();
echo "--- 잡 생성 (프로듀서) ---\n";
$queue->push(new SendWelcomeEmailJob('alice@example.com'));
$queue->push(new SendWelcomeEmailJob('slow@example.com'));     // 실패할 잡
$queue->push(new ProcessPaymentJob(123, 99000));
echo "큐 크기: " . $queue->size() . "\n\n";

$worker = new Worker();

$wave = 1;
while ($queue->size() > 0 && $wave <= 5) {
    echo "--- 웨이브 {$wave} ---\n";
    $worker->process($queue, 5);

    if ($queue->size() > 0) {
        echo "  (시간 경과 시뮬레이션: 10초)\n";
        $queue->simulateTime(10);
    }
    echo "큐 크기: " . $queue->size() . "\n\n";
    $wave++;
}

echo "=== 5. 결과 ===\n";
echo "영구 실패(실패 큐) 잡: " . count($worker->failedJobs()) . "개\n";
foreach ($worker->failedJobs() as $job) {
    echo "  - " . $job::class . " (시도 {$job->attempts()}회)\n";
}
echo "최종 큐 크기: " . $queue->size() . "\n";

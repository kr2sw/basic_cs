# 34: 큐와 잡 — 작업 큐 패턴, 워커 시뮬레이션

## 큐의 필요성

이메일 발송, 영상 변환, 알림 같은 **시간이 걸리는 작업**을 요청 응답과 분리합니다.

```
요청 → 잡(Job)을 큐에 push → 즉시 응답
                ↓ (백그라운드)
            워커가 pop → 처리
```

## 잡 (Job)

해야 할 일 하나를 클래스로 표현합니다.

```php
class SendWelcomeEmailJob implements Job {
    public function handle(): void {
        // 이메일 발송 로직
    }
}

Queue::push(new SendWelcomeEmailJob('alice@example.com'));
```

Laravel에서는 `ShouldQueue` 인터페이스를 구현하면 비동기로 처리됩니다.

## 큐 (Queue)

잡을 저장하는 버퍼입니다. Laravel 기본은 `database`, 그 외 `redis`, `sqs` 등을 지원합니다.

## 워커 (Worker)

큐에서 잡을 꺼내 처리하는 프로세스입니다.

```bash
php artisan queue:work        # 워커 실행
php artisan queue:work --tries=3
php artisan queue:failed      # 실패 잡 목록
```

## 재시도와 백오프

실패한 잡은 **시도 횟수를 늘려 재시도**합니다. 실패할수록 대기 시간을 늘리는 것을 지수 백오프(Exponential Backoff)라고 합니다.

```
시도 1 실패 → 2초 후 재시도
시도 2 실패 → 4초 후 재시도
시도 3 초과 → 실패 큐로 이동 (실패 알림)
```

## 실행

```bash
php index.php
```

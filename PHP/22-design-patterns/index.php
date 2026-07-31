<?php
// --- 디자인 패턴: 싱글턴, 팩토리, 전략, 옵저버, 의존성 주입 ---

echo "=== 1. 싱글턴 (Singleton) ===\n";

final class Logger {
    private static ?Logger $instance = null;
    private array $logs = [];

    private function __construct() {}

    public static function getInstance(): Logger {
        return self::$instance ??= new self();
    }

    public function log(string $message): void {
        $this->logs[] = '[' . date('H:i:s') . '] ' . $message;
    }

    public function getLogs(): array {
        return $this->logs;
    }

    // 복제/역직렬화로 인스턴스가 늘어나는 것을 차단
    private function __clone() {}
    public function __wakeup() {
        throw new Exception('싱글턴은 역직렬화할 수 없습니다.');
    }
}

$a = Logger::getInstance();
$b = Logger::getInstance();
$a->log('앱 시작');
$b->log('로그인 성공');

echo "동일 인스턴스? " . ($a === $b ? 'true' : 'false') . "\n";
echo "로그 수: " . count($a->getLogs()) . "\n";
foreach ($a->getLogs() as $log) {
    echo "  $log\n";
}
echo "\n";

echo "=== 2. 팩토리 (Factory) ===\n";

interface PaymentGateway {
    public function pay(float $amount): string;
}

class CardGateway implements PaymentGateway {
    public function pay(float $amount): string {
        return "카드 결제: {$amount}원 처리됨";
    }
}

class KakaoGateway implements PaymentGateway {
    public function pay(float $amount): string {
        return "카카오페이 결제: {$amount}원 처리됨";
    }
}

class NaverGateway implements PaymentGateway {
    public function pay(float $amount): string {
        return "네이버페이 결제: {$amount}원 처리됨";
    }
}

class PaymentFactory {
    public static function create(string $type): PaymentGateway {
        return match ($type) {
            'card' => new CardGateway(),
            'kakao' => new KakaoGateway(),
            'naver' => new NaverGateway(),
            default => throw new InvalidArgumentException("알 수 없는 결제 수단: $type"),
        };
    }
}

$gateway = PaymentFactory::create('kakao');
echo $gateway->pay(15000) . "\n\n";

echo "=== 3. 전략 (Strategy) ===\n";

interface ShippingStrategy {
    public function calculate(float $weight): float;
}

class StandardShipping implements ShippingStrategy {
    public function calculate(float $weight): float {
        return $weight * 1000 + 3000;   // 무게당 + 기본료
    }
}

class ExpressShipping implements ShippingStrategy {
    public function calculate(float $weight): float {
        return $weight * 2000 + 5000;   // 빠른 배송
    }
}

class FreeShipping implements ShippingStrategy {
    public function calculate(float $weight): float {
        return 0;                       // 무료 배송 (프로모션)
    }
}

class Order {
    private ShippingStrategy $shipping;

    public function __construct(
        private string $item,
        private float $weight
    ) {
        $this->shipping = new StandardShipping();
    }

    public function setShippingStrategy(ShippingStrategy $strategy): void {
        $this->shipping = $strategy;
    }

    public function checkout(): string {
        return "{$this->item} (무게 {$this->weight}kg) 배송비: "
            . $this->shipping->calculate($this->weight) . "원";
    }
}

$order = new Order('노트북', 2.5);
echo $order->checkout() . "\n";
$order->setShippingStrategy(new ExpressShipping());
echo $order->checkout() . "\n";
$order->setShippingStrategy(new FreeShipping());
echo $order->checkout() . "\n\n";

echo "=== 4. 옵저버 (Observer) ===\n";

interface Observer {
    public function update(string $event): void;
}

class EmailNotifier implements Observer {
    public function update(string $event): void {
        echo "  [이메일] 신규 가입자 발생: $event → 환영 메일 발송\n";
    }
}

class SmsNotifier implements Observer {
    public function update(string $event): void {
        echo "  [SMS] 관리자에게 가입 알림: $event\n";
    }
}

class UserManager {
    private array $observers = [];

    public function attach(Observer $observer): void {
        $this->observers[] = $observer;
    }

    public function register(string $email): void {
        echo "회원 가입: $email\n";
        $this->notify($email);   // 상태 변화를 구독자에게 통지
    }

    private function notify(string $event): void {
        foreach ($this->observers as $observer) {
            $observer->update($event);
        }
    }
}

$manager = new UserManager();
$manager->attach(new EmailNotifier());
$manager->attach(new SmsNotifier());
$manager->register('alice@example.com');
echo "\n";

echo "=== 5. 의존성 주입 (DI) ===\n";

interface Mailer {
    public function send(string $to, string $body): string;
}

class SmtpMailer implements Mailer {
    public function send(string $to, string $body): string {
        return "SMTP로 {$to}에게 발송: $body";
    }
}

class MockMailer implements Mailer {
    public function send(string $to, string $body): string {
        return "(테스트) 실제 발송 없이 기록: $to → $body";
    }
}

class UserService {
    public function __construct(private Mailer $mailer) {}  // 생성자 주입

    public function register(string $email): string {
        return $this->mailer->send($email, '회원 가입을 환영합니다!');
    }
}

// 실제 서비스에는 SMTP를, 테스트에는 Mock을 주입
$prod = new UserService(new SmtpMailer());
echo $prod->register('bob@example.com') . "\n";

$test = new UserService(new MockMailer());
echo $test->register('test@example.com') . "\n\n";

// 간단한 DI 컨테이너
echo "=== 6. 간단한 DI 컨테이너 ===\n";

class Container {
    private array $bindings = [];
    private array $singletons = [];

    public function bind(string $id, callable $factory): void {
        $this->bindings[$id] = $factory;
    }

    public function singleton(string $id, callable $factory): void {
        $this->bindings[$id] = function () use ($id, $factory) {
            return $this->singletons[$id] ??= $factory();
        };
    }

    public function get(string $id): mixed {
        return $this->bindings[$id]();
    }
}

$container = new Container();
$container->bind(Mailer::class, fn() => new SmtpMailer());
$container->singleton(UserService::class, fn() => new UserService($container->get(Mailer::class)));

$service1 = $container->get(UserService::class);
$service2 = $container->get(UserService::class);
echo "컨테이너로 생성된 서비스가 동일 인스턴스? " . ($service1 === $service2 ? 'true' : 'false') . "\n";
echo $service1->register('carol@example.com') . "\n";

<?php
// --- 네임스페이스 기본 예제 (파일 내 시뮬레이션) ---

// 첫 번째 네임스페이스
namespace App\Models {
    class User {
        private string $name;
        private string $email;

        public function __construct(string $name, string $email) {
            $this->name = $name;
            $this->email = $email;
        }

        public function getInfo(): string {
            return "User: {$this->name} ({$this->email})";
        }
    }

    class Product {
        public function __construct(
            private string $title,
            private float $price
        ) {}

        public function getInfo(): string {
            return "Product: {$this->title} (\${$this->price})";
        }
    }
}

// 두 번째 네임스페이스
namespace App\Services {
    class EmailService {
        public function send(string $to, string $subject): bool {
            echo "이메일 발송: $to - $subject\n";
            return true;
        }
    }

    class Logger {
        public static function log(string $message): void {
            echo "[LOG] $message\n";
        }
    }
}

// 세 번째 네임스페이스 (유틸리티 함수)
namespace App\Helpers {
    function formatDate(string $format = 'Y-m-d'): string {
        return date($format);
    }

    const APP_NAME = 'Basic PHP Course';
}

// --- 사용 예제 ---
namespace {
    // use import
    use App\Models\User;
    use App\Models\Product;
    use App\Services\EmailService;
    use App\Services\Logger;
    use function App\Helpers\formatDate;
    use const App\Helpers\APP_NAME;

    echo "=== 네임스페이스 예제 ===\n\n";

    // 클래스 사용
    $user = new User('Alice', 'alice@example.com');
    echo $user->getInfo() . "\n";

    $product = new Product('PHP Course', 29.99);
    echo $product->getInfo() . "\n";

    // 서비스 사용
    $email = new EmailService();
    $email->send('bob@example.com', 'Welcome!');

    Logger::log('사용자가 로그인했습니다.');

    // 함수/상수
    echo "오늘 날짜: " . formatDate() . "\n";
    echo "앱 이름: " . APP_NAME . "\n";

    // 별칭 (alias)
    use App\Models\Product as Item;
    $item = new Item('Laptop', 999.99);
    echo $item->getInfo() . "\n";

    // FQCN (Fully Qualified Class Name)
    $logger = new \App\Services\Logger();
    $logger::log('FQCN으로 직접 호출');

    echo "\n=== Composer 사용 예제 ===\n";
    echo "composer.json:\n";
    echo json_encode([
        'name' => 'user/basic-php',
        'autoload' => [
            'psr-4' => ['App\\' => 'src/']
        ]
    ], JSON_PRETTY_PRINT) . "\n\n";
    echo "실행 명령어:\n";
    echo "  composer init\n";
    echo "  composer require monolog/monolog\n";
    echo "  composer dump-autoload\n";
}

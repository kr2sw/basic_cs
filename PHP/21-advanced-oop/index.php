<?php
// --- 고급 OOP: 트레이트, 익명 클래스, 매직 메서드, 객체 복사 ---

echo "=== 1. 트레이트 (Trait) ===\n";

trait Timestampable {
    public function getCreatedAt(): string {
        return $this->createdAt ?? date('Y-m-d H:i:s');
    }
}

trait Sluggable {
    abstract public function getTitle(): string;

    public function getSlug(): string {
        return strtolower(str_replace(' ', '-', $this->getTitle()));
    }
}

trait SoftDeletes {
    public function isDeleted(): bool {
        return $this->deletedAt !== null;
    }
}

class Post {
    use Timestampable, Sluggable, SoftDeletes;

    private ?string $deletedAt = null;

    public function __construct(private string $title) {}

    public function getTitle(): string {
        return $this->title;
    }

    public function delete(): void {
        $this->deletedAt = date('Y-m-d H:i:s');
    }
}

$post = new Post('PHP 고급 기법');
echo "제목: {$post->getTitle()}\n";
echo "슬러그: {$post->getSlug()}\n";
echo "생성일: {$post->getCreatedAt()}\n";
echo "삭제 여부: " . ($post->isDeleted() ? '삭제됨' : '정상') . "\n";
$post->delete();
echo "삭제 후: " . ($post->isDeleted() ? '삭제됨' : '정상') . "\n\n";

echo "=== 2. 트레이트 충돌 해결 (insteadof / as) ===\n";

trait A {
    public function hello(): string {
        return 'A';
    }
}

trait B {
    public function hello(): string {
        return 'B';
    }
}

class Both {
    use A, B {
        A::hello insteadof B;   // A의 hello를 우선 사용
        B::hello as helloFromB; // B의 hello는 별칭으로 사용
    }
}

$both = new Both();
echo "기본: {$both->hello()}\n";
echo "별칭: {$both->helloFromB()}\n\n";

echo "=== 3. 익명 클래스 (Anonymous Class) ===\n";

interface Greeter {
    public function greet(string $name): string;
}

$greeter = new class('안녕하세요') implements Greeter {
    public function __construct(private string $prefix) {}

    public function greet(string $name): string {
        return "{$this->prefix}, $name 님!";
    }
};

echo $greeter->greet('Alice') . "\n";

$logger = new class {
    private int $count = 0;

    public function log(string $msg): void {
        $this->count++;
        echo "  [로그 #{$this->count}] $msg\n";
    }
};

$logger->log('첫 번째 메시지');
$logger->log('두 번째 메시지');
echo "클래스명: " . $logger::class . "\n\n";

echo "=== 4. 매직 메서드 ===\n";

class Config {
    private array $data = [];

    // 존재하지 않는 프로퍼티 읽기
    public function __get(string $name): mixed {
        echo "  [__get] $name 프로퍼티 요청\n";
        return $this->data[$name] ?? null;
    }

    // 존재하지 않는 프로퍼티 쓰기
    public function __set(string $name, mixed $value): void {
        echo "  [__set] $name = " . (is_scalar($value) ? $value : gettype($value)) . "\n";
        $this->data[$name] = $value;
    }

    public function __isset(string $name): bool {
        echo "  [__isset] $name\n";
        return isset($this->data[$name]);
    }

    public function __unset(string $name): void {
        echo "  [__unset] $name\n";
        unset($this->data[$name]);
    }

    // 존재하지 않는 메서드 호출
    public function __call(string $name, array $arguments): mixed {
        echo "  [__call] 존재하지 않는 메서드: $name(" . implode(', ', $arguments) . ")\n";
        return null;
    }

    public static function __callStatic(string $name, array $arguments): mixed {
        echo "  [__callStatic] 존재하지 않는 정적 메서드: $name\n";
        return null;
    }

    // 객체를 문자열로 변환
    public function __toString(): string {
        return json_encode($this->data, JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
    }

    // 객체를 함수처럼 호출
    public function __invoke(string $key): mixed {
        return $this->data[$key] ?? null;
    }
}

$config = new Config();
$config->db_host = 'localhost';
$config->db_name = 'testdb';

echo "db_host 값: " . var_export($config->db_host, true) . "\n";
echo "isset(db_host)? " . (isset($config->db_host) ? 'true' : 'false') . "\n";
unset($config->db_host);
echo "unset 후 isset(db_host)? " . (isset($config->db_host) ? 'true' : 'false') . "\n";

$config->notExistingMethod(1, 2);
Config::someStaticMethod();

echo "toString 출력:\n$config\n";
echo "invoke로 값 조회: " . $config('db_name') . "\n\n";

echo "=== 5. 객체 복사 (clone) ===\n";

class Address {
    public function __construct(public string $city) {}
}

class User {
    public array $tags = [];

    public function __construct(
        public string $name,
        public Address $address
    ) {}

    // 깊은 복사: 참조 타입 프로퍼티를 직접 복제
    public function __clone() {
        $this->address = clone $this->address;
    }
}

$alice = new User('Alice', new Address('서울'));
$alice->tags = ['php', 'oop'];

$bob = clone $alice;
$bob->name = 'Bob';
$bob->address->city = '부산';          // __clone 덕분에 독립된 객체
$bob->tags[] = 'advanced';

echo "Alice: {$alice->name} / {$alice->address->city} / " . implode(',', $alice->tags) . "\n";
echo "Bob:   {$bob->name} / {$bob->address->city} / " . implode(',', $bob->tags) . "\n\n";

echo "=== 6. Enum (PHP 8.1+) ===\n";

enum OrderStatus: string {
    case Pending = 'pending';
    case Paid = 'paid';
    case Shipped = 'shipped';
    case Cancelled = 'cancelled';

    public function label(): string {
        return match ($this) {
            self::Pending => '대기 중',
            self::Paid => '결제 완료',
            self::Shipped => '배송 중',
            self::Cancelled => '취소됨',
        };
    }
}

$status = OrderStatus::Paid;
echo "상태 코드: {$status->value} ({$status->label()})\n";
echo "일치 확인: " . ($status === OrderStatus::Paid ? 'true' : 'false') . "\n";
echo "모든 상태: " . implode(', ', array_map(fn($s) => $s->label(), OrderStatus::cases())) . "\n";

<?php
// --- SOLID 원칙: 단일 책임, 개방-폐쇄 등 5원칙 ---

echo "=== S: 단일 책임 원칙 (SRP) ===\n";
echo "  클래스는 단 하나의 이유로만 변경되어야 한다.\n\n";

// 나쁜 예: 데이터 조회 + 포맷 + 저장을 한 클래스에
class BadReport {
    public function getData(): array {
        return ['sales' => 1000, 'expense' => 300];
    }

    public function formatHtml(array $data): string {
        return "<h1>매출: {$data['sales']}</h1>";
    }

    public function saveToFile(string $html): void {
        echo "  파일 저장: $html\n";
    }
}

// 좋은 예: 책임별 클래스 분리
class ReportData {
    public function getData(): array {
        return ['sales' => 1000, 'expense' => 300];
    }
}

class HtmlFormatter {
    public function format(array $data): string {
        return "<h1>매출: {$data['sales']}, 지출: {$data['expense']}</h1>";
    }
}

class ReportFileWriter {
    public function save(string $content): void {
        echo "  저장 완료: $content\n";
    }
}

$data = (new ReportData())->getData();
$html = (new HtmlFormatter())->format($data);
(new ReportFileWriter())->save($html);
echo "  (포맷을 바꿔도 저장 로직은 영향 없음)\n\n";

echo "=== O: 개방-폐쇄 원칙 (OCP) ===\n";
echo "  확장에는 열려 있고, 수정에는 닫혀 있어야 한다.\n\n";

// 나쁜 예: 도형 추가 때마다 switch를 수정해야 함
class BadAreaCalculator {
    public function area(array $shapes): float {
        $total = 0;
        foreach ($shapes as $s) {
            switch ($s['type']) {
                case 'circle':
                    $total += pi() * $s['r'] ** 2;
                    break;
                case 'square':
                    $total += $s['w'] ** 2;
                    break;
            }
        }
        return $total;
    }
}

// 좋은 예: 다형성으로 확장
interface Shape {
    public function area(): float;
}

class Circle implements Shape {
    public function __construct(private float $radius) {}

    public function area(): float {
        return pi() * $this->radius ** 2;
    }
}

class Square implements Shape {
    public function __construct(private float $width) {}

    public function area(): float {
        return $this->width ** 2;
    }
}

class Rectangle implements Shape {
    public function __construct(private float $w, private float $h) {}

    public function area(): float {
        return $this->w * $this->h;
    }
}

class AreaCalculator {
    public function total(array $shapes): float {
        return array_sum(array_map(fn(Shape $s) => $s->area(), $shapes));
    }
}

$shapes = [new Circle(3), new Square(4), new Rectangle(2, 5)];
printf("  전체 면적: %.2f\n", (new AreaCalculator())->total($shapes));
echo "  (새 도형을 추가해도 AreaCalculator는 수정하지 않음)\n\n";

echo "=== L: 리스코프 치환 원칙 (LSP) ===\n";
echo "  하위 타입은 상위 타입이 쓰이는 곳에 치환 가능해야 한다.\n\n";

// 나쁜 예: Square가 Rectangle의 불변식을 깨뜨림
class BadRectangle {
    public function __construct(protected int $w, protected int $h) {}

    public function setWidth(int $w): void {
        $this->w = $w;
    }

    public function setHeight(int $h): void {
        $this->h = $h;
    }

    public function area(): int {
        return $this->w * $this->h;
    }
}

class BadSquare extends BadRectangle {
    public function setWidth(int $w): void {
        $this->w = $this->h = $w;
    }

    public function setHeight(int $h): void {
        $this->w = $this->h = $h;
    }
}

$bad = new BadSquare(2, 2);
$bad->setWidth(5);
$bad->setHeight(10);
echo "  BadSquare: setWidth(5), setHeight(10) → area = " . $bad->area()
    . " (직사각형이라면 50이어야 하지만 정사각형 규칙이 깨뜨림)\n";

// 좋은 예: 공통 인터페이스에 의존
interface Rectangular {
    public function area(): int;
}

class Rect implements Rectangular {
    public function __construct(protected int $w, protected int $h) {}

    public function area(): int {
        return $this->w * $this->h;
    }
}

class Square2 implements Rectangular {
    public function __construct(protected int $side) {}

    public function area(): int {
        return $this->side * $this->side;
    }
}

echo "  Rect(2,3) 면적: " . (new Rect(2, 3))->area()
    . ", Square(4) 면적: " . (new Square2(4))->area() . "\n";
echo "  (각자 자신의 불변식을 지키며 Rectangular로 치환 가능)\n\n";

echo "=== I: 인터페이스 분리 원칙 (ISP) ===\n";
echo "  클라이언트는 사용하지 않는 메서드에 의존하면 안 된다.\n\n";

// 나쁜 예: 범용 인터페이스 — 사용하지 않는 메서드까지 구현해야 함
interface BadWorker {
    public function code(): void;
    public function design(): void;
    public function test(): void;
}

// 좋은 예: 역할별 인터페이스 분리
interface Coder {
    public function code(): void;
}

interface Designer {
    public function design(): void;
}

interface Tester {
    public function test(): void;
}

class PhpDeveloper implements Coder, Tester {
    public function code(): void {
        echo "  PHP 코딩\n";
    }

    public function test(): void {
        echo "  테스트 작성\n";
    }
}

class UiDesigner implements Designer {
    public function design(): void {
        echo "  UI 설계\n";
    }
}

(new PhpDeveloper())->code();
(new PhpDeveloper())->test();
(new UiDesigner())->design();
echo "  (디자이너는 code()를 구현할 필요 없음)\n\n";

echo "=== D: 의존성 역전 원칙 (DIP) ===\n";
echo "  상위 모듈은 추상화에 의존해야 하며, 구체 클래스에 의존하면 안 된다.\n\n";

// 나쁜 예: 상위 모듈이 구체 클래스를 직접 생성
class BadSmtpClient {
    public function sendMail(string $to, string $body): string {
        return "SMTP 발송: $to";
    }
}

class BadMailService {
    private BadSmtpClient $smtp;

    public function __construct() {
        $this->smtp = new BadSmtpClient();   // 직접 생성 → 교체 불가
    }

    public function notify(string $to): string {
        return $this->smtp->sendMail($to, '공지');
    }
}

// 좋은 예: 추상화(인터페이스)에 의존
interface MailerInterface {
    public function send(string $to, string $body): string;
}

class SmtpMailer implements MailerInterface {
    public function send(string $to, string $body): string {
        return "SMTP 발송: $to ($body)";
    }
}

class MailService {
    public function __construct(private MailerInterface $mailer) {}

    public function notify(string $to): string {
        return $this->mailer->send($to, '공지');
    }
}

echo "  " . (new MailService(new SmtpMailer()))->notify('a@example.com') . "\n";
echo "  (테스트에서는 Mock 구현을 주입할 수 있음)\n";

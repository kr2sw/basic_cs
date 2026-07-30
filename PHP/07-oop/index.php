<?php
class Car {
    // 프로퍼티
    public string $model;
    private int $year;
    private string $color;

    // 상수
    const WHEELS = 4;

    // 생성자 (PHP 8+ 프로모션)
    public function __construct(
        string $model,
        int $year,
        string $color = "White"
    ) {
        $this->model = $model;
        $this->year = $year;
        $this->color = $color;
        echo "Car 객체 생성: $model\n";
    }

    // 소멸자
    public function __destruct() {
        echo "Car 객체 소멸: {$this->model}\n";
    }

    // Getter / Setter
    public function getYear(): int {
        return $this->year;
    }

    public function setYear(int $year): void {
        $this->year = $year;
    }

    public function getColor(): string {
        return $this->color;
    }

    // 메서드
    public function start(): void {
        echo "{$this->model} 시동을 겁니다.\n";
    }

    public function displayInfo(): void {
        echo "{$this->model} ({$this->year}년식, {$this->color})\n";
    }

    // static 메서드
    public static function getWheelCount(): int {
        return self::WHEELS;
    }
}

// 객체 생성
$myCar = new Car("Tesla Model 3", 2023, "Red");
$myCar->displayInfo();
$myCar->start();
echo "바퀴 수: " . Car::getWheelCount() . "\n";
echo "상수: " . Car::WHEELS . "\n";

// Setter
$myCar->setYear(2024);
echo "변경된 연식: " . $myCar->getYear() . "\n";

// Nullsafe 연산자 (PHP 8+)
class Garage {
    public ?Car $car = null;
}
$garage = new Garage();
echo "차량 모델: " . $garage?->car?->model ?? "없음" . "\n";

$garage->car = $myCar;
echo "차량 모델: " . $garage?->car?->model . "\n";

// 객체 비교
$car1 = new Car("Kia EV6", 2023);
$car2 = new Car("Kia EV6", 2023);
echo "== 비교: " . ($car1 == $car2 ? "true" : "false") . "\n";
echo "=== 비교: " . ($car1 === $car2 ? "true" : "false") . "\n";

// instanceof
echo "instanceof Car: " . ($myCar instanceof Car ? "true" : "false") . "\n";

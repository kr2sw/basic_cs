<?php
// 인터페이스
interface Drawable {
    public function draw(): void;
}

interface Resizable {
    public function resize(float $factor): void;
}

// 추상 클래스
abstract class Shape {
    protected string $color;

    public function __construct(string $color) {
        $this->color = $color;
    }

    // 추상 메서드
    abstract public function getArea(): float;

    // 일반 메서드
    public function getColor(): string {
        return $this->color;
    }
}

// Trait
trait Logger {
    public function log(string $message): void {
        echo "[LOG] $message\n";
    }
}

trait Timestamp {
    public function getTimestamp(): string {
        return date("Y-m-d H:i:s");
    }
}

// 구현 클래스
class Circle extends Shape implements Drawable, Resizable {
    use Logger, Timestamp;

    private float $radius;

    public function __construct(string $color, float $radius) {
        parent::__construct($color);
        $this->radius = $radius;
    }

    public function getArea(): float {
        return pi() * $this->radius * $this->radius;
    }

    public function draw(): void {
        $this->log("[" . $this->getTimestamp() . "] 원을 그립니다.");
        echo "○ {$this->color} 원 (반지름: {$this->radius}, 면적: {$this->getArea()})\n";
    }

    public function resize(float $factor): void {
        $this->radius *= $factor;
        echo "크기 조정됨: 반지름 = {$this->radius}\n";
    }
}

class Rectangle extends Shape implements Drawable {
    private float $width;
    private float $height;

    public function __construct(string $color, float $width, float $height) {
        parent::__construct($color);
        $this->width = $width;
        $this->height = $height;
    }

    public function getArea(): float {
        return $this->width * $this->height;
    }

    public function draw(): void {
        echo "▭ {$this->color} 사각형 ({$this->width} x {$this->height}, 면적: {$this->getArea()})\n";
    }
}

// 사용
$circle = new Circle("빨간", 5);
$circle->draw();
$circle->resize(2.0);
$circle->draw();

$rect = new Rectangle("파란", 4, 6);
$rect->draw();

// 다형성
$shapes = [$circle, $rect];
foreach ($shapes as $shape) {
    echo "면적: " . $shape->getArea() . "\n";
}

// instanceof
echo "circle instanceof Shape: " . ($circle instanceof Shape ? "true" : "false") . "\n";
echo "circle instanceof Drawable: " . ($circle instanceof Drawable ? "true" : "false") . "\n";
echo "circle instanceof Resizable: " . ($circle instanceof Resizable ? "true" : "false") . "\n";

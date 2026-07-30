<?php
// 기본 함수
function add(int $a, int $b): int {
    return $a + $b;
}
echo "add(3, 5) = " . add(3, 5) . "\n";

// 기본값 파라미터
function greet(string $name = "Guest"): string {
    return "Hello, $name!";
}
echo greet() . "\n";
echo greet("Alice") . "\n";

// 가변인자
function sumAll(int ...$numbers): int {
    return array_sum($numbers);
}
echo "sumAll(1,2,3,4,5) = " . sumAll(1, 2, 3, 4, 5) . "\n";

// 타입 선언 (PHP 7+)
function calculate(float $a, float $b, string $op): float|string {
    return match ($op) {
        '+' => $a + $b,
        '-' => $a - $b,
        '*' => $a * $b,
        '/' => $b != 0 ? $a / $b : "0으로 나눌 수 없음",
        default => "알 수 없는 연산자"
    };
}
echo "calculate(10, 3, '+') = " . calculate(10, 3, '+') . "\n";

// 익명 함수 (Closure)
$multiply = function(int $a, int $b): int {
    return $a * $b;
};
echo "익명 함수: " . $multiply(4, 5) . "\n";

// 화살표 함수 (PHP 7.4+)
$square = fn($n) => $n * $n;
echo "화살표 함수: " . $square(6) . "\n";

// 화살표 함수는 외부 변수 자동 캡처
$factor = 2;
$double = fn($n) => $n * $factor;
echo "외부 변수 캡처: " . $double(10) . "\n";

// 재귀 함수
function factorial(int $n): int {
    if ($n <= 1) return 1;
    return $n * factorial($n - 1);
}
echo "factorial(5) = " . factorial(5) . "\n";

// Call by reference
function addTen(int &$num): void {
    $num += 10;
}
$value = 5;
addTen($value);
echo "call by reference: $value\n";

// Named Arguments (PHP 8+)
function createUser(string $name, int $age, string $city = "Seoul"): string {
    return "$name ($age세, $city)";
}
echo createUser(age: 30, name: "Bob") . "\n";

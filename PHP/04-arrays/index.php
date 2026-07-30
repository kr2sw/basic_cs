<?php
// 인덱스 배열
$numbers = [10, 20, 30, 40, 50];
echo "numbers[0]: {$numbers[0]}\n";
echo "count: " . count($numbers) . "\n";

// 배열 추가/제거
array_push($numbers, 60);
array_unshift($numbers, 0);
echo "after push/unshift: " . implode(", ", $numbers) . "\n";

$last = array_pop($numbers);
$first = array_shift($numbers);
echo "pop: $last, shift: $first\n";
echo "after pop/shift: " . implode(", ", $numbers) . "\n";

// 연관 배열
$student = [
    "name" => "Alice",
    "age" => 25,
    "major" => "Computer Science"
];
echo "name: {$student['name']}\n";
echo "age: {$student['age']}\n";

// 다차원 배열
$matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];
echo "matrix[1][2]: {$matrix[1][2]}\n";

// 배열 순회
echo "foreach: ";
foreach ($numbers as $num) {
    echo "$num ";
}
echo "\n";

// 연관 배열 순회
foreach ($student as $key => $value) {
    echo "$key: $value, ";
}
echo "\n";

// 배열 함수
$arr1 = [1, 2, 3];
$arr2 = [4, 5, 6];
$merged = array_merge($arr1, $arr2);
echo "merge: " . implode(", ", $merged) . "\n";

$hasValue = in_array(3, $arr1);
echo "in_array(3): " . ($hasValue ? "true" : "false") . "\n";

// array_map
$squared = array_map(fn($n) => $n * $n, $numbers);
echo "squared: " . implode(", ", $squared) . "\n";

// array_filter
$even = array_filter($numbers, fn($n) => $n % 2 == 0);
echo "even: " . implode(", ", $even) . "\n";

// sort
$unsorted = [3, 1, 4, 1, 5, 9];
sort($unsorted);
echo "sorted: " . implode(", ", $unsorted) . "\n";

// explode / implode
$csv = "apple,banana,cherry";
$parts = explode(",", $csv);
echo "explode: " . implode(" | ", $parts) . "\n";

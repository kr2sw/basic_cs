<?php
$score = 85;

// if-elseif-else
if ($score >= 90) {
    echo "A\n";
} elseif ($score >= 80) {
    echo "B\n";
} elseif ($score >= 70) {
    echo "C\n";
} else {
    echo "F\n";
}

// switch
$day = 3;
switch ($day) {
    case 1: echo "월\n"; break;
    case 2: echo "화\n"; break;
    case 3: echo "수\n"; break;
    default: echo "기타\n";
}

// match (PHP 8+)
$grade = match (true) {
    $score >= 90 => "A",
    $score >= 80 => "B",
    $score >= 70 => "C",
    default => "F"
};
echo "match 학점: $grade\n";

// 삼항 연산자
$age = 20;
$status = $age >= 18 ? "성인" : "미성년자";
echo "$status\n";

// Null 병합 연산자 (PHP 7+)
$username = $_GET['user'] ?? 'guest';
echo "사용자: $username\n";

// for
echo "for: ";
for ($i = 1; $i <= 5; $i++) {
    echo "$i ";
}
echo "\n";

// while
echo "while: ";
$j = 1;
while ($j <= 5) {
    echo "$j ";
    $j++;
}
echo "\n";

// do-while
echo "do-while: ";
$k = 1;
do {
    echo "$k ";
    $k++;
} while ($k <= 5);
echo "\n";

// foreach
$fruits = ["Apple", "Banana", "Cherry"];
echo "foreach: ";
foreach ($fruits as $fruit) {
    echo "$fruit ";
}
echo "\n";

// foreach with key
$scores = ["Alice" => 90, "Bob" => 80, "Charlie" => 95];
foreach ($scores as $name => $score) {
    echo "$name: $score점 ";
}
echo "\n";

// break / continue
echo "break/continue: ";
for ($i = 1; $i <= 10; $i++) {
    if ($i % 2 == 0) continue;
    if ($i > 7) break;
    echo "$i ";
}
echo "\n";

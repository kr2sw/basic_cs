<?php
// 변수
$intVar = 42;
$floatVar = 3.14;
$stringVar = "PHP";
$boolVar = true;
$nullVar = null;

echo "int: $intVar\n";
echo "float: $floatVar\n";
echo "string: $stringVar\n";
echo "bool: " . ($boolVar ? "true" : "false") . "\n";
echo "null: " . var_export($nullVar, true) . "\n";

// 타입 확인
var_dump($intVar);
var_dump($floatVar);
var_dump($stringVar);
var_dump($boolVar);

// 동적 타입
$var = "Hello";
echo "\$var = $var (" . gettype($var) . ")\n";

$var = 100;
echo "\$var = $var (" . gettype($var) . ")\n";

// 형변환
$num = "123";
$converted = (int)$num + 1;
echo "형변환: " . $converted . "\n";

$pi = 3.14159;
$intPi = (int)$pi;
echo "float -> int: $intPi\n"; // 3

// 상수
define("SITE_NAME", "Basic CS");
const VERSION = 1.0;
echo SITE_NAME . " v" . VERSION . "\n";

// 상수 배열 (PHP 7+)
define("FRUITS", ["Apple", "Banana", "Cherry"]);
echo FRUITS[0] . "\n";

// 가변 변수
$varName = "message";
$$varName = "Hello from variable variable!";
echo $message . "\n";

// isset / unset
$test = "value";
echo "isset: " . (isset($test) ? "true" : "false") . "\n";
unset($test);
echo "after unset isset: " . (isset($test) ? "true" : "false") . "\n";

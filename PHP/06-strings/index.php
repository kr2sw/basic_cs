<?php
$str = "  Hello, PHP World!  ";

echo "str: '$str'\n";
echo "strlen: " . strlen($str) . "\n";
echo "trim: '" . trim($str) . "'\n";
echo "strtoupper: " . strtoupper($str) . "\n";
echo "strtolower: " . strtolower($str) . "\n";

// 위치 찾기
$pos = strpos($str, "PHP");
echo "strpos('PHP'): $pos\n";

$notFound = strpos($str, "Java");
echo "strpos('Java'): " . ($notFound === false ? "false" : $notFound) . "\n";

// 부분 문자열
echo "substr(3, 5): '" . substr($str, 3, 5) . "'\n";
echo "substr(-6): '" . substr($str, -6) . "'\n";

// 치환
$replaced = str_replace("PHP", "Java", $str);
echo "replace: '$replaced'\n";

// 포맷팅
$name = "Alice";
$age = 25;
$formatted = sprintf("%s is %d years old.", $name, $age);
echo "sprintf: $formatted\n";
printf("printf: %s %d\n", $name, $age);

// HTML 이스케이프
$html = "<script>alert('XSS')</script>";
echo "htmlspecialchars: " . htmlspecialchars($html) . "\n";

// 문자열 분할/결합
$csv = "apple,banana,cherry";
$parts = explode(",", $csv);
echo "explode: ";
print_r($parts);

$joined = implode(" | ", $parts);
echo "implode: $joined\n";

// 문자열 비교
$str1 = "abc";
$str2 = "ABC";
echo "strcmp: " . strcmp($str1, $str2) . "\n"; // 0이면 같음
echo "strcasecmp: " . strcasecmp($str1, $str2) . "\n"; // 0 (대소문자 무시)

// 패딩 / 반복
echo "str_pad: '" . str_pad("PHP", 10, "-=", STR_PAD_BOTH) . "'\n";
echo "str_repeat: " . str_repeat("Ha", 3) . "\n";

// Heredoc
$heredoc = <<<EOD
이것은 Heredoc입니다.
변수 $name도 사용 가능합니다.
EOD;
echo "$heredoc\n";

// Nowdoc
$nowdoc = <<<'EOD'
이것은 Nowdoc입니다.
변수 $name도 문자 그대로 출력됩니다.
EOD;
echo "$nowdoc\n";

// 역슬래시 이스케이프
echo "줄바꿈: 첫 줄\n둘째 줄\n";
echo "탭:\t들여쓰기\n";

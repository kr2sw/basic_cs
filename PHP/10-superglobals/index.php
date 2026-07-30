<?php
// 실행: php -S localhost:8000
// 브라우저에서 http://localhost:8000/10-superglobals/index.php?name=Alice&age=25

// $_GET
echo "=== \$_GET ===\n";
$name = $_GET['name'] ?? 'Guest';
$age = $_GET['age'] ?? 'unknown';
echo "Name: $name, Age: $age\n\n";

// $_SERVER
echo "=== \$_SERVER ===\n";
echo "REQUEST_METHOD: {$_SERVER['REQUEST_METHOD']}\n";
echo "SERVER_NAME: {$_SERVER['SERVER_NAME']}\n";
echo "SERVER_PORT: {$_SERVER['SERVER_PORT']}\n";
echo "REQUEST_URI: {$_SERVER['REQUEST_URI']}\n";
echo "QUERY_STRING: " . ($_SERVER['QUERY_STRING'] ?? '없음') . "\n";
echo "REMOTE_ADDR: {$_SERVER['REMOTE_ADDR']}\n";
echo "SCRIPT_FILENAME: {$_SERVER['SCRIPT_FILENAME']}\n";
echo "HTTP_USER_AGENT: {$_SERVER['HTTP_USER_AGENT']}\n\n";

// $GLOBALS
echo "=== \$GLOBALS ===\n";
$globalVar = "전역 변수입니다";
echo "globalVar: {$GLOBALS['globalVar']}\n\n";

// 환경 변수
echo "=== \$_ENV ===\n";
echo "OS: " . ($_ENV['OS'] ?? 'Not set') . "\n";
echo "COMPUTERNAME: " . ($_ENV['COMPUTERNAME'] ?? 'Not set') . "\n\n";

// POST (CLI 모드에서는 직접 설정)
echo "=== \$_POST (CLI 모드) ===\n";
echo "CLI에서는 POST 데이터가 없습니다.\n";
echo "웹 서버에서 form 전송 시 사용: \$_POST['username']\n\n";

// 쿼리 문자열 파싱
parse_str($_SERVER['QUERY_STRING'] ?? '', $queryParams);
echo "=== 파싱된 쿼리 파라미터 ===\n";
print_r($queryParams);

<?php
// 세션 시작 (CLI 모드에서는 파일 기반 세션 사용)
session_save_path(__DIR__ . '/sessions');
if (!is_dir(__DIR__ . '/sessions')) {
    mkdir(__DIR__ . '/sessions');
}
session_start();

echo "=== 세션 (Session) ===\n";

// 세션 저장
$_SESSION['username'] = 'Alice';
$_SESSION['role'] = 'admin';
$_SESSION['login_time'] = time();

echo "세션 ID: " . session_id() . "\n";
echo "세션 데이터:\n";
foreach ($_SESSION as $key => $value) {
    if ($key === 'password') continue;
    echo "  $key => " . (is_array($value) ? json_encode($value) : $value) . "\n";
}

// 세션 카운터
$_SESSION['visits'] = ($_SESSION['visits'] ?? 0) + 1;
echo "방문 횟수: {$_SESSION['visits']}\n";

echo "\n=== 쿠키 (Cookie) ===\n";

// 쿠키 설정 (CLI에서는 실제로 전송되지 않음)
setcookie('user_preference', 'dark_mode', time() + 86400 * 30, '/');
setcookie('language', 'ko', time() + 86400 * 7, '/');

echo "쿠키는 HTTP 헤더를 통해 전송됩니다.\n";
echo "setcookie('user_preference', 'dark_mode', time()+86400*30);\n";
echo "setcookie('language', 'ko', time()+86400*7);\n";

// $_COOKIE 읽기 (실제 브라우저에서 요청 시)
if (isset($_COOKIE)) {
    echo "\n현재 요청의 쿠키:\n";
    foreach ($_COOKIE as $key => $value) {
        echo "  $key => $value\n";
    }
}

echo "\n=== 세션 vs 쿠키 ===\n";
echo "세션: 서버에 저장, 보안 우수, 용량 제한 없음\n";
echo "쿠키: 클라이언트에 저장, 4KB 제한, 문자열만 가능\n";

// CLI 환경에서 POST/COOKIE 시뮬레이션
echo "\n=== 로그인 시뮬레이션 (CLI) ===\n";
echo "웹 환경에서는 아래처럼 사용:\n";
echo '  $_SESSION["user_id"] = 1;' . "\n";
echo '  $_SESSION["logged_in"] = true;' . "\n";
echo '  setcookie("remember_me", $token, time() + 86400 * 30);' . "\n";

// 세션 파일 정리
session_write_close();

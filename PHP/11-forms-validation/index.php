<?php
// 폼 데이터 시뮬레이션
$_POST = [
    'username' => '  Alice  ',
    'email' => 'alice@example.com',
    'age' => '25',
    'website' => 'https://example.com',
];

// 입력 검증 함수
function validate(array $data): array {
    $errors = [];
    $sanitized = [];

    // username
    $sanitized['username'] = trim($data['username'] ?? '');
    if (empty($sanitized['username'])) {
        $errors['username'] = '사용자 이름을 입력하세요.';
    } elseif (strlen($sanitized['username']) < 2) {
        $errors['username'] = '이름은 2자 이상이어야 합니다.';
    }

    // email
    $email = filter_var($data['email'] ?? '', FILTER_SANITIZE_EMAIL);
    if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
        $errors['email'] = '유효한 이메일을 입력하세요.';
    } else {
        $sanitized['email'] = $email;
    }

    // age
    $age = filter_var($data['age'] ?? '', FILTER_VALIDATE_INT, [
        'options' => ['min_range' => 1, 'max_range' => 150]
    ]);
    if ($age === false) {
        $errors['age'] = '유효한 나이를 입력하세요. (1-150)';
    } else {
        $sanitized['age'] = $age;
    }

    // website
    $website = filter_var($data['website'] ?? '', FILTER_VALIDATE_URL);
    if ($website === false) {
        $errors['website'] = '유효한 URL을 입력하세요.';
    } else {
        $sanitized['website'] = $website;
    }

    return ['errors' => $errors, 'sanitized' => $sanitized];
}

$result = validate($_POST);

echo "=== 유효성 검사 결과 ===\n";
if (!empty($result['errors'])) {
    echo "오류 발생:\n";
    foreach ($result['errors'] as $field => $msg) {
        echo "  - $field: $msg\n";
    }
} else {
    echo "모든 입력이 유효합니다!\n";
    echo "정제된 데이터:\n";
    foreach ($result['sanitized'] as $key => $value) {
        echo "  $key: $value\n";
    }
}

// filter_input 사용
echo "\n=== filter_input ===\n";
$email = filter_input(INPUT_POST, 'email', FILTER_VALIDATE_EMAIL);
echo "filter_input email: " . ($email ?: 'invalid') . "\n";

// XSS 방지
echo "\n=== XSS 방지 ===\n";
$userInput = "<script>alert('xss')</script><b>굵은 글씨</b>";
echo "원본: $userInput\n";
echo "htmlspecialchars: " . htmlspecialchars($userInput, ENT_QUOTES, 'UTF-8') . "\n";
echo "strip_tags: " . strip_tags($userInput) . "\n";

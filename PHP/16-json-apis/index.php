<?php
// --- JSON 인코딩/디코딩 ---
echo "=== JSON 인코딩/디코딩 ===\n";

$data = [
    'name' => 'Alice',
    'age' => 25,
    'skills' => ['PHP', 'JavaScript', 'Python'],
    'active' => true,
    'address' => [
        'city' => 'Seoul',
        'zip' => '12345'
    ]
];

$json = json_encode($data, JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE);
echo "JSON 출력:\n$json\n\n";

// JSON -> PHP 배열
$decoded = json_decode($json, true); // true = 연관 배열
echo "디코딩된 name: {$decoded['name']}\n";
echo "디코딩된 city: {$decoded['address']['city']}\n";

// JSON -> PHP 객체
$obj = json_decode($json);
echo "객체 접근: {$obj->name}, {$obj->address->city}\n";

// --- file_get_contents로 API 호출 ---
echo "\n=== HTTP GET (file_get_contents) ===\n";

$context = stream_context_create([
    'http' => [
        'method' => 'GET',
        'header' => "Accept: application/json\r\n",
        'timeout' => 5,
    ]
]);

$response = @file_get_contents(
    'https://httpbin.org/get?name=Alice&age=25',
    false,
    $context
);

if ($response !== false) {
    $data = json_decode($response, true);
    echo "args: " . json_encode($data['args'], JSON_UNESCAPED_UNICODE) . "\n";
    echo "url: {$data['url']}\n";
} else {
    echo "API 호출 실패 (네트워크 필요)\n";
}

// --- cURL 사용 ---
echo "\n=== cURL ===\n";

function httpGet(string $url): ?array {
    $ch = curl_init();
    curl_setopt_array($ch, [
        CURLOPT_URL => $url,
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_TIMEOUT => 5,
        CURLOPT_HTTPHEADER => ['Accept: application/json'],
    ]);

    $response = curl_exec($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    if ($httpCode === 200 && $response) {
        return json_decode($response, true);
    }
    return null;
}

function httpPost(string $url, array $data): ?array {
    $ch = curl_init();
    curl_setopt_array($ch, [
        CURLOPT_URL => $url,
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_POST => true,
        CURLOPT_POSTFIELDS => json_encode($data),
        CURLOPT_HTTPHEADER => [
            'Content-Type: application/json',
            'Accept: application/json',
        ],
    ]);

    $response = curl_exec($ch);
    $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    if ($httpCode === 200 && $response) {
        return json_decode($response, true);
    }
    return null;
}

// GET 요청
$result = httpGet('https://httpbin.org/get?q=php');
if ($result) {
    echo "GET args: " . json_encode($result['args'] ?? []) . "\n";
}

// POST 요청
$result = httpPost('https://httpbin.org/post', [
    'title' => 'Hello',
    'body' => 'World'
]);
if ($result) {
    echo "POST data: " . json_encode($result['data'] ?? []) . "\n";
}

// --- JSON 파일 읽기/쓰기 ---
echo "\n=== JSON 파일 ===\n";
$filePath = __DIR__ . '/data.json';

file_put_contents($filePath, json_encode($data, JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE));
echo "JSON 파일 저장 완료\n";

$loaded = json_decode(file_get_contents($filePath), true);
echo "JSON 파일 읽기: {$loaded['name']}\n";

unlink($filePath);
echo "임시 파일 정리 완료\n";

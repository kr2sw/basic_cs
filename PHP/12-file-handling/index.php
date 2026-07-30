<?php
$filePath = __DIR__ . "/sample.txt";
$copyPath = __DIR__ . "/sample_copy.txt";

// 파일 쓰기 (file_put_contents)
$content = "Hello, PHP File!\n파일 입출력 예제입니다.\n여러 줄을 테스트합니다.";
file_put_contents($filePath, $content);
echo "파일 작성 완료: $filePath\n";

// 파일 읽기 (file_get_contents)
$readContent = file_get_contents($filePath);
echo "\n=== file_get_contents ===\n$readContent\n";

// 파일을 배열로 읽기
$lines = file($filePath);
echo "\n=== file() - 줄 단위 ===\n";
foreach ($lines as $i => $line) {
    echo "Line " . ($i + 1) . ": $line";
}

// fopen / fgets / fwrite
echo "\n=== fopen/fgets ===\n";
$handle = fopen($filePath, "r");
if ($handle) {
    while (($line = fgets($handle)) !== false) {
        echo "읽기: $line";
    }
    fclose($handle);
}

// 파일 추가 쓰기
file_put_contents($filePath, "추가된 내용\n", FILE_APPEND);
echo "\n파일 추가 완료\n";

// 파일 복사
copy($filePath, $copyPath);
echo "파일 복사 완료: $copyPath\n";

// 파일 정보
echo "\n=== 파일 정보 ===\n";
echo "파일명: " . basename($filePath) . "\n";
echo "디렉토리: " . dirname($filePath) . "\n";
echo "크기: " . filesize($filePath) . " bytes\n";
echo "존재: " . (file_exists($filePath) ? "true" : "false") . "\n";
echo "수정 시간: " . date("Y-m-d H:i:s", filemtime($filePath)) . "\n";

// 디렉토리
echo "\n=== 디렉토리 목록 ===\n";
$items = scandir(__DIR__);
foreach ($items as $item) {
    if ($item === "." || $item === "..") continue;
    $type = is_dir(__DIR__ . "/$item") ? "[DIR]" : "[FILE]";
    echo "$type $item\n";
}

// glob 패턴 검색
echo "\n=== glob 패턴 ===\n";
$phpFiles = glob(__DIR__ . "/*.php");
foreach ($phpFiles as $f) {
    echo "  " . basename($f) . "\n";
}

// 파일 삭제
unlink($filePath);
unlink($copyPath);
echo "\n임시 파일 정리 완료\n";

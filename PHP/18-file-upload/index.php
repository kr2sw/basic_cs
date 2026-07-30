<?php
$uploadDir = __DIR__ . '/uploads';
if (!is_dir($uploadDir)) {
    mkdir($uploadDir, 0777, true);
}

// 허용 MIME 타입
$allowedTypes = ['image/jpeg', 'image/png', 'image/gif', 'text/plain'];
$maxFileSize = 5 * 1024 * 1024; // 5MB

// 파일 업로드 시뮬레이션 (실제 업로드 대신 가상 데이터)
function simulateUpload(string $filename, string $type, int $size, int $errorCode): array {
    $tmpFile = tempnam(sys_get_temp_dir(), 'upl');
    file_put_contents($tmpFile, str_repeat('x', $size));
    
    return [
        'name' => $filename,
        'type' => $type,
        'tmp_name' => $tmpFile,
        'error' => $errorCode,
        'size' => $size,
    ];
}

// 업로드된 파일 처리 함수
function processUpload(array $file, string $uploadDir, array $allowedTypes, int $maxFileSize): string {
    // 에러 확인
    if ($file['error'] !== UPLOAD_ERR_OK) {
        $errorMessages = [
            UPLOAD_ERR_INI_SIZE => 'php.ini 업로드 크기 초과',
            UPLOAD_ERR_FORM_SIZE => '폼 업로드 크기 초과',
            UPLOAD_ERR_PARTIAL => '파일이 일부만 업로드됨',
            UPLOAD_ERR_NO_FILE => '파일이 업로드되지 않음',
            UPLOAD_ERR_NO_TMP_DIR => '임시 디렉토리 없음',
            UPLOAD_ERR_CANT_WRITE => '디스크 쓰기 실패',
        ];
        throw new RuntimeException($errorMessages[$file['error']] ?? '알 수 없는 오류');
    }

    // 파일 크기 검사
    if ($file['size'] > $maxFileSize) {
        throw new RuntimeException("파일 크기 초과 ({$file['size']} > {$maxFileSize})");
    }

    // MIME 타입 검사
    $finfo = finfo_open(FILEINFO_MIME_TYPE);
    $mimeType = finfo_file($finfo, $file['tmp_name']);
    finfo_close($finfo);

    if (!in_array($mimeType, $allowedTypes)) {
        throw new RuntimeException("허용되지 않는 파일 타입: $mimeType");
    }

    // 안전한 파일명 생성
    $extension = pathinfo($file['name'], PATHINFO_EXTENSION);
    $safeName = uniqid('upload_') . '.' . $extension;
    $destPath = $uploadDir . '/' . $safeName;

    // 파일 이동
    if (!move_uploaded_file($file['tmp_name'], $destPath)) {
        throw new RuntimeException('파일 이동 실패');
    }

    return $destPath;
}

echo "=== 파일 업로드 예제 ===\n\n";

// 성공 케이스
echo "1. 성공 케이스:\n";
$fakeFile = simulateUpload('profile.jpg', 'image/jpeg', 102400, UPLOAD_ERR_OK);
try {
    $result = processUpload($fakeFile, $uploadDir, $allowedTypes, $maxFileSize);
    echo "  ✅ 업로드 성공: $result\n";
    echo "  파일 크기: " . filesize($result) . " bytes\n";
    echo "  MIME: " . mime_content_type($result) . "\n";
} catch (RuntimeException $e) {
    echo "  ❌ 실패: " . $e->getMessage() . "\n";
}

// 타입 오류
echo "\n2. 타입 오류 케이스:\n";
$fakeFile = simulateUpload('malware.exe', 'application/x-msdownload', 50000, UPLOAD_ERR_OK);
try {
    $result = processUpload($fakeFile, $uploadDir, $allowedTypes, $maxFileSize);
    echo "  ✅ 업로드 성공: $result\n";
} catch (RuntimeException $e) {
    echo "  ❌ 실패: " . $e->getMessage() . "\n";
}

// 파일 없음
echo "\n3. 파일 없음 케이스:\n";
$fakeFile = simulateUpload('', '', 0, UPLOAD_ERR_NO_FILE);
try {
    $result = processUpload($fakeFile, $uploadDir, $allowedTypes, $maxFileSize);
    echo "  ✅ 업로드 성공: $result\n";
} catch (RuntimeException $e) {
    echo "  ❌ 실패: " . $e->getMessage() . "\n";
}

// 실제 폼 예제 출력
echo "\n=== HTML 폼 예제 ===\n";
echo <<<HTML
<form method="POST" enctype="multipart/form-data">
    <input type="hidden" name="MAX_FILE_SIZE" value="5242880">
    <label>파일 선택: <input type="file" name="upload"></label>
    <input type="submit" value="업로드">
</form>
HTML;
echo "\n\n";

// 업로드 디렉토리 정리
array_map('unlink', glob($uploadDir . '/*'));
rmdir($uploadDir);
echo "임시 파일 정리 완료\n";

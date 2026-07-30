# 16: JSON & APIs — JSON 처리와 API 통신

## JSON 함수

| 함수 | 설명 |
|------|------|
| `json_encode()` | PHP 배열/객체 → JSON 문자열 |
| `json_decode()` | JSON 문자열 → PHP 배열/객체 |
| `json_last_error()` | 마지막 JSON 오류 확인 |
| `json_last_error_msg()` | 오류 메시지 반환 |

## cURL (Client URL Library)

PHP에서 HTTP 요청을 보내는 라이브러리입니다.

```php
$ch = curl_init();
curl_setopt_array($ch, [
    CURLOPT_URL => 'https://api.example.com',
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_HTTPHEADER => ['Content-Type: application/json'],
]);
$response = curl_exec($ch);
curl_close($ch);
```

## file_get_contents로 간단한 GET 요청

```php
$response = file_get_contents('https://api.example.com/data');
```

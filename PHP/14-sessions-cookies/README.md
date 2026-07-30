# 14: Sessions & Cookies — 세션과 쿠키

## 세션 (Session)

서버 측에 데이터를 저장하는 방식입니다.

```php
session_start();                    // 세션 시작
$_SESSION['user'] = 'Alice';        // 세션 저장
$user = $_SESSION['user'];          // 세션 읽기
session_destroy();                  // 세션 삭제
```

## 쿠키 (Cookie)

클라이언트 측(브라우저)에 데이터를 저장하는 방식입니다.

```php
setcookie('name', 'value', time() + 3600, '/');  // 쿠키 설정
$value = $_COOKIE['name'];                        // 쿠키 읽기
setcookie('name', '', time() - 3600);             // 쿠키 삭제
```

## 차이점

| 특징 | 세션 | 쿠키 |
|------|------|------|
| 저장 위치 | 서버 | 클라이언트 |
| 용량 제한 | 없음 | 4KB |
| 보안 | 상대적 안전 | 취약 |
| 만료 | 브라우저 종료 (기본) | 설정 가능 |
| 데이터 타입 | 배열/객체 가능 | 문자열만 |

# 29: 인증 — 세션 인증, Sanctum/JWT 토큰 개념

## 비밀번호 해싱

비밀번호는 **절대 평문으로 저장하지 않습니다**.

```php
$hash = password_hash($password, PASSWORD_BCRYPT, ['cost' => 12]);
password_verify($password, $hash);   // true/false
```

`salt`가 자동 포함되어 레인보우 테이블 공격에 안전합니다.

## 세션 인증 (웹)

브라우저 쿠키 + 서버 세션 기반의 전통적 방식입니다.

```php
Auth::attempt($email, $password);  // 성공 시 세션에 사용자 기록
Auth::check();                     // 로그인 여부
Auth::user();                      // 현재 사용자
```

라우트는 `middleware('auth')`로 보호합니다. 로그인하지 않으면 로그인 페이지로 리다이렉트됩니다.

## API 토큰 (Laravel Sanctum)

API 요청은 쿠키 대신 `Authorization: Bearer <token>` 헤더로 인증합니다.

```php
$user->createToken('mobile-app');           // 토큰 발급
$request->user();                           // 토큰으로 사용자 해석
```

토큰을 해시해서 저장하므로 DB가 유출되어도 원본 토큰은 알 수 없습니다.

## JWT (JSON Web Token)

`헤더.페이로드.서명` 세 부분으로 구성된 **무상태(stateless)** 토큰입니다.

```
base64url(header).base64url(payload).HMAC-SHA256(서명)
```

- 서버는 토큰을 저장하지 않고 서명만 검증합니다
- 만료(`exp`), 발급자(`iss`), 사용자(`sub`)를 페이로드에 담습니다
- 만료 전에는 폐기하기 어려워 짧은 유효기간 + Refresh Token을 함께 씁니다

## 실행

```bash
php index.php
```

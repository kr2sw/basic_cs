# 32: 보안 — password_hash, CSRF, XSS, SQL 인젝션 방어

## 비밀번호 해싱

```php
$hash = password_hash($password, PASSWORD_BCRYPT, ['cost' => 12]);
password_verify($password, $hash);   // true/false
```

- salt 자동 생성, 평문 절대 저장 금지
- `cost` 값으로 연산 비용(보안 강도) 조절
- 로그인 성공 시점에 `password_needs_rehash()`로 재해싱 가능

## CSRF (Cross-Site Request Forgery)

사용자가 자신도 모르게 위조된 요청을 보내도록 만드는 공격입니다.

```php
// 폼에 토큰 포함
<input type="hidden" name="_token" value="{{ csrf_token() }}">

// 서버에서 검증
if (!hash_equals($_SESSION['_token'], $_POST['_token'])) {
    abort(419);
}
```

- 토큰은 `random_bytes()`로 생성하고 세션에 저장
- 비교는 반드시 `hash_equals()` (타이밍 공격 방어)

## XSS (Cross-Site Scripting)

사용자 입력에 포함된 스크립트를 그대로 출력하면 발생합니다.

```php
echo htmlspecialchars($userComment, ENT_QUOTES, 'UTF-8');
```

`ENT_QUOTES`는 `'`와 `"`를 모두 인코딩합니다. 블레이드는 `{{ }}`이 기본적으로 이스케이프합니다.

## SQL 인젝션

문자열 연결로 SQL을 만들지 말고 **Prepared Statement**를 사용합니다.

```php
// 안전
$stmt = $pdo->prepare('SELECT * FROM users WHERE email = ?');
$stmt->execute([$input]);
```

## 입력 검증

```php
filter_var($email, FILTER_VALIDATE_EMAIL);
filter_var($id, FILTER_VALIDATE_INT);
```

검증과 이스케이프는 각각 다른 시점(입력 시 검증, 출력 시 이스케이프)에 적용합니다.

## 실행

```bash
php index.php
```

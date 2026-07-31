# 24: 고급 PDO — 트랜잭션, prepared statement, Repository 패턴

## PDO 연결 (SQLite 인메모리)

외부 DB 없이 테스트하려면 SQLite 메모리 DB를 사용합니다.

```php
$pdo = new PDO('sqlite::memory:');
$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
```

`ERRMODE_EXCEPTION`을 켜면 SQL 오류가 예외로 변환됩니다.

## Prepared Statement

위치 기반(`?`)과 이름 기반(`:name`) 두 가지 방식이 있습니다.

```php
$stmt = $pdo->prepare('INSERT INTO users (name) VALUES (:name)');
$stmt->execute(['name' => 'Alice']);   // 이름 기반
```

사용자 입력은 절대 문자열 연결(`$sql . $_GET['id']`)로 넣지 말고 바인딩합니다.

## 트랜잭션

여러 SQL을 하나의 작업으로 묶습니다. 중간에 실패하면 전부 되돌립니다.

```php
$pdo->beginTransaction();
try {
    // ... 여러 INSERT / UPDATE ...
    $pdo->commit();
} catch (Throwable $e) {
    $pdo->rollBack();
}
```

잔액 이체, 주문 생성처럼 **여러 레코드가 원자적**으로 변경돼야 하는 작업에 필수입니다.

## Repository 패턴

데이터 접근 로직을 인터페이스 뒤에 숨기고, 서비스는 인터페이스에만 의존합니다.

| 계층 | 역할 |
|------|------|
| `UserRepositoryInterface` | 데이터 접근 계약 (findById, create ...) |
| `PdoUserRepository` | PDO 구현체 |
| `UserService` | 비즈니스 로직 (중복 검사 등) |

테스트에서는 메모리 저장소 구현체로 교체할 수 있어 확장성이 좋아집니다.

## fetch 모드

- `FETCH_ASSOC` — 연관 배열
- `FETCH_NUM` — 숫자 배열
- `FETCH_CLASS` — 객체로 변환
- `fetchColumn()` — 단일 값 조회

## 실행

```bash
php index.php
```

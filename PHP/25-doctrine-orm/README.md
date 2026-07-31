# 25: ORM 개념 — 엔티티 매핑, 영속성 (인메모리 시뮬레이션)

## ORM (Object-Relational Mapping)

테이블을 클래스(엔티티)로, 레코드를 객체로 매핑해 SQL 없이 객체 지향 코드만으로 데이터를 다루게 해줍니다. Doctrine ORM이 대표적입니다.

## 엔티티 매핑 (PHP 8 Attribute)

PHP 8부터 속성(Attribute)으로 매핑 정보를 선언합니다.

```php
#[Entity(table: 'users')]
class User {
    #[Id, Column('id', 'integer')]
    public int $id;
    #[Column('name', 'string')]
    public string $name;
}
```

## 영속성 컨텍스트 (Persistence Context)

`EntityManager`가 엔티티의 생명주기를 관리합니다.

| 상태 | 설명 |
|------|------|
| **New (Transient)** | 아직 저장 전, 영속성 컨텍스트에 없음 |
| **Managed** | persist() 후, flush() 대상 |
| **Detached** | 컨텍스트에서 분리됨 |
| **Removed** | 삭제 예정 |

```php
$em->persist($user);   // 관리 대상에 등록
$em->flush();          // INSERT/UPDATE 실행
$em->remove($user);    // DELETE 예약
```

## Identity Map

같은 id로 여러 번 조회해도 **항상 같은 객체 인스턴스**를 반환합니다.

```php
$a = $em->find(User::class, 1);
$b = $em->find(User::class, 1);
$a === $b;  // true
```

## UnitOfWork

변경을 추적하는 내부 컴포넌트입니다. flush 시점에 변경분(dirty)을 DB에 반영합니다.

## Repository

엔티티 조회를 담당하는 계층입니다.

```php
$em->getRepository(User::class)->findAll();
```

## 실행

```bash
php index.php
```

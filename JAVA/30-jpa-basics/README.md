# 30: JPA Basics — JPA/Hibernate 기초

## ORM 이란?

객체와 관계형 DB(RDB) 사이의 불일치를 해결해 주는 매핑 기술입니다.

```java
@Entity
@Table(name = "users")
public class User {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(nullable = false)
    private String name;
}
```

## 엔티티 (Entity)

- DB 테이블에 대응하는 클래스 (`@Entity`)
- PK 를 나타내는 `@Id`
- `@GeneratedValue` 로 PK 자동 생성
- 엔티티는 **영속성 컨텍스트(Persistence Context)** 안에서 관리

## 영속성 컨텍스트

JPA 가 엔티티를 관리하는 메모리 영역 (EntityManager 가 담당) 입니다.

| 특징 | 설명 |
|------|------|
| 1차 캐시 | 같은 PK 조회 시 같은 인스턴스 반환 (Identity Map) |
| Dirty Checking | 변경 감지로 자동 UPDATE SQL 생성 |
| 쓰기 지연 | SQL 을 바로 실행하지 않고 모아두었다가 flush |
| Flush | 영속성 컨텍스트 변경을 DB에 반영 |

```java
User user = em.find(User.class, 1L);   // 1차 캐시 조회
user.setName("새이름");                 // dirty checking -> flush 시 UPDATE
em.persist(newUser);                   // INSERT 예약
em.flush();                            // SQL 반영
```

## 엔티티 생명주기

`Transient(비영속) → Managed(영속) → Detached(분리) → Removed(삭제)`

## JPQL

SQL 과 비슷하지만 **엔티티와 필드**를 대상으로 하는 쿼리 언어입니다.

```java
em.createQuery("SELECT u FROM User u WHERE u.name LIKE :name", User.class)
    .setParameter("name", "%김%")
    .getResultList();
```

## 실행

```bash
cd JAVA/30-jpa-basics
javac Main.java && java Main
```

> JPA 프레임워크 없이 영속성 컨텍스트의 동작 원리를 자바 코드로 흉내 냅니다.
> 실제 코드는 `entityManager.persist(...)` 형태이며 주석으로 함께 안내합니다.

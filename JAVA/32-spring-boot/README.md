# 32: Spring Boot REST — 스프링 부트 REST API

## Spring Boot 란?

의존성 관리와 자동 설정으로 스프링 애플리케이션을 빠르게 만드는 프레임워크입니다.

```java
@SpringBootApplication
public class Application {
    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
    }
}
```

- `spring-boot-starter-web`: 내장 톰캣 + REST 기능
- 자동 설정(Auto Configuration)으로 별도 XML 없이 실행

## REST 컨트롤러

| 어노테이션 | HTTP 매핑 |
|-----------|-----------|
| `@GetMapping` | GET (조회) |
| `@PostMapping` | POST (생성) |
| `@PutMapping` | PUT (전체 수정) |
| `@PatchMapping` | PATCH (부분 수정) |
| `@DeleteMapping` | DELETE (삭제) |

```java
@RestController
@RequestMapping("/api/users")
public class UserController {
    @GetMapping("/{id}")
    public User get(@PathVariable Long id) {
        return service.findById(id);
    }
}
```

## 계층 구조 (Layered Architecture)

| 계층 | 역할 |
|------|------|
| Controller | HTTP 요청/응답 처리 |
| Service | 비즈니스 로직, 트랜잭션 |
| Repository | DB 접근 |
| DTO | 계층 간 데이터 전달 |

## 의존성 주입 흐름

```
요청 -> Controller -> Service -> Repository -> DB
```

## 실행

```bash
cd JAVA/32-spring-boot
javac Main.java && java Main
```

> 스프링 부트 없이 컨트롤러/서비스/리포지토리 계층과 URL 라우팅을 직접 구현해 봅니다.

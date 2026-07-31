# 31: Spring Core — Spring 핵심

## IoC (Inversion of Control)

객체의 생성과 생명주기를 개발자가 아닌 **컨테이너**가 관리합니다.

```java
// 개발자가 직접 생성 (결합도 높음)
UserService service = new UserService(new UserRepository());

// 컨테이너가 주입 (Spring)
@Autowired UserService service;
```

## DI (Dependency Injection)

의존성을 외부에서 주입받아 결합도를 낮춥니다.

| 주입 방식 | 예 |
|-----------|-----|
| 생성자 주입 (권장) | `public UserService(UserRepository repo)` |
| Setter 주입 | `@Autowired setter` |
| 필드 주입 | `@Autowired private UserRepository repo;` |

## Bean 과 스프링 컨테이너

- `@Component` 계열 어노테이션으로 빈 등록
- `@Configuration` + `@Bean` 으로 수동 등록
- `ApplicationContext` 가 빈을 생성/조립/관리

```java
@Configuration
class AppConfig {
    @Bean
    UserRepository userRepository() {
        return new UserRepository();
    }
}
```

## AOP (Aspect Oriented Programming)

핵심 로직과 횡단 관심사(로깅, 트랜잭션, 보안)를 분리합니다.

```java
@Aspect
class LoggingAspect {
    @Before("execution(* service.*.*(..))")
    public void log() { ... }
}
```

- **Aspect**: 부가 기능 모듈
- **Pointcut**: 적용할 위치 지정
- **Advice**: 실제 부가 동작 (Before, After, Around)

## 실행

```bash
cd JAVA/31-spring-core
javac Main.java && java Main
```

> 스프링 없이 DI 컨테이너와 AOP 프록시의 원리를 직접 구현해 봅니다.

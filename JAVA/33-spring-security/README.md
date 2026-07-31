# 33: Spring Security — 스프링 시큐리티와 JWT

## 인증과 인가

| 용어 | 의미 |
|------|------|
| 인증 (Authentication) | "너가 누구냐" — 로그인, 신원 확인 |
| 인가 (Authorization) | "뭘 할 수 있냐" — 권한 확인 |

Spring Security 는 필터 체인으로 동작합니다.

```
요청 -> 인증 필터 -> 인가 필터 -> 컨트롤러
```

## SecurityContext

인증된 사용자 정보를 보관하는 컨텍스트입니다.

```java
Authentication auth = SecurityContextHolder.getContext().getAuthentication();
```

## JWT (JSON Web Token)

토큰 기반 인증으로, 서버에 세션을 저장하지 않는 무상태(Stateless) 방식입니다.

```
header.payload.signature
eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiIxIn0.3v2H...
```

| 부분 | 내용 |
|------|------|
| Header | 타입, 서명 알고리즘 |
| Payload | claims (sub, exp, role ...) |
| Signature | 비밀키로 서명 — 위변조 방지 |

## JWT 인증 흐름

1. 로그인 성공 -> 서버가 JWT 발급
2. 클라이언트가 이후 요청마다 `Authorization: Bearer <jwt>` 전송
3. 서버는 서명을 검증하고 사용자 정보를 복원

## Spring Security 설정 예

```java
@Bean
SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
    http.authorizeHttpRequests(auth -> auth
            .requestMatchers("/public/**").permitAll()
            .requestMatchers("/admin/**").hasRole("ADMIN")
            .anyRequest().authenticated())
        .sessionManagement(s -> s.sessionCreationPolicy(SessionCreationPolicy.STATELESS))
        .addFilterBefore(jwtFilter, UsernamePasswordAuthenticationFilter.class);
    return http.build();
}
```

## 실행

```bash
cd JAVA/33-spring-security
javac Main.java && java Main
```

> 스프링 시큐리티 없이 HMAC-SHA256 서명으로 JWT 생성/검증과
> 인증 필터 체인을 직접 구현해 봅니다.

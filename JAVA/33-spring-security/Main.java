import java.nio.charset.StandardCharsets;
import java.time.Instant;
import java.util.*;

import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;

public class Main {

    // --- 간단한 JWT 구현 (HMAC-SHA256 + Base64URL) ---

    static class Jwt {
        private static final String SECRET = "course-secret-key-1234";   // 비밀키 (운영에서는 안전하게 보관)

        static String base64Url(byte[] data) {
            return Base64.getUrlEncoder().withoutPadding().encodeToString(data);
        }

        static byte[] base64UrlDecode(String data) {
            return Base64.getUrlDecoder().decode(data);
        }

        // payload(JSON) 를 받아 서명된 JWT 생성
        static String create(String payload) throws Exception {
            String header = base64Url("{\"alg\":\"HS256\",\"typ\":\"JWT\"}".getBytes(StandardCharsets.UTF_8));
            String body = base64Url(payload.getBytes(StandardCharsets.UTF_8));
            String signature = hmac(header + "." + body);
            return header + "." + body + "." + signature;
        }

        static String hmac(String data) throws Exception {
            Mac mac = Mac.getInstance("HmacSHA256");
            mac.init(new SecretKeySpec(SECRET.getBytes(StandardCharsets.UTF_8), "HmacSHA256"));
            return base64Url(mac.doFinal(data.getBytes(StandardCharsets.UTF_8)));
        }

        // 서명 검증: 변조 여부 확인
        static boolean verify(String token) throws Exception {
            String[] parts = token.split("\\.");
            if (parts.length != 3) return false;
            String expected = hmac(parts[0] + "." + parts[1]);
            return expected.equals(parts[2]);
        }

        // 페이로드(claims) 추출
        static String payloadOf(String token) {
            String[] parts = token.split("\\.");
            return new String(base64UrlDecode(parts[1]), StandardCharsets.UTF_8);
        }
    }

    // --- 인증 필터 체인 시뮬레이션 (Spring Security FilterChain 흉내) ---
    record AuthenticatedUser(String username, String role, long exp) {}

    static class JwtAuthFilter {
        private final Map<String, AuthenticatedUser> context = new HashMap<>();  // SecurityContext 역할

        // Authorization 헤더 값으로 인증 수행
        boolean authenticate(String header) throws Exception {
            if (header == null || !header.startsWith("Bearer ")) return false;
            String token = header.substring("Bearer ".length());

            if (!Jwt.verify(token)) {
                System.out.println("  [인증 필터] 서명 검증 실패 - 토큰 거부");
                return false;
            }
            if (isExpired(token)) {
                System.out.println("  [인증 필터] 만료된 토큰 - 거부");
                return false;
            }

            String claims = Jwt.payloadOf(token);
            AuthenticatedUser user = parseUser(claims);
            context.put("authentication", user);   // SecurityContextHolder 에 저장
            System.out.println("  [인증 필터] 인증 성공: " + user.username() + " (" + user.role() + ")");
            return true;
        }

        AuthenticatedUser currentUser() { return context.get("authentication"); }

        static boolean isExpired(String token) {
            String claims = Jwt.payloadOf(token);
            return claims.contains("\"exp\"") &&
                Long.parseLong(claims.replaceAll(".*\"exp\":(\\d+).*", "$1")) < Instant.now().getEpochSecond();
        }

        static AuthenticatedUser parseUser(String claims) {
            String username = claims.replaceAll(".*\"sub\":\"([^\"]+)\".*", "$1");
            String role = claims.replaceAll(".*\"role\":\"([^\"]+)\".*", "$1");
            long exp = Long.parseLong(claims.replaceAll(".*\"exp\":(\\d+).*", "$1"));
            return new AuthenticatedUser(username, role, exp);
        }
    }

    public static void main(String[] args) throws Exception {
        System.out.println("=== JWT 생성 ===");

        long now = Instant.now().getEpochSecond();
        String payload = "{\"sub\":\"kim\",\"role\":\"ADMIN\",\"exp\":" + (now + 3600) + "}";
        String token = Jwt.create(payload);

        System.out.println("  토큰: " + token);
        String[] parts = token.split("\\.");
        System.out.println("  header    : " + parts[0]);
        System.out.println("  payload   : " + parts[1]);
        System.out.println("  signature : " + parts[2]);
        System.out.println("  페이로드  : " + Jwt.payloadOf(token));

        System.out.println("\n=== JWT 검증 ===");

        System.out.println("  서명 검증(정상 토큰): " + Jwt.verify(token));

        // 위변조 시도: payload 를 다른 사용자로 바꾸기
        String[] forged = token.split("\\.");
        String fakePayload = Jwt.base64Url(
            "{\"sub\":\"hacker\",\"role\":\"ADMIN\",\"exp\":" + (now + 3600) + "}".getBytes(StandardCharsets.UTF_8));
        String fakeToken = forged[0] + "." + fakePayload + "." + forged[2];
        System.out.println("  서명 검증(위변조 토큰): " + Jwt.verify(fakeToken) + " <- 변조 감지");

        System.out.println("\n=== 인증 필터 체인 (Spring Security 흉내) ===");

        JwtAuthFilter filter = new JwtAuthFilter();

        // 1. 정상 토큰으로 접근
        System.out.println("--- 요청 1: 정상 토큰 ---");
        if (filter.authenticate("Bearer " + token)) {
            System.out.println("  [인가] 관리자 페이지 접근: " + 
                ("ADMIN".equals(filter.currentUser().role()) ? "허용" : "거부"));
        }

        // 2. 위변조 토큰 접근
        System.out.println("--- 요청 2: 위변조 토큰 ---");
        filter.authenticate("Bearer " + fakeToken);

        // 3. 만료 토큰 접근
        System.out.println("--- 요청 3: 만료 토큰 ---");
        String expiredPayload = "{\"sub\":\"kim\",\"role\":\"USER\",\"exp\":" + (now - 100) + "}";
        filter.authenticate("Bearer " + Jwt.create(expiredPayload));

        // 4. 토큰 없는 요청
        System.out.println("--- 요청 4: 토큰 없음 ---");
        filter.authenticate(null);

        System.out.println("\n=== 실제 Spring Security 코드 형태 (주석) ===");

        /*
        // 실제 Spring Security + JWT (강의자료용 참고)
        @Configuration
        @EnableWebSecurity
        public class SecurityConfig {

            @Bean
            public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
                return http
                    .csrf(csrf -> csrf.disable())
                    .sessionManagement(s -> s.sessionCreationPolicy(SessionCreationPolicy.STATELESS))
                    .authorizeHttpRequests(auth -> auth
                        .requestMatchers("/api/login").permitAll()
                        .requestMatchers("/api/admin/**").hasRole("ADMIN")
                        .anyRequest().authenticated())
                    .addFilterBefore(jwtAuthenticationFilter, UsernamePasswordAuthenticationFilter.class)
                    .build();
            }
        }

        // JwtAuthenticationFilter 는 요청의 Authorization 헤더를 검증하고
        // SecurityContextHolder 에 Authentication 을 저장합니다.
        */
    }
}

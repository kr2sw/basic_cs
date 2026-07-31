import java.util.*;

public class Main {

    // --- 요청/응답 모델 (HTTP JSON 흉내) ---
    record Request(String path, Map<String, String> params) {}
    record Response(int status, String body) {
        static Response ok(String body) { return new Response(200, body); }
        static Response fail(String body) { return new Response(500, body); }
    }

    // --- 마이크로서비스 인터페이스 ---
    interface Service {
        String name();
        Response handle(Request req);
    }

    // 1. User Service (회원 도메인)
    static class UserService implements Service {
        private final Map<Long, String> users = new HashMap<>(Map.of(1L, "김철수", 2L, "이영희"));

        @Override public String name() { return "user-service"; }

        @Override public Response handle(Request req) {
            if (req.path().equals("/api/users")) {
                return Response.ok("회원 목록: " + users);
            }
            if (req.path().startsWith("/api/users/")) {
                long id = Long.parseLong(req.path().replace("/api/users/", ""));
                String user = users.get(id);
                return user != null
                    ? Response.ok("회원 조회: " + user)
                    : Response.fail("회원 없음 id=" + id);
            }
            return Response.fail("알 수 없는 경로: " + req.path());
        }
    }

    // 2. Order Service (주문 도메인, 별도 DB를 가짐)
    static class OrderService implements Service {
        private final List<String> orders = new ArrayList<>(List.of("커피", "도서", "키보드"));
        private volatile boolean down = false;   // 장애 시뮬레이션용

        @Override public String name() { return "order-service"; }

        @Override public Response handle(Request req) {
            if (down) throw new IllegalStateException("order-service 다운!");
            if (req.path().equals("/api/orders")) {
                return Response.ok("주문 목록: " + orders);
            }
            return Response.fail("알 수 없는 경로");
        }
    }

    // --- 서비스 레지스트리 (Eureka 흉내) ---
    static class ServiceRegistry {
        private final Map<String, List<Service>> services = new HashMap<>();

        void register(Service service) {
            services.computeIfAbsent(service.name(), k -> new ArrayList<>()).add(service);
            System.out.println("  [레지스트리] 등록: " + service.name() + " (" + instances(service.name()) + "개 인스턴스)");
        }

        int instances(String name) { return services.getOrDefault(name, List.of()).size(); }

        // 인스턴스 반환 (간단한 라운드로빈)
        Service discover(String name) {
            List<Service> list = services.get(name);
            if (list == null || list.isEmpty()) throw new NoSuchElementException("서비스 없음: " + name);
            return list.get(roundRobin.getAndIncrement() % list.size());
        }

        java.util.concurrent.atomic.AtomicInteger roundRobin = new java.util.concurrent.atomic.AtomicInteger();
    }

    // --- 게이트웨이 (라우팅 + 서킷 브레이커) ---
    static class Gateway {
        private final ServiceRegistry registry;
        private final Map<String, Integer> failures = new HashMap<>();
        private final Map<String, Boolean> circuitOpen = new HashMap<>();

        Gateway(ServiceRegistry registry) { this.registry = registry; }

        // "GET http://user-service/api/users" 형태의 호출을 라우팅
        Response call(String target) {
            String[] parts = target.split(" ", 2);   // [GET, http://user-service/api/users]
            String url = parts[1].replace("http://", "");
            String serviceName = url.substring(0, url.indexOf('/'));
            String path = url.substring(url.indexOf('/'));

            if (Boolean.TRUE.equals(circuitOpen.get(serviceName))) {
                System.out.println("  [서킷 브레이커] " + serviceName + " 회로 열림(OPEN) - 빠른 실패");
                return Response.fail("서킷 브레이커: " + serviceName + " 사용 불가");
            }

            try {
                Service service = registry.discover(serviceName);   // 디스커버리
                Response resp = service.handle(new Request(path, Map.of()));
                failures.put(serviceName, 0);                       // 성공 시 실패 횟수 초기화
                if (circuitOpen.containsKey(serviceName)) {
                    circuitOpen.put(serviceName, false);
                    System.out.println("  [서킷 브레이커] " + serviceName + " 회로 닫힘(CLOSED)");
                }
                return resp;
            } catch (Exception e) {
                int fail = failures.merge(serviceName, 1, Integer::sum);
                System.out.println("  [게이트웨이] " + serviceName + " 호출 실패 (" + fail + "회 연속 실패)");
                if (fail >= 3) {
                    circuitOpen.put(serviceName, true);
                    System.out.println("  [서킷 브레이커] " + serviceName + " 회로 열림! (연속 3회 실패)");
                }
                return Response.fail("호출 실패: " + e.getMessage());
            }
        }
    }

    public static void main(String[] args) {
        System.out.println("=== 서비스 등록 (레지스트리) ===");

        ServiceRegistry registry = new ServiceRegistry();
        registry.register(new UserService());
        OrderService orderService = new OrderService();
        registry.register(orderService);

        Gateway gateway = new Gateway(registry);

        System.out.println("\n=== 게이트웨이 라우팅 (REST 호출 흉내) ===");

        System.out.println("  GET http://user-service/api/users/1  -> " +
            gateway.call("GET http://user-service/api/users/1"));
        System.out.println("  GET http://user-service/api/users   -> " +
            gateway.call("GET http://user-service/api/users"));
        System.out.println("  GET http://order-service/api/orders -> " +
            gateway.call("GET http://order-service/api/orders"));

        System.out.println("\n=== 서비스 장애와 서킷 브레이커 ===");

        // OrderService 다운 시뮬레이션 (3회 연속 실패로 회로 개방)
        orderService.down = true;

        gateway.call("GET http://order-service/api/orders");   // 1회 실패
        gateway.call("GET http://order-service/api/orders");   // 2회 실패
        gateway.call("GET http://order-service/api/orders");   // 3회 실패 -> OPEN
        gateway.call("GET http://order-service/api/orders");   // OPEN 상태라 빠른 실패
        gateway.call("GET http://order-service/api/orders");   // OPEN 상태라 빠른 실패

        System.out.println("\n=== 장애 복구 ===");

        orderService.down = false;   // 서비스 복구 (실제로는 half-open 시도 후 복구)
        gateway.call("GET http://order-service/api/orders");

        System.out.println("\n=== 서비스 분리 확인 ===");

        System.out.println("  각 서비스는 독립된 DB/데이터를 가짐:");
        System.out.println("  user-service  의 데이터: 김철수, 이영희");
        System.out.println("  order-service 의 데이터: 커피, 도서, 키보드");
        System.out.println("  user-service 장애가 order-service 에 영향 주지 않음 (독립 배포)");

        System.out.println("\n=== 실제 마이크로서비스 코드 형태 (주석) ===");

        /*
        // 실제 Spring Cloud (강의자료용 참고)
        @RestController
        @RequestMapping("/api/users")
        public class UserController {
            // @FeignClient / WebClient 로 다른 서비스 호출
            @GetMapping("/{id}/orders")
            public List<Order> getUserOrders(@PathVariable Long id) {
                return orderClient.getOrdersByUserId(id);  // HTTP REST 통신
            }
        }

        // Eureka 에 서비스 등록
        spring.application.name=user-service
        eureka.client.service-url.defaultZone=http://localhost:8761/eureka
        */
    }
}

import java.lang.annotation.*;
import java.lang.reflect.*;
import java.util.*;
import java.util.regex.*;

public class Main {

    // --- 스프링 MVC 어노테이션을 흉내 낸 메타 어노테이션 ---
    @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.TYPE)
    @interface RestController {}

    @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.METHOD)
    @interface GetMapping { String value(); }

    @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.METHOD)
    @interface PostMapping { String value(); }

    @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.METHOD)
    @interface DeleteMapping { String value(); }

    @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.PARAMETER)
    @interface PathVariable { String value(); }

    // --- 도메인 ---
    record User(Long id, String name, int age) {}

    // --- Repository 계층 (DB 접근) ---
    static class UserRepository {
        private final Map<Long, User> db = new HashMap<>();
        private long seq = 1;

        UserRepository() {
            db.put(seq, new User(seq++, "김철수", 30));
            db.put(seq, new User(seq++, "이영희", 28));
        }

        List<User> findAll() { return new ArrayList<>(db.values()); }
        User findById(Long id) { return db.get(id); }
        User save(User user) {
            User saved = new User(seq++, user.name(), user.age());
            db.put(saved.id(), saved);
            return saved;
        }
        boolean delete(Long id) { return db.remove(id) != null; }
    }

    // --- Service 계층 (비즈니스 로직) ---
    static class UserService {
        private final UserRepository repo = new UserRepository();

        List<User> getAll() { return repo.findAll(); }
        User getById(Long id) {
            User user = repo.findById(id);
            if (user == null) throw new NoSuchElementException("사용자 없음: " + id);
            return user;
        }
        User create(String name, int age) { return repo.save(new User(null, name, age)); }
        boolean delete(Long id) { return repo.delete(id); }
    }

    // --- Controller 계층 (HTTP 요청 처리) ---
    @RestController
    static class UserController {
        private final UserService service = new UserService();

        @GetMapping("/api/users")
        List<User> list() { return service.getAll(); }

        @GetMapping("/api/users/{id}")
        User detail(@PathVariable("id") Long id) { return service.getById(id); }

        @PostMapping("/api/users")
        User create(@PathVariable("name") String name, @PathVariable("age") int age) {
            return service.create(name, age);
        }

        @DeleteMapping("/api/users/{id}")
        String remove(@PathVariable("id") Long id) {
            return service.delete(id) ? "삭제 성공" : "삭제 실패";
        }
    }

    // --- 미니 디스패처: 어노테이션을 읽어 URL 라우팅 ---
    static class MiniDispatcher {
        static class Route {
            final String method, pattern;
            final Method handler;
            final Object controller;
            final List<String> varNames;
            final Pattern regex;
            Route(String method, String pattern, Method handler, Object controller, List<String> varNames) {
                this.method = method;
                this.pattern = pattern;
                this.handler = handler;
                this.controller = controller;
                this.varNames = varNames;
                this.regex = Pattern.compile("^" + pattern.replaceAll("\\{\\w+\\}", "([^/]+)") + "$");
            }
        }

        private final List<Route> routes = new ArrayList<>();

        // 컨트롤러 클래스를 스캔해 라우트 등록 (스프링 RequestMappingHandlerMapping 흉내)
        MiniDispatcher(Object... controllers) throws Exception {
            for (Object controller : controllers) {
                Class<?> cls = controller.getClass();
                if (!cls.isAnnotationPresent(RestController.class)) continue;
                for (Method m : cls.getDeclaredMethods()) {
                    String method = null, path = null;
                    if (m.isAnnotationPresent(GetMapping.class))   { method = "GET";    path = m.getAnnotation(GetMapping.class).value(); }
                    if (m.isAnnotationPresent(PostMapping.class))  { method = "POST";   path = m.getAnnotation(PostMapping.class).value(); }
                    if (m.isAnnotationPresent(DeleteMapping.class)){ method = "DELETE"; path = m.getAnnotation(DeleteMapping.class).value(); }
                    if (method == null) continue;

                    List<String> vars = new ArrayList<>();
                    Matcher matcher = Pattern.compile("\\{(\\w+)\\}").matcher(path);
                    while (matcher.find()) vars.add(matcher.group(1));

                    routes.add(new Route(method, path, m, controller, vars));
                    System.out.println("  라우트 등록: " + method + " " + path);
                }
            }
        }

        // 요청 시뮬레이션: (HTTP 메서드, 경로, 파라미터) -> 응답
        Object dispatch(String method, String path, Object... args) throws Exception {
            for (Route route : routes) {
                if (!route.method.equals(method)) continue;
                Matcher matcher = route.regex.matcher(path);
                if (!matcher.matches()) continue;

                Object[] params = new Object[route.handler.getParameterCount()];
                int p = 0;
                for (Parameter parameter : route.handler.getParameters()) {
                    PathVariable pv = parameter.getAnnotation(PathVariable.class);
                    if (pv != null) {
                        int idx = route.varNames.indexOf(pv.value());
                        String raw = matcher.group(idx + 1);
                        params[p] = convert(raw, parameter.getType());
                    } else {
                        params[p] = args.length > p ? args[p] : null;
                    }
                    p++;
                }
                return route.handler.invoke(route.controller, params);
            }
            throw new NoSuchElementException("404 Not Found: " + method + " " + path);
        }

        static Object convert(String raw, Class<?> type) {
            if (type == Long.class || type == long.class) return Long.parseLong(raw);
            if (type == Integer.class || type == int.class) return Integer.parseInt(raw);
            return raw;
        }
    }

    public static void main(String[] args) throws Exception {
        System.out.println("=== 계층 구조: Controller -> Service -> Repository ===");

        MiniDispatcher dispatcher = new MiniDispatcher(new UserController());

        System.out.println("\n=== REST 요청 시뮬레이션 ===");

        System.out.println("GET /api/users          -> " + dispatcher.dispatch("GET", "/api/users"));
        System.out.println("GET /api/users/1        -> " + dispatcher.dispatch("GET", "/api/users/1"));
        System.out.println("GET /api/users/2        -> " + dispatcher.dispatch("GET", "/api/users/2"));
        System.out.println("POST /api/users/박민준/35 -> " + dispatcher.dispatch("POST", "/api/users/박민준/35"));
        System.out.println("DELETE /api/users/1     -> " + dispatcher.dispatch("DELETE", "/api/users/1"));
        System.out.println("GET /api/users (목록)   -> " + dispatcher.dispatch("GET", "/api/users"));

        try {
            dispatcher.dispatch("GET", "/api/users/999");
        } catch (NoSuchElementException e) {
            System.out.println("존재하지 않는 id: " + e.getMessage());
        }

        System.out.println("\n=== 실제 Spring Boot 코드 형태 (주석) ===");

        /*
        // 실제 Spring Boot REST API (강의자료용 참고)
        @RestController
        @RequestMapping("/api/users")
        public class UserController {

            @GetMapping
            public List<User> list() {
                return userService.getAll();
            }

            @GetMapping("/{id}")
            public User detail(@PathVariable Long id) {
                return userService.getById(id);
            }

            @PostMapping
            public User create(@RequestBody UserCreateRequest req) {
                return userService.create(req.name(), req.age());
            }
        }
        */
    }
}

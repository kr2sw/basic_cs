import java.lang.annotation.*;
import java.lang.reflect.*;
import java.util.*;

public class Main {

    // --- 스프링 어노테이션을 흉내 낸 메타 어노테이션 ---
    @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.TYPE)
    @interface Component {}

    @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.TYPE)
    @interface Service {}

    @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.TYPE)
    @interface Repository {}

    @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.FIELD)
    @interface Autowired {}

    // --- 빈(Bean) 정의 ---
    record User(String name, int age) {}

    interface UserRepository {
        List<User> findAll();
        User save(User user);
    }

    @Repository
    static class MemoryUserRepository implements UserRepository {
        private final List<User> db = new ArrayList<>(List.of(new User("김철수", 30)));

        @Override public List<User> findAll() { return new ArrayList<>(db); }
        @Override public User save(User user) { db.add(user); return user; }
    }

    @Service
    static class UserService {
        @Autowired
        UserRepository repo;   // 컨테이너가 인터페이스 타입으로 주입

        List<User> all() { return repo.findAll(); }
        User create(String name, int age) { return repo.save(new User(name, age)); }
    }

    // --- 미니 DI 컨테이너 (리플렉션으로 생성 + 주입) ---
    static class MiniContainer {
        private final Map<Class<?>, Object> beans = new HashMap<>();

        // 등록된 클래스들을 인스턴스화하고 @Autowired 필드에 의존성 주입
        MiniContainer(Class<?>... classes) throws Exception {
            for (Class<?> cls : classes) {
                if (cls.isAnnotationPresent(Component.class) ||
                    cls.isAnnotationPresent(Service.class) ||
                    cls.isAnnotationPresent(Repository.class)) {
                    Object bean = cls.getDeclaredConstructor().newInstance();
                    beans.put(cls, bean);
                    // 인터페이스 타입으로도 조회 가능하게 등록
                    for (Class<?> iface : cls.getInterfaces()) {
                        beans.put(iface, bean);
                    }
                }
            }
            // 의존성 주입
            for (Object bean : beans.values()) {
                for (Field f : bean.getClass().getDeclaredFields()) {
                    if (f.isAnnotationPresent(Autowired.class)) {
                        f.setAccessible(true);
                        Object dependency = beans.get(f.getType());
                        if (dependency != null) {
                            f.set(bean, dependency);
                            System.out.println("  [DI] " + bean.getClass().getSimpleName()
                                + " <- " + f.getType().getSimpleName() + " 주입");
                        }
                    }
                }
            }
        }

        <T> T getBean(Class<T> type) { return type.cast(beans.get(type)); }
    }

    public static void main(String[] args) throws Exception {
        System.out.println("=== IoC: 미니 DI 컨테이너 ===");

        MiniContainer container = new MiniContainer(MemoryUserRepository.class, UserService.class);

        UserService service = container.getBean(UserService.class);
        System.out.println("  UserService 주입 확인: " + service.repo.getClass().getSimpleName());

        System.out.println("\n=== DI 를 통한 서비스 사용 ===");

        System.out.println("  전체 사용자: " + service.all());
        service.create("이영희", 28);
        System.out.println("  등록 후 사용자: " + service.all());

        System.out.println("\n=== AOP: 프록시 기반 로깅 어드바이스 ===");

        // UserRepository 인터페이스를 감싸 호출 로그를 남기는 프록시 (스프링 AOP 흉내)
        Object proxy = Proxy.newProxyInstance(
            UserRepository.class.getClassLoader(),
            new Class<?>[]{UserRepository.class},
            (p, method, args2) -> {
                System.out.println("  [Around] " + method.getName() + " 호출 전");
                Object result = method.invoke(service.repo, args2);
                System.out.println("  [Around] " + method.getName() + " 호출 후, 반환=" + result);
                return result;
            });

        UserRepository loggedRepo = (UserRepository) proxy;
        loggedRepo.findAll();

        System.out.println("\n=== 실제 스프링 코드 형태 (주석) ===");

        /*
        // 실제 Spring Boot 코드 (강의자료용 참고)
        @Service
        public class UserService {
            private final UserRepository userRepository;   // 생성자 주입

            public UserService(UserRepository userRepository) {
                this.userRepository = userRepository;
            }

            @Transactional
            public List<User> findAll() {
                return userRepository.findAll();
            }
        }

        // ApplicationContext 에서 빈 꺼내 쓰기
        ApplicationContext ctx = new AnnotationConfigApplicationContext(AppConfig.class);
        UserService service = ctx.getBean(UserService.class);
        */
    }
}

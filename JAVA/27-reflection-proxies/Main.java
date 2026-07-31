import java.lang.reflect.*;
import java.util.*;

public class Main {

    // 리플렉션으로 분석할 대상 클래스
    static class UserService {
        public String name = "김철수";
        private String secret = "비밀값";

        public String greet(String who) {
            return "안녕하세요, " + who + "님";
        }

        private int add(int a, int b) {
            return a + b;
        }
    }

    // 프록시로 감쌀 비즈니스 인터페이스
    interface OrderService {
        String placeOrder(String item, int qty);
    }

    static class RealOrderService implements OrderService {
        @Override
        public String placeOrder(String item, int qty) {
            return "주문 완료: " + item + " x " + qty;
        }
    }

    public static void main(String[] args) throws Exception {
        System.out.println("=== 클래스 정보 조회 ===");

        Class<?> cls = UserService.class;
        System.out.println("클래스 이름: " + cls.getName());
        System.out.println("패키지: " + cls.getPackageName());
        System.out.println("수퍼클래스: " + cls.getSuperclass().getSimpleName());

        System.out.println("\n=== 메서드/필드 조회 ===");

        System.out.println("public 메서드 목록:");
        for (Method m : cls.getDeclaredMethods()) {
            System.out.println("  " + Modifier.toString(m.getModifiers()) + " " + m.getName() + "()");
        }
        System.out.println("필드 목록:");
        for (Field f : cls.getDeclaredFields()) {
            System.out.println("  " + Modifier.toString(f.getModifiers()) + " " + f.getName());
        }

        System.out.println("\n=== 동적 메서드 호출 ===");

        UserService service = new UserService();

        // public 메서드 동적 호출
        Method greet = cls.getMethod("greet", String.class);
        Object result = greet.invoke(service, "이영희");
        System.out.println("invoke(greet): " + result);

        // private 메서드에 setAccessible 로 접근
        Method add = cls.getDeclaredMethod("add", int.class, int.class);
        add.setAccessible(true);
        System.out.println("invoke(private add): " + add.invoke(service, 10, 20));

        System.out.println("\n=== private 필드 읽기/쓰기 ===");

        Field secret = cls.getDeclaredField("secret");
        secret.setAccessible(true);
        System.out.println("private 필드 읽기: " + secret.get(service));
        secret.set(service, "수정된 값");
        System.out.println("private 필드 수정 후: " + secret.get(service));

        System.out.println("\n=== 동적 프록시 (InvocationHandler) ===");

        RealOrderService target = new RealOrderService();

        // 호출 기록을 남기는 로깅 프록시
        InvocationHandler handler = (proxy, method, args) -> {
            System.out.println("  [프록시] 호출 메서드: " + method.getName() + ", 인자: " + Arrays.toString(args));
            long start = System.nanoTime();
            Object returnValue = method.invoke(target, args);   // 실제 대상 호출
            System.out.println("  [프록시] 반환: " + returnValue +
                ", 소요: " + (System.nanoTime() - start) / 1_000_000 + "ms");
            return returnValue;
        };

        OrderService proxy = (OrderService) Proxy.newProxyInstance(
            OrderService.class.getClassLoader(),
            new Class<?>[]{OrderService.class},
            handler);

        System.out.println("프록시 클래스: " + proxy.getClass().getName());
        String order = proxy.placeOrder("커피", 2);
        System.out.println("호출 결과: " + order);

        System.out.println("\n=== 프록시 활용: 캐싱 ===");

        // 결과를 캐싱하는 프록시
        Map<String, Object> cache = new HashMap<>();
        InvocationHandler caching = (proxy, method, args) -> {
            String key = method.getName() + Arrays.toString(args);
            if (cache.containsKey(key)) {
                System.out.println("  [캐시] " + key + " -> 캐시에서 반환");
                return cache.get(key);
            }
            Object value = method.invoke(target, args);
            cache.put(key, value);
            return value;
        };
        OrderService cachedProxy = (OrderService) Proxy.newProxyInstance(
            OrderService.class.getClassLoader(),
            new Class<?>[]{OrderService.class},
            caching);

        cachedProxy.placeOrder("빵", 3);
        cachedProxy.placeOrder("빵", 3);   // 두 번째 호출은 캐시 사용
        cachedProxy.placeOrder("빵", 5);
    }
}

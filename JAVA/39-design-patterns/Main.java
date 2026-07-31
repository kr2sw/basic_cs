import java.util.*;
import java.util.function.*;

public class Main {

    // ================= 싱글턴 =================

    // 1. enum 기반 싱글턴 (스레드 안전)
    enum Config {
        INSTANCE;

        private final String appName = "디자인패턴 예제";

        String appName() { return appName; }
    }

    // 2. 이중 검사 잠금(DCL) 싱글턴
    static class Logger {
        private static volatile Logger instance;

        private Logger() {}

        static Logger getInstance() {
            if (instance == null) {            // 1차 체크
                synchronized (Logger.class) {
                    if (instance == null) {    // 2차 체크
                        instance = new Logger();
                    }
                }
            }
            return instance;
        }

        void log(String msg) { System.out.println("  [로그] " + msg); }
    }

    // ================= 팩토리 =================

    interface Shape {
        double area();
    }

    static class Circle implements Shape {
        private final double r;
        Circle(double r) { this.r = r; }
        @Override public double area() { return Math.PI * r * r; }
        @Override public String toString() { return "원(r=" + r + ")"; }
    }

    static class Rectangle implements Shape {
        private final double w, h;
        Rectangle(double w, double h) { this.w = w; this.h = h; }
        @Override public double area() { return w * h; }
        @Override public String toString() { return "사각형(" + w + "x" + h + ")"; }
    }

    // 객체 생성 로직을 한 곳에 모은 팩토리
    static class ShapeFactory {
        static Shape create(String type, double... dims) {
            return switch (type) {
                case "circle" -> new Circle(dims[0]);
                case "rect" -> new Rectangle(dims[0], dims[1]);
                default -> throw new IllegalArgumentException("지원하지 않는 도형: " + type);
            };
        }
    }

    // ================= 전략 =================

    interface DiscountStrategy {
        double discount(double price);
    }

    static class NoDiscount implements DiscountStrategy {
        @Override public double discount(double price) { return price; }
    }

    static class PercentDiscount implements DiscountStrategy {
        private final double percent;
        PercentDiscount(double percent) { this.percent = percent; }
        @Override public double discount(double price) { return price * (1 - percent); }
    }

    static class FixedDiscount implements DiscountStrategy {
        private final double amount;
        FixedDiscount(double amount) { this.amount = amount; }
        @Override public double discount(double price) { return Math.max(0, price - amount); }
    }

    static class PriceCalculator {
        private DiscountStrategy strategy;

        PriceCalculator(DiscountStrategy strategy) { this.strategy = strategy; }

        // 전략을 런타임에 교체
        void setStrategy(DiscountStrategy strategy) { this.strategy = strategy; }

        double calculate(double price) { return strategy.discount(price); }
    }

    // ================= 옵저버 =================

    interface NewsListener {
        void onNews(String news);
    }

    static class EmailListener implements NewsListener {
        private final String email;
        EmailListener(String email) { this.email = email; }
        @Override public void onNews(String news) {
            System.out.println("  [이메일 알림] " + email + " <- " + news);
        }
    }

    static class SmsListener implements NewsListener {
        private final String phone;
        SmsListener(String phone) { this.phone = phone; }
        @Override public void onNews(String news) {
            System.out.println("  [SMS 알림] " + phone + " <- " + news);
        }
    }

    static class NewsPublisher {
        private final List<NewsListener> listeners = new ArrayList<>();

        void register(NewsListener listener) { listeners.add(listener); }
        void unregister(NewsListener listener) { listeners.remove(listener); }

        void publish(String news) {
            System.out.println("  [발행] 뉴스: " + news);
            listeners.forEach(l -> l.onNews(news));   // 등록된 구독자에게 알림
        }
    }

    public static void main(String[] args) {
        System.out.println("=== 1. 싱글턴 ===");

        Config a = Config.INSTANCE;
        Config b = Config.INSTANCE;
        System.out.println("  enum 싱글턴 같은 인스턴스? " + (a == b) + ", 앱 이름: " + a.appName());

        Logger l1 = Logger.getInstance();
        Logger l2 = Logger.getInstance();
        System.out.println("  DCL 싱글턴 같은 인스턴스? " + (l1 == l2));
        l1.log("동작 확인");

        System.out.println("\n=== 2. 팩토리 ===");

        // 호출자는 구체 클래스가 아닌 인터페이스만 사용
        Shape circle = ShapeFactory.create("circle", 2);
        Shape rect = ShapeFactory.create("rect", 3, 4);
        System.out.println("  " + circle + " 넓이=" + circle.area());
        System.out.println("  " + rect + " 넓이=" + rect.area());

        System.out.println("\n=== 3. 전략 ===");

        // 런타임에 전략 교체로 할인 규칙 변경
        double price = 10_000;
        PriceCalculator no = new PriceCalculator(new NoDiscount());
        PriceCalculator percent = new PriceCalculator(new PercentDiscount(0.20));
        PriceCalculator fixed = new PriceCalculator(new FixedDiscount(3_000));
        System.out.println("  정가: " + price);
        System.out.println("  할인 없음     : " + no.calculate(price));
        System.out.println("  20% 할인     : " + percent.calculate(price));
        System.out.println("  3000원 할인  : " + fixed.calculate(price));

        // 전략 런타임 교체: 같은 객체로 할인 규칙 변경
        PriceCalculator switchable = new PriceCalculator(new NoDiscount());
        switchable.setStrategy(new PercentDiscount(0.10));
        System.out.println("  교체 후 10% 할인: " + switchable.calculate(price));
        switchable.setStrategy(new FixedDiscount(1_000));
        System.out.println("  교체 후 1000원 할인: " + switchable.calculate(price));

        // 람다로 즉석 전략 정의
        DiscountStrategy lambdaStrategy = p -> p * 0.5;
        System.out.println("  람다 50% 할인  : " + new PriceCalculator(lambdaStrategy).calculate(price));

        System.out.println("\n=== 4. 옵저버 ===");

        NewsPublisher publisher = new NewsPublisher();
        NewsListener email = new EmailListener("user@example.com");
        NewsListener sms = new SmsListener("010-1234-5678");

        publisher.register(email);
        publisher.register(sms);
        publisher.publish("자바 과정 40챕터 완료!");

        publisher.unregister(email);
        publisher.publish("해지된 구독자는 알림을 받지 않습니다.");

        System.out.println("\n=== 실제 프레임워크에서의 사용 (주석) ===");

        /*
        // Spring: 컴포넌트 등록 + 전략 주입 + 이벤트 리스너
        @Component
        public class OrderService {
            @Autowired
            private List<PaymentStrategy> strategies;  // 전략 패턴

            public void complete() {
                eventPublisher.publishEvent(new OrderCompletedEvent(order));  // 옵저버
            }
        }
        */
    }
}

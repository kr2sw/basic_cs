import java.util.*;
import java.util.function.*;

public class Main {

    public static void main(String[] args) {
        System.out.println("=== Optional 심화 ===");

        // ofNullable + map + filter + orElse 체이닝
        String input = "  Hello Java  ";
        Optional<String> opt = Optional.ofNullable(input);

        String result = opt
            .map(String::trim)
            .map(String::toUpperCase)
            .filter(s -> s.contains("JAVA"))
            .orElse("기본값");
        System.out.println("변환 결과: " + result);

        // empty() 인 경우의 안전한 처리
        String safe = Optional.<String>empty()
            .orElseGet(() -> "공급자(Supplier)가 만든 기본값");
        System.out.println("빈 Optional: " + safe);

        // orElseThrow
        try {
            Optional.ofNullable(null).orElseThrow(() -> new IllegalStateException("값이 없음!"));
        } catch (IllegalStateException e) {
            System.out.println("orElseThrow 예외: " + e.getMessage());
        }

        // ifPresent + or
        Optional.of("존재").ifPresent(v -> System.out.println("ifPresent: " + v));
        String orResult = Optional.<String>empty()
            .or(() -> Optional.of("대체 Optional"))
            .get();
        System.out.println("or(): " + orResult);

        System.out.println("\n=== Supplier / Consumer ===");

        // Supplier: 지연 계산 (비용이 큰 연산을 필요할 때만)
        Supplier<Double> randomSupplier = () -> Math.random() * 100;
        System.out.println("Supplier 1회 호출: " + randomSupplier.get());

        // Consumer: 데이터 소비
        Consumer<String> printer = System.out::println;
        Consumer<String> collector = s -> System.out.println("  [수집] " + s.toUpperCase());
        printer.andThen(collector).accept("consumer");

        System.out.println("\n=== BiFunction ===");

        // 두 인자를 받아 하나를 반환
        BiFunction<Integer, Integer, Integer> max = Math::max;
        System.out.println("BiFunction max(3, 7): " + max.apply(3, 7));

        BiFunction<String, String, String> greeting = (name, role) ->
            role + "님, " + name + "님 환영합니다!";
        System.out.println(greeting.apply("김철수", "관리자"));

        // BiFunction으로 Map 병합
        Map<String, Integer> a = new HashMap<>(Map.of("x", 10, "y", 20));
        Map<String, Integer> b = Map.of("y", 5, "z", 30);
        Map<String, Integer> merged = new HashMap<>(a);
        b.forEach((k, v) -> merged.merge(k, v, Integer::sum));
        System.out.println("Map 병합 결과: " + merged);

        System.out.println("\n=== 커링 (Currying) ===");

        // 커링: (x, y) -> x + y 를 x부터 적용하는 함수로 분리
        Function<Integer, Function<Integer, Integer>> curriedAdd =
            x -> y -> x + y;
        System.out.println("curriedAdd(3)(4): " + curriedAdd.apply(3).apply(4));

        // 부분 적용: 5를 먼저 고정한 함수 재사용
        Function<Integer, Integer> addFive = curriedAdd.apply(5);
        System.out.println("addFive(10): " + addFive.apply(10));
        System.out.println("addFive(100): " + addFive.apply(100));

        // 커링으로 할인율 계산기 만들기
        Function<Double, Function<Double, Double>> discount = rate -> price -> price * (1 - rate);
        Function<Double, Double> tenPercentOff = discount.apply(0.10);
        System.out.println("10% 할인된 가격 (50000): " + tenPercentOff.apply(50_000.0));

        System.out.println("\n=== Function 합성 ===");

        Function<Integer, Integer> doubleIt = x -> x * 2;
        Function<Integer, Integer> plusOne = x -> x + 1;
        System.out.println("double.andThen(plus)(3): " + doubleIt.andThen(plusOne).apply(3));
        System.out.println("double.compose(plus)(3): " + doubleIt.compose(plusOne).apply(3));

        System.out.println("\n=== 함수형 파이프라인 ===");

        // 여러 함수를 조합해 변환 파이프라인 구성
        List<Integer> prices = List.of(1000, 2500, 4800, 9900);
        Function<Integer, Integer> applyTax = p -> (int) (p * 1.1);
        Function<Integer, Integer> applySale = p -> (int) (p * 0.8);
        Function<Integer, Integer> pipeline = applySale.andThen(applyTax);

        prices.stream()
            .map(pipeline)
            .forEach(p -> System.out.print(p + " "));
        System.out.println();
    }
}

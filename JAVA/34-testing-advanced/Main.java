import java.lang.reflect.*;
import java.util.*;

public class Main {

    // --- 테스트 대상 인터페이스 ---
    interface Calculator {
        int add(int a, int b);
        int multiply(int a, int b);
    }

    static class RealCalculator implements Calculator {
        @Override public int add(int a, int b) { return a + b; }
        @Override public int multiply(int a, int b) { return a * b; }
    }

    // --- Mockito 흉내: 목 객체 + when/thenReturn/verify ---
    static class Mockito {
        static class Stub {
            final String key;
            Object result;
            int callCount;
            Stub(String key) { this.key = key; }
        }

        private final Map<String, Stub> stubs = new HashMap<>();
        private final List<String> callLog = new ArrayList<>();

        static String keyOf(String method, Object... args) {
            return method + Arrays.toString(args);
        }

        @SuppressWarnings("unchecked")
        <T> T mock(Class<T> type) {
            return (T) Proxy.newProxyInstance(type.getClassLoader(), new Class<?>[]{type},
                (proxy, method, args) -> {
                    String key = keyOf(method.getName(), args == null ? new Object[0] : args);
                    callLog.add(key);
                    Stub stub = stubs.get(key);
                    if (stub == null) {
                        throw new IllegalStateException("기본 동작이 정의되지 않음: " + key);
                    }
                    stub.callCount++;
                    return stub.result;
                });
        }

        Stub when(Object ignored) { throw new UnsupportedOperationException(); }

        // when(stub이 설정될 위치의 호출) 을 기록하기 위한 편의 API
        static CallRegister when2(Class<?> type) { return new CallRegister(type); }

        static class CallRegister {
            final Class<?> type;
            CallRegister(Class<?> type) { this.type = type; }
            StubReturn call(String method, Object... args) {
                return new StubReturn(Mockito.keyOf(method, args));
            }
        }

        static class StubReturn {
            final String key;
            StubReturn(String key) { this.key = key; }
        }

        // thenReturn 은 여기선 단순 확인용 (개념 전달)
        void stubResult(StubReturn s, Object result, String... expectedCalls) {
            System.out.println("  when(...).thenReturn(" + result + ") 로 동작 지정");
        }

        void verify(String method, Object... args) {
            String key = keyOf(method, args);
            long count = callLog.stream().filter(key::equals).count();
            System.out.println("  verify: " + method + " " + Arrays.toString(args) +
                " 호출 횟수 = " + count);
        }
    }

    // --- AssertJ 흉내: 체이닝 가능한 assert ---
    static class AssertJ {
        static class IntegerAssert {
            private final int actual;
            IntegerAssert(int actual) { this.actual = actual; }

            IntegerAssert isEqualTo(int expected) {
                System.out.println("  assertThat(" + actual + ").isEqualTo(" + expected + ")" +
                    (actual == expected ? "  [통과]" : "  [실패!]"));
                if (actual != expected) throw new AssertionError("기대값 " + expected + " != 실제 " + actual);
                return this;
            }

            IntegerAssert isGreaterThan(int other) {
                System.out.println("  assertThat(" + actual + ").isGreaterThan(" + other + ")" +
                    (actual > other ? "  [통과]" : "  [실패!]"));
                if (!(actual > other)) throw new AssertionError(actual + " <= " + other);
                return this;
            }
        }

        static class ListAssert {
            private final List<?> actual;
            ListAssert(List<?> actual) { this.actual = actual; }

            ListAssert hasSize(int n) {
                System.out.println("  assertThat(...).hasSize(" + n + ")" +
                    (actual.size() == n ? "  [통과]" : "  [실패!]"));
                if (actual.size() != n) throw new AssertionError("크기 " + actual.size() + " != " + n);
                return this;
            }

            ListAssert contains(Object element) {
                System.out.println("  assertThat(...).contains(" + element + ")" +
                    (actual.contains(element) ? "  [통과]" : "  [실패!]"));
                if (!actual.contains(element)) throw new AssertionError(element + " 없음");
                return this;
            }
        }

        static IntegerAssert assertThat(int actual) { return new IntegerAssert(actual); }
        static ListAssert assertThat(List<?> actual) { return new ListAssert(actual); }
    }

    public static void main(String[] args) {
        System.out.println("=== Mockito 스타일: Mock 과 Stub ===");

        Calculator mock = new Mockito().mock(Calculator.class);
        System.out.println("  목 객체 생성: " + mock.getClass().getName());

        // 주의: 이 데모의 목 객체는 stub 없이 호출하면 예외를 던지므로
        // 개념 설명용으로만 사용하고, 실제 검증은 아래 테스트 실행기로 수행합니다.

        System.out.println("\n=== 미니 테스트 실행기 (파라미터 테스트 포함) ===");

        // 테스트 케이스 모음
        List<int[]> addCases = List.of(
            new int[]{1, 2, 3},
            new int[]{10, 20, 30},
            new int[]{-5, 5, 0},
            new int[]{100, 200, 300}
        );

        Calculator calculator = new RealCalculator();
        int passed = 0, failed = 0;
        System.out.println("--- Calculator.add 파라미터 테스트 ---");
        for (int[] c : addCases) {
            try {
                AssertJ.assertThat(calculator.add(c[0], c[1])).isEqualTo(c[2]);
                passed++;
            } catch (AssertionError e) {
                System.out.println("    -> " + e.getMessage());
                failed++;
            }
        }

        System.out.println("--- AssertJ 체이닝 ---");
        List<String> langs = new ArrayList<>(List.of("Java", "Kotlin", "Python"));
        try {
            AssertJ.assertThat(langs).hasSize(3).contains("Java");
            AssertJ.assertThat(calculator.multiply(6, 7)).isEqualTo(42).isGreaterThan(40);
            passed++;
        } catch (AssertionError e) {
            System.out.println("    -> " + e.getMessage());
            failed++;
        }

        System.out.println("\n=== verify 호출 검증 (Mockito 흉내) ===");

        // 호출 로그 기반 검증 시뮬레이션
        List<String> callLog = new ArrayList<>();
        callLog.add("add[1, 2]");
        callLog.add("add[1, 2]");
        callLog.add("multiply[6, 7]");

        long addCount = callLog.stream().filter("add[1, 2]"::equals).count();
        long multiplyCount = callLog.stream().filter("multiply[6, 7]"::equals).count();
        System.out.println("  verify(mock).add(1, 2)      호출 횟수 = " + addCount);
        System.out.println("  verify(mock).multiply(6,7)  호출 횟수 = " + multiplyCount);

        System.out.println("\n=== 테스트 결과 요약 ===");

        System.out.println("  통과: " + passed + ", 실패: " + failed);
        System.out.println("  실행 결과: " + (failed == 0 ? "ALL GREEN" : "FAILED"));

        System.out.println("\n=== 실제 JUnit5 + Mockito 코드 형태 (주석) ===");

        /*
        // 실제 테스트 코드 (강의자료용 참고)
        @ParameterizedTest
        @CsvSource({"1,2,3", "10,20,30", "-5,5,0"})
        void add(int a, int b, int expected) {
            assertThat(calculator.add(a, b)).isEqualTo(expected);
        }

        @Test
        void mockTest() {
            OrderRepository repo = Mockito.mock(OrderRepository.class);
            Mockito.when(repo.findById(1L)).thenReturn(Optional.of(order));
            assertThat(repo.findById(1L)).isPresent();
            Mockito.verify(repo).findById(1L);
        }
        */
    }
}

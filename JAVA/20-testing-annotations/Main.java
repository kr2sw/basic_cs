import java.lang.annotation.*;
import java.lang.reflect.*;

// --- 커스텀 어노테이션 ---
@Retention(RetentionPolicy.RUNTIME)
@Target({ElementType.METHOD, ElementType.TYPE})
@interface Info {
    String author() default "unknown";
    String description() default "";
    int version() default 1;
}

// --- 테스트할 클래스 ---
class Calculator {
    public int add(int a, int b) { return a + b; }
    public int subtract(int a, int b) { return a - b; }
    public int multiply(int a, int b) { return a * b; }
    public int divide(int a, int b) {
        if (b == 0) throw new IllegalArgumentException("0으로 나눌 수 없습니다");
        return a / b;
    }
}

// --- 간단한 테스트 프레임워크 ---
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.METHOD)
@interface TestMethod {}

class SimpleTest {
    private final Calculator calc = new Calculator();

    @TestMethod
    public void testAdd() {
        assertEqual(5, calc.add(2, 3), "add(2,3)");
    }

    @TestMethod
    public void testSubtract() {
        assertEqual(1, calc.subtract(3, 2), "subtract(3,2)");
    }

    @TestMethod
    public void testMultiply() {
        assertEqual(6, calc.multiply(2, 3), "multiply(2,3)");
    }

    @TestMethod
    public void testDivide() {
        assertEqual(2, calc.divide(6, 3), "divide(6,3)");
    }

    @TestMethod
    public void testDivideByZero() {
        try {
            calc.divide(1, 0);
            System.out.println("  ❌ 실패: divideByZero - 예외가 발생하지 않음");
        } catch (IllegalArgumentException e) {
            System.out.println("  ✅ 성공: divideByZero - 예외 발생 확인");
        }
    }

    private void assertEqual(int expected, int actual, String testName) {
        if (expected == actual) {
            System.out.println("  ✅ 성공: " + testName);
        } else {
            System.out.println("  ❌ 실패: " + testName
                + " (기대=" + expected + ", 실제=" + actual + ")");
        }
    }
}

// --- 어노테이션 사용 ---
@Info(author = "강사", description = "계산기 클래스", version = 2)
public class Main {

    @Info(author = "강사", description = "메인 메서드")
    public static void main(String[] args) throws Exception {
        // 1. 커스텀 어노테이션 읽기 (Reflection)
        System.out.println("=== Reflection: 클래스 정보 ===");
        Class<Main> clazz = Main.class;

        Info classInfo = clazz.getAnnotation(Info.class);
        if (classInfo != null) {
            System.out.println("클래스: " + clazz.getSimpleName());
            System.out.println("작성자: " + classInfo.author());
            System.out.println("설명: " + classInfo.description());
            System.out.println("버전: " + classInfo.version());
        }

        // 2. 메서드 정보
        System.out.println("\n=== Reflection: 메서드 정보 ===");
        Method[] methods = clazz.getDeclaredMethods();
        for (Method method : methods) {
            System.out.println("메서드: " + method.getName());
            Info methodInfo = method.getAnnotation(Info.class);
            if (methodInfo != null) {
                System.out.println("  설명: " + methodInfo.description());
            }
        }

        // 3. Reflection으로 객체 생성 및 호출
        System.out.println("\n=== Reflection: 동적 호출 ===");
        Calculator calc = Calculator.class.getDeclaredConstructor().newInstance();
        Method addMethod = Calculator.class.getMethod("add", int.class, int.class);
        int result = (int) addMethod.invoke(calc, 10, 20);
        System.out.println("Reflection으로 호출한 add(10,20) = " + result);

        // 4. 간단한 테스트 실행
        System.out.println("\n=== 간단한 테스트 실행 ===");
        SimpleTest test = new SimpleTest();
        int passed = 0, failed = 0;

        for (Method method : SimpleTest.class.getDeclaredMethods()) {
            if (method.isAnnotationPresent(TestMethod.class)) {
                try {
                    method.invoke(test);
                    passed++;
                } catch (Exception e) {
                    System.out.println("  ❌ 예외: " + method.getName()
                        + " - " + e.getCause().getMessage());
                    failed++;
                }
            }
        }

        System.out.printf("\n결과: %d 성공, %d 실패%n", passed, failed);
    }
}

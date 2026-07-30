public class Main {

    // 기본 메서드
    static int add(int a, int b) {
        return a + b;
    }

    // 오버로딩
    static double add(double a, double b) {
        return a + b;
    }

    static int add(int a, int b, int c) {
        return a + b + c;
    }

    // 가변인자 (Varargs)
    static int sumAll(int... numbers) {
        int sum = 0;
        for (int n : numbers) sum += n;
        return sum;
    }

    // void 반환
    static void printInfo(String name, int age) {
        System.out.println("이름: " + name + ", 나이: " + age);
    }

    // 재귀 메서드
    static int factorial(int n) {
        if (n <= 1) return 1;
        return n * factorial(n - 1);
    }

    public static void main(String[] args) {
        System.out.println("add(3, 5) = " + add(3, 5));
        System.out.println("add(3.5, 2.5) = " + add(3.5, 2.5));
        System.out.println("add(1, 2, 3) = " + add(1, 2, 3));

        System.out.println("sumAll(1,2,3,4,5) = " + sumAll(1, 2, 3, 4, 5));
        System.out.println("sumAll() = " + sumAll());

        printInfo("Alice", 25);

        System.out.println("factorial(5) = " + factorial(5));

        // Call by Value 증명
        int x = 10;
        changeValue(x);
        System.out.println("main의 x = " + x); // 여전히 10
    }

    static void changeValue(int x) {
        x = 100; // 원본 영향 없음
    }
}

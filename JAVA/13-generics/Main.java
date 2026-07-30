import java.util.*;

// 제네릭 클래스
class Box<T> {
    private T value;

    public Box(T value) {
        this.value = value;
    }

    public T get() { return value; }
    public void set(T value) { this.value = value; }

    @Override
    public String toString() {
        return "Box{" + value + " (" + value.getClass().getSimpleName() + ")}";
    }
}

// 제한된 타입 파라미터
class Calculator<T extends Number> {
    public double add(T a, T b) {
        return a.doubleValue() + b.doubleValue();
    }
}

public class Main {

    // 제네릭 메서드
    public static <T> void printArray(T[] array) {
        for (T element : array) {
            System.out.print(element + " ");
        }
        System.out.println();
    }

    // 와일드카드
    public static void printList(List<?> list) {
        for (Object elem : list) {
            System.out.print(elem + " ");
        }
        System.out.println();
    }

    public static double sumNumbers(List<? extends Number> list) {
        double sum = 0;
        for (Number n : list) sum += n.doubleValue();
        return sum;
    }

    public static void main(String[] args) {
        // 제네릭 클래스 사용
        Box<String> stringBox = new Box<>("Hello Generic");
        Box<Integer> intBox = new Box<>(42);
        System.out.println(stringBox);
        System.out.println(intBox);

        // 타입 안전성 (컴파일 타임에 검사)
        // stringBox.set(123); // 컴파일 에러!

        // 제한된 타입 파라미터
        Calculator<Integer> intCalc = new Calculator<>();
        System.out.println("int add: " + intCalc.add(10, 20));

        Calculator<Double> doubleCalc = new Calculator<>();
        System.out.println("double add: " + doubleCalc.add(3.5, 2.5));

        // 제네릭 메서드
        String[] strs = {"A", "B", "C"};
        Integer[] nums = {1, 2, 3};
        printArray(strs);
        printArray(nums);

        // 와일드카드
        List<String> strList = Arrays.asList("X", "Y", "Z");
        List<Integer> intList = Arrays.asList(10, 20, 30);
        printList(strList);
        printList(intList);

        System.out.println("sum: " + sumNumbers(intList));
        System.out.println("sum: " + sumNumbers(Arrays.asList(1.5, 2.5, 3.5)));

        // 타입 추론 (Java 7+ 다이아몬드 연산자)
        Map<String, List<Integer>> map = new HashMap<>();
        map.put("scores", Arrays.asList(90, 85, 92));
        System.out.println("Map: " + map);
    }
}

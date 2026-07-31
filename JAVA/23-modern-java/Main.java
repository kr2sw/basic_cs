import java.util.*;

public class Main {

    // record: 불변 데이터 클래스 (생성자/equals/hashCode/toString 자동 생성)
    record Point(int x, int y) {
        // 컴팩트 생성자: 검증 로직 추가 가능
        public Point {
            if (x < 0 || y < 0) throw new IllegalArgumentException("좌표는 0 이상이어야 함");
        }
        // 추가 메서드 정의 가능
        double distanceFromOrigin() {
            return Math.sqrt(x * x + y * y);
        }
    }

    // sealed interface: 상속 가능 타입 제한
    sealed interface Shape permits Circle, Square, Triangle {}

    record Circle(double r) implements Shape {}
    record Square(double side) implements Shape {}
    record Triangle(double base, double height) implements Shape {}

    public static void main(String[] args) {
        System.out.println("=== record ===");

        Point p = new Point(3, 4);
        Point q = new Point(3, 4);
        System.out.println("p = " + p);                        // toString 자동
        System.out.println("p.x() = " + p.x() + ", p.y() = " + p.y());
        System.out.println("p.equals(q) = " + p.equals(q));    // 값 비교 자동
        System.out.println("p.hashCode() = " + p.hashCode());
        System.out.println("원점과 거리 = " + p.distanceFromOrigin());

        // record 의 불변성: 값 변경은 새 인스턴스로
        Point moved = new Point(p.x() + 10, p.y());
        System.out.println("이동 후: " + moved);

        try {
            new Point(-1, 5);
        } catch (IllegalArgumentException e) {
            System.out.println("컴팩트 생성자 검증: " + e.getMessage());
        }

        System.out.println("\n=== sealed class ===");

        List<Shape> shapes = List.of(
            new Circle(5),
            new Square(4),
            new Triangle(6, 3)
        );

        // switch 패턴 매칭으로 타입별 처리
        for (Shape shape : shapes) {
            System.out.println(describe(shape));
        }

        System.out.println("\n=== pattern matching for instanceof ===");

        Object obj = "Hello, Modern Java";
        if (obj instanceof String s && s.length() > 5) {
            System.out.println("String 타입, 길이 " + s.length() + ": " + s.toUpperCase());
        }

        Object num = 42;
        if (num instanceof Integer i) {
            System.out.println("Integer 타입, 값 * 2 = " + (i * 2));
        }

        System.out.println("\n=== var 와 Text Block ===");

        var list = new ArrayList<String>();   // var: 타입 추론
        list.add("var");
        list.add("추론");
        System.out.println("var 리스트: " + list);

        String json = """
            {
              "name": "김철수",
              "age": 30,
              "skills": ["Java", "Spring", "SQL"]
            }
            """;
        System.out.println("Text Block JSON:\n" + json);
    }

    // sealed 타입에 대한 총괄(exhaustive) switch 패턴 매칭
    static String describe(Shape shape) {
        return switch (shape) {
            case Circle c -> "원 (반지름 " + c.r() + ", 넓이 " + Math.PI * c.r() * c.r() + ")";
            case Square s -> "정사각형 (한변 " + s.side() + ", 넓이 " + s.side() * s.side() + ")";
            case Triangle t -> "삼각형 (넓이 " + t.base() * t.height() / 2 + ")";
        };
    }
}

public class Main {
    public static void main(String[] args) {
        // 객체 생성
        Car myCar = new Car("Tesla Model 3", 2023, "Red");
        myCar.displayInfo();
        myCar.start();

        // Setter 사용
        myCar.setColor("Blue");
        System.out.println("변경된 색상: " + myCar.getColor());

        // 기본 생성자
        Car defaultCar = new Car();
        defaultCar.displayInfo();

        // static vs instance
        System.out.println("Math.PI = " + Math.PI);
        System.out.println("max(10, 20) = " + Math.max(10, 20));

        // final 필드
        final int CONSTANT = 100;
        System.out.println("상수: " + CONSTANT);
    }
}

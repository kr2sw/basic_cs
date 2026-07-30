// 추상 클래스
public abstract class Shape {
    protected String color;

    public Shape(String color) {
        this.color = color;
    }

    // 추상 메서드
    public abstract double getArea();

    // 일반 메서드
    public void displayColor() {
        System.out.println("색상: " + color);
    }
}

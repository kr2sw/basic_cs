public class Circle extends Shape implements Drawable, Resizable {
    private double radius;

    public Circle(String color, double radius) {
        super(color);
        this.radius = radius;
    }

    @Override
    public double getArea() {
        return Math.PI * radius * radius;
    }

    @Override
    public void draw() {
        System.out.println("○ " + color + " 원을 그립니다. (반지름: " + radius + ")");
    }

    @Override
    public void resize(double factor) {
        radius *= factor;
        System.out.println("크기 조정됨: 반지름 = " + radius);
    }
}

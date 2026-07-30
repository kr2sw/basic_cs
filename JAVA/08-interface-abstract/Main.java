public class Main {
    public static void main(String[] args) {
        Circle circle = new Circle("빨간", 5);

        // 추상 클래스 다형성
        Shape shape = circle;
        shape.displayColor();
        System.out.println("넓이: " + shape.getArea());

        // 인터페이스 다형성
        Drawable drawable = circle;
        drawable.draw();
        drawable.printInfo();

        Resizable resizable = circle;
        resizable.resize(2.0);

        // 인터페이스 static 메서드
        System.out.println("타입: " + Drawable.getType());

        // instanceof
        System.out.println("circle instanceof Shape: " + (circle instanceof Shape));
        System.out.println("circle instanceof Drawable: " + (circle instanceof Drawable));
        System.out.println("circle instanceof Resizable: " + (circle instanceof Resizable));
    }
}

// 인터페이스
public interface Drawable {
    void draw(); // public abstract

    // Java 8+ default 메서드
    default void printInfo() {
        System.out.println("이 도형은 그릴 수 있습니다.");
    }

    // Java 8+ static 메서드
    static String getType() {
        return "Drawable";
    }
}

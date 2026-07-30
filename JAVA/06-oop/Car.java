public class Car {
    private String model;
    private int year;
    private String color;

    // 기본 생성자
    public Car() {
        this("Unknown", 2024, "White");
    }

    // 생성자 오버로딩
    public Car(String model, int year, String color) {
        this.model = model;
        this.year = year;
        this.color = color;
    }

    // Getter / Setter
    public String getModel() { return model; }
    public void setModel(String model) { this.model = model; }

    public int getYear() { return year; }
    public void setYear(int year) { this.year = year; }

    public String getColor() { return color; }
    public void setColor(String color) { this.color = color; }

    // 인스턴스 메서드
    public void start() {
        System.out.println(model + " 시동을 겁니다.");
    }

    public void displayInfo() {
        System.out.println(model + " (" + year + "년식, " + color + ")");
    }
}

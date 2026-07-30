public class Main {
    public static void main(String[] args) {
        Dog dog = new Dog("초코", "골든 리트리버");
        dog.speak();   // 오버라이딩된 메서드
        dog.fetch();   // 자식 고유 메서드

        // 다형성 (Polymorphism)
        Animal animal = new Dog("멍멍이", "진돗개");
        animal.speak(); // 동적 바인딩 → Dog의 speak()
        // animal.fetch(); // 컴파일 에러: Animal 타입에 없음

        // instanceof
        System.out.println("animal instanceof Dog: " + (animal instanceof Dog));
        System.out.println("animal instanceof Animal: " + (animal instanceof Animal));

        // Object 메서드 활용
        Dog dog2 = new Dog("초코", "골든 리트리버");
        System.out.println("dog.equals(dog2): " + dog.equals(dog2));
        System.out.println("dog.toString(): " + dog);

        // final 클래스 (String)
        String s = "Hello";
        System.out.println(s.toUpperCase());
    }
}

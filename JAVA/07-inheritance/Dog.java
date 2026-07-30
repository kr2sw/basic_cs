public class Dog extends Animal {
    private String breed;

    public Dog(String name, String breed) {
        super(name); // 부모 생성자 호출
        this.breed = breed;
    }

    @Override
    public void speak() {
        System.out.println(name + "이(가) 멍멍 짖습니다!");
    }

    public void fetch() {
        System.out.println(name + "이(가) 공을 물어옵니다.");
    }

    public String getBreed() { return breed; }
}

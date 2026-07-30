public class Animal {
    protected String name;

    public Animal(String name) {
        this.name = name;
    }

    public void speak() {
        System.out.println(name + "이(가) 소리를 냅니다.");
    }

    @Override
    public String toString() {
        return "Animal{name='" + name + "'}";
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) return true;
        if (obj == null || getClass() != obj.getClass()) return false;
        Animal animal = (Animal) obj;
        return name.equals(animal.name);
    }

    @Override
    public int hashCode() {
        return name.hashCode();
    }
}

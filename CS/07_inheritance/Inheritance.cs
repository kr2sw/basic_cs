using System;

namespace BasicCS.Chapter07
{
    internal class Inheritance
    {
        static void Main()
        {
            // 기본 클래스 사용 (Base class usage)
            var animal = new Animal("Generic Animal");
            animal.MakeSound();

            // 파생 클래스 사용 (Derived class usage)
            var dog = new Dog("Buddy");
            dog.MakeSound();
            dog.Fetch();

            var cat = new Cat("Whiskers");
            cat.MakeSound();

            // 다형성 (Polymorphism)
            Animal poly = new Dog("Max");
            poly.MakeSound();  // Dog's version
            // poly.Fetch();   // 접근 불가 (Not accessible)

            // is / as 연산자
            if (poly is Dog d)
                d.Fetch();

            var maybeCat = poly as Cat;
            Console.WriteLine($"Is cat? {maybeCat != null}");

            // sealed 클래스 (Sealed class)
            var final = new FinalClass();
            final.Show();

            // abstract 예제
            Shape circle = new Circle(5);
            Shape rect = new Rectangle(4, 6);
            Console.WriteLine($"\nCircle area: {circle.GetArea():F2}");
            Console.WriteLine($"Rectangle area: {rect.GetArea():F2}");
        }
    }

    // 기본 클래스 (Base class)
    class Animal
    {
        public string Name { get; }

        public Animal(string name) => Name = name;

        // virtual 키워드로 오버라이드 허용 (Allows override)
        public virtual void MakeSound()
        {
            Console.WriteLine($"{Name} makes a sound");
        }
    }

    // 파생 클래스 (Derived class)
    class Dog : Animal
    {
        public Dog(string name) : base(name) { }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} barks: Woof!");
        }

        public void Fetch() => Console.WriteLine($"{Name} fetches the ball");
    }

    sealed class Cat : Animal
    {
        public Cat(string name) : base(name) { }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} meows: Meow!");
        }
    }

    // sealed 클래스는 더 이상 상속 불가 (Cannot inherit from sealed class)
    sealed class FinalClass
    {
        public void Show() => Console.WriteLine("FinalClass - cannot be inherited");
    }

    // 추상 클래스 (Abstract class)
    abstract class Shape
    {
        public abstract double GetArea();
    }

    class Circle : Shape
    {
        private double Radius { get; }
        public Circle(double r) => Radius = r;

        public override double GetArea() => Math.PI * Radius * Radius;
    }

    class Rectangle : Shape
    {
        private double Width { get; }
        private double Height { get; }
        public Rectangle(double w, double h) => (Width, Height) = (w, h);

        public override double GetArea() => Width * Height;
    }
}

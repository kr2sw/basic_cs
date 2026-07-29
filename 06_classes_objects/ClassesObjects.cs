using System;

namespace BasicCS.Chapter06
{
    internal class ClassesObjects
    {
        static void Main()
        {
            // 객체 생성 (Object instantiation)
            var p1 = new Person("Alice", 30);
            p1.Introduce();

            // 객체 이니셜라이저 (Object initializer)
            var p2 = new Person { Name = "Bob", Age = 25 };
            p2.Introduce();

            // record (C# 9.0+)
            var r1 = new Product("Laptop", 1200m);
            var r2 = r1 with { Name = "Tablet" };
            Console.WriteLine($"Record: {r1}, Modified: {r2}");

            // static 멤버 (Static members)
            Console.WriteLine($"Total persons: {Person.Count}");

            // readonly struct 예제
            var pt = new Point(3, 4);
            Console.WriteLine($"Point distance: {pt.DistanceFromOrigin()}");

            // extension method
            Console.WriteLine($"\"hello\" reversed: {"hello".Reverse()}");
        }
    }

    // 기본 클래스 (Basic class)
    class Person
    {
        // 자동 구현 속성 (Auto-implemented properties)
        public string Name { get; set; }
        public int Age { get; set; }

        // 읽기 전용 속성 (Read-only property)
        public bool IsAdult => Age >= 18;

        // 정적 필드 (Static field)
        public static int Count { get; private set; } = 0;

        // 생성자 (Constructor)
        public Person() { Name = ""; Age = 0; Count++; }

        public Person(string name, int age) : this()
        {
            Name = name;
            Age = age;
        }

        // 메서드 (Method)
        public void Introduce()
        {
            Console.WriteLine($"Hi, I'm {Name}, {Age} years old. Adult: {IsAdult}");
        }

        // 소멸자 (Finalizer)
        ~Person()
        {
            Count--;
        }
    }

    // record (C# 9.0+)
    record Product(string Name, decimal Price);

    // readonly struct
    readonly struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double DistanceFromOrigin() => Math.Sqrt(X * X + Y * Y);
    }

    // 확장 메서드 (Extension methods)
    static class StringExtensions
    {
        public static string Reverse(this string s)
        {
            char[] chars = s.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
    }
}

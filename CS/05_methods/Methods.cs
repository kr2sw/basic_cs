using System;

namespace BasicCS.Chapter05
{
    internal class Methods
    {
        static void Main()
        {
            int sum = Add(3, 5);
            Console.WriteLine($"Add(3, 5) = {sum}");

            PrintMessage("Hello, Methods!");

            // ref parameter
            int val = 10;
            Increment(ref val);
            Console.WriteLine($"After Increment(ref): {val}");

            // out parameter
            TryDivide(10, 3, out double result, out int remainder);
            Console.WriteLine($"TryDivide: {result}, remainder {remainder}");

            // params
            Console.WriteLine($"SumAll(1..5) = {SumAll(1, 2, 3, 4, 5)}");

            // optional / named arguments
            Greet("Alice");
            Greet("Bob", greeting: "Hi");

            // local function
            int Multiply(int a, int b) => a * b;
            Console.WriteLine($"Local function: {Multiply(4, 5)}");

            // expression-bodied method
            Console.WriteLine($"Square(6) = {Square(6)}");
        }

        // 기본 메서드 (Basic method)
        static int Add(int a, int b) => a + b;

        // void 반환 (Void return)
        static void PrintMessage(string msg)
        {
            Console.WriteLine(msg);
        }

        // ref 파라미터 (Reference parameter)
        static void Increment(ref int x) => x++;

        // out 파라미터 (Output parameter)
        static void TryDivide(int a, int b, out double quot, out int rem)
        {
            quot = (double)a / b;
            rem = a % b;
        }

        // params 가변 인자 (Variable-length parameters)
        static int SumAll(params int[] numbers)
        {
            int total = 0;
            foreach (var n in numbers) total += n;
            return total;
        }

        // 선택적 매개변수 (Optional parameters)
        static void Greet(string name, string greeting = "Hello")
        {
            Console.WriteLine($"{greeting}, {name}!");
        }

        // 식 본문 메서드 (Expression-bodied method)
        static int Square(int x) => x * x;

        // 오버로딩 (Overloading)
        static double Add(double a, double b) => a + b;
    }
}

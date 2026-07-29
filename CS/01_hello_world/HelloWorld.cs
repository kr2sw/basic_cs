using System;

namespace BasicCS.Chapter01
{
    internal class HelloWorld
    {
        static void Main()
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine("Welcome to C# Basic Course");

            Console.Write("Enter your name: ");
            string name = Console.ReadLine()!;
            Console.WriteLine($"Nice to meet you, {name}!");
        }
    }
}

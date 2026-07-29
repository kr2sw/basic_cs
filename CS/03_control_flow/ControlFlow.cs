using System;

namespace BasicCS.Chapter03
{
    internal class ControlFlow
    {
        static void Main()
        {
            // ---- 조건문 (Conditionals) ----

            int score = 85;

            if (score >= 90)
                Console.WriteLine("Grade: A");
            else if (score >= 80)
                Console.WriteLine("Grade: B");
            else if (score >= 70)
                Console.WriteLine("Grade: C");
            else
                Console.WriteLine("Grade: F");

            // switch 문 (switch statement)
            string day = "Monday";
            switch (day)
            {
                case "Monday":
                    Console.WriteLine("Start of work week");
                    break;
                case "Friday":
                    Console.WriteLine("TGIF!");
                    break;
                case "Saturday":
                case "Sunday":
                    Console.WriteLine("Weekend!");
                    break;
                default:
                    Console.WriteLine("Midweek");
                    break;
            }

            // Switch expression (C# 8.0+)
            string category = score switch
            {
                >= 90 => "Excellent",
                >= 80 => "Good",
                >= 70 => "Average",
                _ => "Needs improvement"
            };
            Console.WriteLine($"Category: {category}");

            // ---- 반복문 (Loops) ----

            // for loop
            Console.Write("for loop: ");
            for (int i = 0; i < 5; i++)
                Console.Write($"{i} ");
            Console.WriteLine();

            // foreach loop
            Console.Write("foreach: ");
            foreach (char ch in "C#")
                Console.Write($"{ch} ");
            Console.WriteLine();

            // while loop
            int count = 0;
            Console.Write("while: ");
            while (count < 3)
                Console.Write($"{count++} ");
            Console.WriteLine();

            // do-while loop
            int n = 0;
            Console.Write("do-while: ");
            do
                Console.Write($"{n++} ");
            while (n < 3);
            Console.WriteLine();
        }
    }
}

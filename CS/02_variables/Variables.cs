using System;

namespace BasicCS.Chapter02
{
    internal class Variables
    {
        static void Main()
        {
            // 정수 타입 (Integer types)
            int age = 25;
            long bigNumber = 1_000_000_000_000L;
            byte small = 255;

            // 실수 타입 (Floating-point types)
            float price = 19.99f;
            double pi = 3.141592653589793;
            decimal precise = 199.95m;

            // 논리 타입 (Boolean)
            bool isActive = true;

            // 문자와 문자열 (Char and string)
            char grade = 'A';
            string name = "C# Programming";

            // var 키워드 (Implicitly typed variable)
            var message = "Type is inferred at compile time";

            // 출력 (Output)
            Console.WriteLine($"int    : {age}");
            Console.WriteLine($"long   : {bigNumber}");
            Console.WriteLine($"byte   : {small}");
            Console.WriteLine($"float  : {price}");
            Console.WriteLine($"double : {pi:F4}");
            Console.WriteLine($"decimal: {precise}");
            Console.WriteLine($"bool   : {isActive}");
            Console.WriteLine($"char   : {grade}");
            Console.WriteLine($"string : {name}");
            Console.WriteLine($"var    : {message}");

            // 형변환 (Type conversion)
            int intVal = 42;
            double dblVal = intVal;          // 암시적 변환 (implicit)
            int backToInt = (int)dblVal;     // 명시적 변환 (explicit cast)
            string strVal = 123.ToString();  // ToString()

            Console.WriteLine($"\nConversion: {intVal} -> {dblVal} -> {backToInt}, string: {strVal}");

            // nullable 타입 (Nullable types)
            int? maybeNull = null;
            Console.WriteLine($"Nullable has value: {maybeNull.HasValue}");
            maybeNull = 10;
            Console.WriteLine($"Nullable value: {maybeNull ?? -1}");
        }
    }
}

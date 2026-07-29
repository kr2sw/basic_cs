using System;

namespace BasicCS.Chapter11
{
    // 델리게이트 선언 (Delegate declaration)
    delegate int Operation(int x, int y);
    delegate void Notify(string message);

    internal class DelegatesEvents
    {
        static void Main()
        {
            // ---- 델리게이트 (Delegates) ----
            Operation add = (a, b) => a + b;
            Operation multiply = (a, b) => a * b;

            Console.WriteLine($"Add(3,5): {add(3, 5)}");
            Console.WriteLine($"Multiply(3,5): {multiply(3, 5)}");

            // 멀티캐스트 델리게이트 (Multicast delegate)
            Notify notifier = LogToConsole;
            notifier += LogToFile;
            notifier("Multicast delegate example");

            notifier = (Notify)Delegate.Remove(notifier!, LogToConsole)!;
            notifier("After removing console logger");

            // 델리게이트를 파라미터로 전달 (Delegate as parameter)
            int[] numbers = { 1, 2, 3, 4, 5 };
            int sum = ProcessNumbers(numbers, (a, b) => a + b);
            int product = ProcessNumbers(numbers, (a, b) => a * b);
            Console.WriteLine($"\nSum: {sum}, Product: {product}");

            // ---- Func / Action / Predicate (내장 델리게이트) ----
            Func<int, int, string> formatSum = (a, b) => $"{a} + {b} = {a + b}";
            Console.WriteLine(formatSum(10, 20));

            Action<string> print = msg => Console.WriteLine($"Action: {msg}");
            print("Hello from Action");

            Predicate<int> isEven = x => x % 2 == 0;
            Console.WriteLine($"Is 4 even? {isEven(4)}");

            // ---- 이벤트 (Events) ----
            var button = new Button();
            button.Clicked += OnButtonClick;
            button.Clicked += (sender, e) => Console.WriteLine("Lambda event handler");
            button.SimulateClick();

            // ---- Anonymous method ----
            Operation anonymous = delegate (int a, int b) { return a - b; };
            Console.WriteLine($"Anonymous method 10-3: {anonymous(10, 3)}");

            // ---- Closure 예제 ----
            int factor = 3;
            Func<int, int> multiplier = x => x * factor;
            Console.WriteLine($"Closure 5*3: {multiplier(5)}");
            factor = 10;
            Console.WriteLine($"Closure 5*10 (factor updated): {multiplier(5)}");
        }

        static void LogToConsole(string msg) => Console.WriteLine($"[Console] {msg}");
        static void LogToFile(string msg) => Console.WriteLine($"[File]    {msg}");

        static int ProcessNumbers(int[] nums, Operation op)
        {
            int result = nums[0];
            for (int i = 1; i < nums.Length; i++)
                result = op(result, nums[i]);
            return result;
        }

        static void OnButtonClick(object? sender, EventArgs e)
        {
            Console.WriteLine("Button was clicked!");
        }
    }

    // 이벤트를 갖는 클래스 (Class with events)
    class Button
    {
        // 이벤트 선언 (Event declaration)
        public event EventHandler? Clicked;

        public void SimulateClick()
        {
            Console.WriteLine("\nSimulating button click...");
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

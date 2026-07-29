using System;

namespace BasicCS.Chapter09
{
    internal class Exceptions
    {
        static void Main()
        {
            // 기본 try-catch-finally
            try
            {
                Console.Write("Enter a number: ");
                string input = Console.ReadLine()!;
                int number = int.Parse(input);
                Console.WriteLine($"You entered: {number}");

                int result = 100 / number;
                Console.WriteLine($"100 / {number} = {result}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Invalid format: {ex.Message}");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Cannot divide by zero");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.GetType().Name}: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Finally block always executes");
            }

            // throw 키워드
            try
            {
                ValidateAge(-5);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation error: {ex.Message}");
            }

            // 예외 필터 (Exception filter, C# 6.0+)
            try
            {
                throw new InvalidOperationException("Custom error code 42");
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("42"))
            {
                Console.WriteLine($"Caught filtered exception: {ex.Message}");
            }

            // using 문 (안전한 리소스 관리)
            try
            {
                using var resource = new Resource("MyResource");
                resource.DoWork();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with resource: {ex.Message}");
            }

            // 사용자 정의 예외 (Custom exception)
            try
            {
                Withdraw(100, 200);
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"Custom exception: {ex.Message}, Balance: {ex.Balance:C}");
            }
        }

        static void ValidateAge(int age)
        {
            if (age < 0)
                throw new ArgumentException("Age cannot be negative", nameof(age));
            Console.WriteLine($"Age {age} is valid");
        }

        static void Withdraw(decimal balance, decimal amount)
        {
            if (amount > balance)
                throw new InsufficientFundsException(balance, amount);
            Console.WriteLine($"Withdrew {amount:C}");
        }
    }

    // 사용자 정의 예외 클래스 (Custom exception class)
    class InsufficientFundsException : Exception
    {
        public decimal Balance { get; }
        public decimal Amount { get; }

        public InsufficientFundsException(decimal balance, decimal amount)
            : base($"Insufficient funds. Balance: {balance:C}, Required: {amount:C}")
        {
            Balance = balance;
            Amount = amount;
        }
    }

    // IDisposable 리소스 예제
    class Resource : IDisposable
    {
        private readonly string _name;
        public Resource(string name) => _name = name;

        public void DoWork() => Console.WriteLine($"Resource '{_name}' doing work");

        public void Dispose() => Console.WriteLine($"Resource '{_name}' disposed");
    }
}

namespace BasicCS.Chapter20;

/// <summary>
/// 기본적인 사칙연산을 제공하는 계산기 클래스입니다.
/// </summary>
public class Calculator
{
    /// <summary>
    /// 두 정수를 더합니다.
    /// </summary>
    /// <param name="a">첫 번째 피연산자</param>
    /// <param name="b">두 번째 피연산자</param>
    /// <returns>a와 b의 합</returns>
    public int Add(int a, int b) => a + b;

    /// <summary>
    /// 두 정수를 뺍니다.
    /// </summary>
    /// <param name="a">피감수</param>
    /// <param name="b">감수</param>
    /// <returns>a에서 b를 뺀 결과</returns>
    public int Subtract(int a, int b) => a - b;

    /// <summary>
    /// 두 정수를 곱합니다.
    /// </summary>
    /// <param name="a">첫 번째 피연산자</param>
    /// <param name="b">두 번째 피연산자</param>
    /// <returns>a와 b의 곱</returns>
    public int Multiply(int a, int b) => a * b;

    /// <summary>
    /// 두 정수를 나눕니다.
    /// </summary>
    /// <param name="a">피제수</param>
    /// <param name="b">제수 (0이 될 수 없음)</param>
    /// <returns>a를 b로 나눈 몫</returns>
    /// <exception cref="DivideByZeroException">b가 0인 경우 발생</exception>
    public int Divide(int a, int b)
    {
        if (b == 0)
            throw new DivideByZeroException("0으로 나눌 수 없습니다.");
        return a / b;
    }
}

internal class CalculatorDemo
{
    static void Main()
    {
        Console.WriteLine("=== Calculator 데모 ===\n");

        var calc = new Calculator();

        // 덧셈
        int sum = calc.Add(10, 5);
        Console.WriteLine($"Add(10, 5) = {sum}");

        // 뺄셈
        int diff = calc.Subtract(10, 5);
        Console.WriteLine($"Subtract(10, 5) = {diff}");

        // 곱셈
        int product = calc.Multiply(10, 5);
        Console.WriteLine($"Multiply(10, 5) = {product}");

        // 나눗셈
        int quotient = calc.Divide(10, 5);
        Console.WriteLine($"Divide(10, 5) = {quotient}");

        // 0으로 나누기 예외 처리
        try
        {
            calc.Divide(10, 0);
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Divide(10, 0) 예외: {ex.Message}");
        }

        Console.WriteLine("\n=== Calculator 데모 종료 ===");
    }
}

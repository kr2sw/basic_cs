using System;
using System.Diagnostics;
using System.Text;

namespace BasicCS.Chapter14;

internal class Strings
{
    static void Main()
    {
        // ---- 1. String (불변) vs StringBuilder (가변) ----
        Console.WriteLine("=== String vs StringBuilder ===");

        // String: 불변 객체, 문자열 연결 시 새 객체 생성
        string str = "Hello";
        str += ", ";    // 새 문자열 객체 생성
        str += "World!"; // 또 새 문자열 객체 생성
        Console.WriteLine($"String result: {str}");

        // StringBuilder: 가변 객체, 내부 버퍼로 효율적 처리
        var sb = new StringBuilder("Hello");
        sb.Append(", ");
        sb.Append("World!");
        Console.WriteLine($"StringBuilder result: {sb}");

        // 성능 비교
        const int iterations = 10000;
        var sw = Stopwatch.StartNew();
        string strResult = "";
        for (int i = 0; i < iterations; i++)
            strResult += "a";
        sw.Stop();
        Console.WriteLine($"\nString '+' concatenation ({iterations}회): {sw.ElapsedMilliseconds}ms");

        sw.Restart();
        var sbResult = new StringBuilder();
        for (int i = 0; i < iterations * 100; i++) // StringBuilder가 훨씬 빠름
            sbResult.Append("a");
        sw.Stop();
        Console.WriteLine($"StringBuilder Append ({iterations * 100}회): {sw.ElapsedMilliseconds}ms");

        // ---- 2. StringBuilder 주요 메서드 ----
        Console.WriteLine("\n=== StringBuilder 주요 메서드 ===");
        var builder = new StringBuilder("Hello World!");

        builder.Append(" Welcome.");
        Console.WriteLine($"Append: {builder}");

        builder.AppendLine(" This is extra.");
        Console.WriteLine($"AppendLine: {builder}");

        builder.Insert(6, "Beautiful ");
        Console.WriteLine($"Insert(6, \"Beautiful \"): {builder}");

        builder.Replace("World", "C#");
        Console.WriteLine($"Replace(\"World\", \"C#\"): {builder}");

        builder.Remove(6, 10); // "Beautiful " 제거
        Console.WriteLine($"Remove(6, 10): {builder}");

        builder.Clear();
        Console.WriteLine($"Clear: \"{builder}\"");

        // ---- 3. 문자열 메서드 ----
        Console.WriteLine("\n=== String 주요 메서드 ===");
        string text = "  Hello, C# World! Welcome to programming.  ";

        Console.WriteLine($"Original: \"{text}\"");
        Console.WriteLine($"Trim: \"{text.Trim()}\"");
        Console.WriteLine($"ToUpper: \"{text.ToUpper()}\"");
        Console.WriteLine($"ToLower: \"{text.ToLower()}\"");
        Console.WriteLine($"Length: {text.Length}");

        // Substring
        Console.WriteLine($"Substring(2, 5): \"{text.Substring(2, 5)}\"");

        // IndexOf
        Console.WriteLine($"IndexOf(\"World\"): {text.IndexOf("World")}");
        Console.WriteLine($"LastIndexOf(\"o\"): {text.LastIndexOf("o")}");

        // Contains, StartsWith, EndsWith
        Console.WriteLine($"Contains(\"C#\"): {text.Contains("C#")}");
        Console.WriteLine($"StartsWith(\"  \"): {text.StartsWith("  ")}");
        Console.WriteLine($"EndsWith(\"  \"): {text.EndsWith("  ")}");

        // Replace
        string replaced = text.Replace("C#", "CSharp");
        Console.WriteLine($"Replace(\"C#\", \"CSharp\"): \"{replaced}\"");

        // ---- 4. StringComparison (문화권 비교) ----
        Console.WriteLine("\n=== StringComparison ===");
        string a = "apple";
        string b = "Apple";

        // 기본 비교 (문화권 구분)
        Console.WriteLine($"Default: \"{a}\".Equals(\"{b}\") = {a.Equals(b)}");

        // 대소문자 무시 비교
        Console.WriteLine($"OrdinalIgnoreCase: \"{a}\".Equals(\"{b}\", OrdinalIgnoreCase) = {a.Equals(b, StringComparison.OrdinalIgnoreCase)}");

        // CurrentCulture 비교
        int cmp = string.Compare("a", "A", StringComparison.CurrentCultureIgnoreCase);
        Console.WriteLine($"CurrentCultureIgnoreCase compare: {cmp}");

        // ---- 5. Split & Join ----
        Console.WriteLine("\n=== Split & Join ===");
        string csv = "apple,banana,cherry,durian";
        string[] parts = csv.Split(',');
        Console.WriteLine($"Split: {string.Join(" | ", parts)}");

        // 여러 구분자로 Split
        string messy = "one;two,three|four";
        string[] messyParts = messy.Split(';', ',', '|');
        Console.WriteLine($"Multi-split: {string.Join(", ", messyParts)}");

        // Split with options
        string spaced = "  a  ,  b  ,  c  ";
        string[] cleanParts = spaced.Split(new[] { ',' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        Console.WriteLine($"Clean split: {string.Join(", ", cleanParts)}");

        // String.Join
        string[] words = { "C#", "is", "awesome" };
        string joined = string.Join(" - ", words);
        Console.WriteLine($"String.Join: \"{joined}\"");

        // ---- 6. 보간 문자열 (Interpolated String) ----
        Console.WriteLine("\n=== 보간 문자열 ($\"...\") ===");
        string name = "Alice";
        int age = 30;
        double pi = 3.1415926535;

        string msg = $"My name is {name} and I'm {age} years old.";
        Console.WriteLine(msg);

        // 형식 지정
        Console.WriteLine($"Pi = {pi:F2} (소수점 2자리)");
        Console.WriteLine($"Pi = {pi:F4} (소수점 4자리)");
        Console.WriteLine($"Pi = {pi,10:F2} (우측 정렬 10칸)");

        // 보간 내 조건식
        Console.WriteLine($"{name} is {(age >= 19 ? "adult" : "minor")}");

        // ---- 7. 축자 문자열 리터럴 (Verbatim String) ----
        Console.WriteLine("\n=== 축자 문자열 (@\"...\") ===");
        string path = @"C:\Users\Alice\Documents\file.txt";
        Console.WriteLine($"Path: {path}");

        string multiLine = @"첫 번째 줄
두 번째 줄
세 번째 줄";
        Console.WriteLine($"Multi-line:{Environment.NewLine}{multiLine}");

        // ---- 8. String.Format ----
        Console.WriteLine("\n=== String.Format ===");
        string formatted = string.Format("{0} scored {1} points out of {2}.", "Bob", 95, 100);
        Console.WriteLine(formatted);

        // 날짜 형식
        DateTime now = DateTime.Now;
        Console.WriteLine(string.Format("Today is {{0:yyyy-MM-dd}} {0:yyyy-MM-dd}", now));
        Console.WriteLine(string.Format("Time: {0:HH:mm:ss}", now));

        // 숫자 형식
        Console.WriteLine(string.Format("Currency: {0:C}", 12345.67));
        Console.WriteLine(string.Format("Percentage: {0:P}", 0.1234));
        Console.WriteLine(string.Format("Number with commas: {0:N0}", 1234567));

        // ---- 9. StringBuilder로 CSV 생성 ----
        Console.WriteLine("\n=== StringBuilder 응용: CSV 생성 ===");
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("Name,Age,Score");
        csvBuilder.AppendLine("Alice,30,95");
        csvBuilder.AppendLine("Bob,25,87");
        csvBuilder.AppendLine("Charlie,35,92");
        Console.WriteLine(csvBuilder.ToString());

        Console.WriteLine("=== All string examples completed ===");
    }
}

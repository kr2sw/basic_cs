using System.Diagnostics;

namespace BasicCS.Chapter27;

static class Program
{
    // ---- Span 슬라이싱: 할당 없이 부분 문자열 ----
    static int CountOccurrences(ReadOnlySpan<char> text, char target)
    {
        int count = 0;
        foreach (char c in text)
            if (c == target)
                count++;
        return count;
    }

    // ---- Span으로 CSV 첫 줄 파싱 (문자열 할당 없이) ----
    static int SumNumbers(ReadOnlySpan<char> line)
    {
        int sum = 0;
        foreach (var part in line.Split(','))
        {
            sum += int.Parse(part);
        }
        return sum;
    }

    // ---- Memory<T>: async 경계를 넘을 수 있는 힙 기반 버퍼 ----
    static async Task ProcessBufferAsync(Memory<byte> buffer)
    {
        // await 경계에서도 안전하게 사용 가능
        await Task.Delay(50);
        ReadOnlySpan<byte> span = buffer.Span;
        int sum = 0;
        foreach (byte b in span)
            sum += b;
        Console.WriteLine($"  [async 처리] 버퍼 합계: {sum}");
    }

    static void Main()
    {
        // ---- 1. 기본 슬라이싱 ----
        string text = "Hello, .NET World!";
        ReadOnlySpan<char> hello = text.AsSpan(0, 5);
        ReadOnlySpan<char> net = text.AsSpan(7, 4);
        Console.WriteLine($"[Slice] '{hello}' / '{net}'");

        // ---- 2. 대소문자 무시 비교 (ref struct 스택 할당) ----
        Span<char> upper = stackalloc char[10];
        "abcdefghij".AsSpan().CopyTo(upper);
        bool equal = upper.SequenceEqual("ABCDEFGHIJ".AsSpan().ToString().ToUpper().AsSpan());
        Console.WriteLine($"[SequenceEqual] 스택 버퍼 대소문자 비교: {equal}");

        // ---- 3. 문자 카운트 ----
        Console.WriteLine($"[CountOccurrences] 'l' 개수: {CountOccurrences(text.AsSpan(), 'l')}");

        // ---- 4. CSV 숫자 합계 ----
        string csv = "10,20,30,40";
        Console.WriteLine($"[CSV 파싱] '{csv}' 합계: {SumNumbers(csv.AsSpan())}");

        // ---- 5. TryParse로 할당 없이 파싱 ----
        ReadOnlySpan<char> numSpan = "0042".AsSpan();
        bool parsed = int.TryParse(numSpan, out int value);
        Console.WriteLine($"[TryParse] '{numSpan.ToString()}' -> {value} (성공: {parsed})");

        // ---- 6. 배열을 Span으로 ----
        int[] arr = { 1, 2, 3, 4, 5 };
        Span<int> spanArr = arr;
        spanArr[0] = 100; // Span을 통해 배열 수정
        Console.WriteLine($"[Span<배열>] 첫 요소 수정 후 arr[0] = {arr[0]}");

        // ---- 7. Memory<T> 비동기 사용 ----
        Console.WriteLine("[Memory<byte>] async 경계 전달");
        byte[] bytes = { 1, 2, 3, 4, 5 };
        ProcessBufferAsync(bytes).GetAwaiter().GetResult();

        // ---- 8. 성능 비교: 문자열 vs Span (간단 벤치) ----
        const int iterations = 1_000_000;
        string big = new string('a', 100);

        var sw = Stopwatch.StartNew();
        long stringSum = 0;
        for (int i = 0; i < iterations; i++)
        {
            string sub = big.Substring(10, 20); // 매번 할당
            stringSum += sub.Length;
        }
        sw.Stop();
        Console.WriteLine($"\n[벤치] Substring (할당 O): {sw.ElapsedMilliseconds}ms (합계 {stringSum})");

        sw.Restart();
        long spanSum = 0;
        for (int i = 0; i < iterations; i++)
        {
            ReadOnlySpan<char> sub = big.AsSpan(10, 20); // 할당 없음
            spanSum += sub.Length;
        }
        sw.Stop();
        Console.WriteLine($"[벤치] AsSpan    (할당 X): {sw.ElapsedMilliseconds}ms (합계 {spanSum})");
    }
}

using System.Diagnostics;
using System.Text;

namespace BasicCS.Chapter28;

static class Program
{
    static void Main()
    {
        const int n = 100_000;

        // ---- 1. 문자열 합치기: + vs StringBuilder ----
        var sw = Stopwatch.StartNew();
        string concat = "";
        for (int i = 0; i < n; i++)
            concat += "x"; // O(n^2) — 매번 새 문자열 할당
        sw.Stop();
        Console.WriteLine($"[문자열] '+' 연결:    {sw.ElapsedMilliseconds} ms (길이 {concat.Length})");

        sw.Restart();
        var sb = new StringBuilder(n);
        for (int i = 0; i < n; i++)
            sb.Append('x');
        string built = sb.ToString();
        sw.Stop();
        Console.WriteLine($"[문자열] StringBuilder: {sw.ElapsedMilliseconds} ms (길이 {built.Length})");

        // ---- 2. 컬렉션 선택: List.Contains vs HashSet ----
        var list = Enumerable.Range(0, n).ToList();
        var set = Enumerable.Range(0, n).ToHashSet();
        int target = n - 1;

        sw.Restart();
        bool inList = list.Contains(target); // O(n)
        sw.Stop();
        Console.WriteLine($"\n[검색] List.Contains:    {sw.ElapsedMilliseconds} ms ({inList})");

        sw.Restart();
        bool inSet = set.Contains(target); // O(1)
        sw.Stop();
        Console.WriteLine($"[검색] HashSet.Contains: {sw.ElapsedMilliseconds} ms ({inSet})");

        // ---- 3. Dictionary 사전 크기 예약 (capacity) ----
        sw.Restart();
        var dict1 = new Dictionary<int, int>();
        for (int i = 0; i < n; i++)
            dict1[i] = i;
        sw.Stop();
        Console.WriteLine($"\n[딕셔너리] 예약 없음:      {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        var dict2 = new Dictionary<int, int>(n); // capacity 예약 -> 리해시 감소
        for (int i = 0; i < n; i++)
            dict2[i] = i;
        sw.Stop();
        Console.WriteLine($"[딕셔너리] capacity 예약:   {sw.ElapsedMilliseconds} ms");

        // ---- 4. List vs 배열 순회 (foreach 성능) ----
        var intArr = new int[n];
        var intList = new List<int>(intArr);

        sw.Restart();
        long sumArr = 0;
        for (int i = 0; i < n; i++)
            sumArr += intArr[i];
        sw.Stop();
        Console.WriteLine($"\n[순회] 배열 for:  {sw.ElapsedMilliseconds} ms (합 {sumArr})");

        sw.Restart();
        long sumList = 0;
        for (int i = 0; i < n; i++)
            sumList += intList[i];
        sw.Stop();
        Console.WriteLine($"[순회] List for: {sw.ElapsedMilliseconds} ms (합 {sumList})");

        // ---- 5. LINQ vs 수동 루프 ----
        sw.Restart();
        int maxLinq = Enumerable.Range(0, n).Max();
        sw.Stop();
        Console.WriteLine($"\n[집계] LINQ Max:     {sw.ElapsedMilliseconds} ms ({maxLinq})");

        sw.Restart();
        int maxManual = 0;
        for (int i = 0; i < n; i++)
            if (i > maxManual) maxManual = i;
        sw.Stop();
        Console.WriteLine($"[집계] 수동 루프:     {sw.ElapsedMilliseconds} ms ({maxManual})");

        Console.WriteLine("\n결론: 컬렉션 종류·문자열 처리 선택이 성능에 큰 영향을 줍니다.");
    }
}

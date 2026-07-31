namespace BasicCS.Chapter36;

// ---- IDisposable 구현: 비관리 리소스(파일)를 정리 ----
public class FileWriter : IDisposable
{
    private readonly StreamWriter _writer;
    private bool _disposed;

    public FileWriter(string path)
    {
        _writer = new StreamWriter(path);
        Console.WriteLine("  [FileWriter] 파일 열림");
    }

    public void Write(string text) => _writer.WriteLine(text);

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _writer.Dispose();
        Console.WriteLine("  [FileWriter] 파일 닫힘 (Dispose)");
        GC.SuppressFinalize(this); // 파이널라이저 중복 실행 방지
    }

    // 파이널라이저: Dispose가 잊혀졌을 때만 실행되는 안전망
    ~FileWriter()
    {
        Console.WriteLine("  [FileWriter] 파이널라이저 실행 (안전망)");
        Dispose();
    }
}

// ---- 파이널라이저만 가진 예 (비결정적 정리 데모) ----
public class TempResource
{
    private static int _count;
    public TempResource() => Console.WriteLine($"  [Temp#{++_count}] 생성");

    ~TempResource() => Console.WriteLine($"  [Temp] 파이널라이저: 언제 실행될지 모름");
}

static class Program
{
    static void Main()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "mem_test.txt");

        Console.WriteLine("== 1) using 선언: 블록 끝에서 자동 Dispose ==");
        using (var writer = new FileWriter(path))
        {
            writer.Write("첫 번째 줄");
        } // 여기서 자동 Dispose
        Console.WriteLine("  (블록을 벗어났으므로 이미 정리됨)");

        Console.WriteLine("\n== 2) using var: 메서드 끝에서 자동 정리 ==");
        using var writer2 = new FileWriter(path);
        writer2.Write("두 번째 줄");
        // Main이 끝나면 정리됨

        Console.WriteLine("\n== 3) try/finally 수동 Dispose (using의 실제 모습) ==");
        FileWriter? manual = null;
        try
        {
            manual = new FileWriter(path);
            manual.Write("세 번째 줄");
        }
        finally
        {
            manual?.Dispose(); // using은 이 try/finally로 컴파일된다
        }

        Console.WriteLine("\n== 4) 파이널라이저: GC가 수집할 때 실행 ==");
        for (int i = 0; i < 3; i++)
            _ = new TempResource(); // 참조가 끊김 -> GC 대상

        Console.WriteLine("  GC.Collect() 호출 전...");
        GC.Collect();       // 강제 수집 (실전에서는 권장하지 않음)
        GC.WaitForPendingFinalizers();
        Console.WriteLine("  GC.Collect() 완료");

        Console.WriteLine("\n== 5) GC 세대 확인 ==");
        var obj = new object();
        Console.WriteLine($"  방금 만든 object의 세대: Gen{GC.GetGeneration(obj)}");
        Console.WriteLine($"  총 메모리: {GC.GetTotalMemory(false) / 1024.0:F1} KB");

        Console.WriteLine("\n[요약] using으로 결정적 정리 -> GC는 관리 메모리만 담당");
    }
}

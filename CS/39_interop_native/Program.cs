using System.Runtime.InteropServices;

namespace BasicCS.Chapter39;

// ---- user32.dll: Windows 메시지 박스 P/Invoke 선언 ----
internal static partial class NativeMethods
{
    // MessageBox(핸들, 본문, 제목, 버튼 조합)
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    internal static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    // kernel32.dll: 시스템 시간을 struct로 받아온다
    [DllImport("kernel32.dll")]
    internal static extern void GetSystemTime(out SystemTime lpSystemTime);
}

// C struct SYSTEMTIME와 1:1 대응 (배치 순서 유지)
[StructLayout(LayoutKind.Sequential)]
public struct SystemTime
{
    public ushort Year;
    public ushort Month;
    public ushort DayOfWeek;
    public ushort Day;
    public ushort Hour;
    public ushort Minute;
    public ushort Second;
    public ushort Milliseconds;

    public override string ToString()
        => $"{Year:D4}-{Month:D2}-{Day:D2} {Hour:D2}:{Minute:D2}:{Second:D2}.{Milliseconds:D3}";
}

static class Program
{
    static void Main()
    {
        bool isWindows = OperatingSystem.IsWindows();

        Console.WriteLine("== 1) kernel32.GetSystemTime (네이티브 struct 반환) ==");
        if (isWindows)
        {
            NativeMethods.GetSystemTime(out var sysTime);
            Console.WriteLine($"  네이티브 시스템 시간: {sysTime}");
        }
        else
        {
            Console.WriteLine($"  (Windows 아님) 대신 관리 시간: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
        }

        Console.WriteLine("\n== 2) user32.MessageBox (Windows 전용) ==");
        if (isWindows)
        {
            // MB_OK(0x0) | MB_ICONINFORMATION(0x40)
            int result = NativeMethods.MessageBox(IntPtr.Zero, "P/Invoke로 호출된 네이티브 메시지 박스", "C# Interop", 0x40);
            Console.WriteLine($"  MessageBox 반환값: {result} (1=OK)");
        }
        else
        {
            Console.WriteLine("  (Windows가 아니므로 콘솔 출력으로 대체)");
            Console.WriteLine("  [메시지 박스] P/Invoke로 호출된 네이티브 메시지 박스");
        }

        Console.WriteLine("\n== 3) 마샬링 개념 정리 ==");
        Console.WriteLine("  string  <-> char*      (CharSet로 인코딩 결정)");
        Console.WriteLine("  IntPtr  <-> 포인터       (핸들/주소 전달)");
        Console.WriteLine("  struct  <-> C 구조체     ([StructLayout] 배치 지정)");
        Console.WriteLine("  out var <-> 포인터 인자  (GetSystemTime의 &lpSystemTime)");

        Console.WriteLine("\n== 4) 최신 대안: LibraryImport (소스 생성기) ==");
        Console.WriteLine("  [LibraryImport(\"user32.dll\", StringMarshalling=Utf16)]");
        Console.WriteLine("  // .NET 7+ 에서 DllImport보다 안전하고 빠른 방식입니다.");

        Console.WriteLine("\n[참고] C/C++ 공유 라이브러리는 .so(리눅스)/.dll(윈도우)로 호출");
    }
}

Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Threading

Module Program
    Sub Main()
        ' 실제로는 .resx 파일 + ResourceManager 사용 (README 참고)
        ' 여기서는 언어별 메시지 테이블을 딕셔너리로 재현
        Dim messages As New Dictionary(Of String, Dictionary(Of String, String))() From {
            {"ko", New Dictionary(Of String, String)() From {{"welcome", "환영합니다"}, {"bye", "안녕히 가세요"}}},
            {"en", New Dictionary(Of String, String)() From {{"welcome", "Welcome"}, {"bye", "Goodbye"}}},
            {"ja", New Dictionary(Of String, String)() From {{"welcome", "ようこそ"}, {"bye", "さようなら"}}}
        }

        Console.WriteLine("=== 1. 언어별 메시지 (CurrentUICulture) ===")
        For Each lang In {"ko", "en", "ja"}
            Thread.CurrentThread.CurrentUICulture = New CultureInfo(lang)
            Dim table = messages(lang)
            Console.WriteLine($"  [{lang}] {table("welcome")}, {table("bye")}")
        Next

        Console.WriteLine()
        Console.WriteLine("=== 2. 문화권별 날짜/숫자/통화 형식 ===")
        Dim price = 1234567.89
        Dim now = New DateTime(2026, 7, 31, 13, 45, 0)
        For Each lang In {"ko-KR", "en-US", "de-DE", "ja-JP"}
            Dim culture As New CultureInfo(lang)
            Console.WriteLine($"  [{lang}] 통화: {price.ToString("C", culture)}")
            Console.WriteLine($"           날짜: {now.ToString("d", culture)} / 시간: {now.ToString("T", culture)}")
        Next

        Console.WriteLine()
        Console.WriteLine("=== 3. 문화권 인식 정렬/비교 ===")
        Dim words = {"apple", "Apple", "äpfel", "Banana"}
        Dim comparer = StringComparer.Create(New CultureInfo("sv-SE"), True)
        Dim sorted = words.OrderBy(Function(w) w, comparer).ToArray()
        Console.WriteLine($"  sv-SE 정렬: {String.Join(", ", sorted)}")

        Dim ordinal = words.OrderBy(Function(w) w, StringComparer.OrdinalIgnoreCase).ToArray()
        Console.WriteLine($"  Ordinal 정렬: {String.Join(", ", ordinal)}")

        Console.WriteLine()
        Console.WriteLine("=== 4. 문자열 조합 예제 ===")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        Dim greeting = String.Format("MSG: {0}님, 잔액 {1:N0}원", "홍길동", 50000)
        Console.WriteLine($"  {greeting}")
    End Sub
End Module

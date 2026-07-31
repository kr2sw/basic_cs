Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text

Module Program
    Sub Main()
        Console.WriteLine("=== 1. StringBuilder vs 문자열 연결 ===")
        Dim iterations = 20000

        ' 문자열은 불변이므로 & 연결마다 새 객체 생성
        Dim sw = Stopwatch.StartNew()
        Dim result = ""
        For i = 1 To iterations
            result &= "a"
        Next
        sw.Stop()
        Console.WriteLine($"  String 연결: {sw.ElapsedMilliseconds}ms (길이 {result.Length})")

        ' StringBuilder는 내부 버퍼에 누적 (미리 용량 예약)
        sw.Restart()
        Dim sb As New StringBuilder(capacity:=iterations)
        For i = 1 To iterations
            sb.Append("a")
        Next
        sw.Stop()
        Console.WriteLine($"  StringBuilder: {sw.ElapsedMilliseconds}ms (길이 {sb.Length})")

        Console.WriteLine()
        Console.WriteLine("=== 2. 컬렉션 선택 (HashSet vs List) ===")
        Dim data = New List(Of Integer)()
        For i = 0 To 99999
            data.Add(i)
        Next
        Dim set = New HashSet(Of Integer)(data)
        Dim target = 50000

        sw.Restart()
        Dim inList = data.Contains(target)      ' O(n)
        sw.Stop()
        Console.WriteLine($"  List.Contains: {sw.ElapsedTicks} ticks ({inList})")

        sw.Restart()
        Dim inSet = set.Contains(target)        ' O(1)
        sw.Stop()
        Console.WriteLine($"  HashSet.Contains: {sw.ElapsedTicks} ticks ({inSet})")

        Console.WriteLine()
        Console.WriteLine("=== 3. Dictionary 키 조회 ===")
        Dim dict = New Dictionary(Of String, Integer)()
        For i = 0 To 99999
            dict.Add($"key{i}", i)
        Next

        sw.Restart()
        For i = 0 To 99999
            Dim v = dict($"key{i}")
        Next
        sw.Stop()
        Console.WriteLine($"  사전 조회 10만 회: {sw.ElapsedMilliseconds}ms")

        Console.WriteLine()
        Console.WriteLine("=== 4. GC (가비지 컬렉터) 이해 ===")
        Console.WriteLine($"  시작 메모리: {GC.GetTotalMemory(False) / 1024} KB")

        For i = 1 To 100000
            Dim tmp = New String("x"c, 100)   ' 일시 객체 다수 생성
        Next

        Console.WriteLine($"  생성 후: {GC.GetTotalMemory(False) / 1024} KB")

        GC.Collect()          ' 보통은 수동 호출 불필요 (성능 저하 원인)
        GC.WaitForPendingFinalizers()
        Console.WriteLine($"  GC.Collect 후: {GC.GetTotalMemory(False) / 1024} KB")

        Console.WriteLine($"  세대별 수집: Gen0 {GC.CollectionCount(0)}회, Gen1 {GC.CollectionCount(1)}회, Gen2 {GC.CollectionCount(2)}회")
    End Sub
End Module

Imports System
Imports System.Collections.Concurrent
Imports System.Diagnostics
Imports System.Linq
Imports System.Threading

Module Program
    Sub Main()
        Console.WriteLine("=== 1. Parallel.For (병렬 계산) ===")
        Dim squares As New ConcurrentBag(Of Integer)
        Dim sw = Stopwatch.StartNew()

        Parallel.For(1, 1000001, Sub(i) squares.Add(i * i))

        sw.Stop()
        Console.WriteLine($"병렬 계산: {squares.Count:N0}개 결과, {sw.ElapsedMilliseconds}ms")

        Console.WriteLine()
        Console.WriteLine("=== 2. PLINQ (병렬 LINQ) ===")
        Dim numbers = Enumerable.Range(1, 1000000).ToArray()

        sw.Restart()
        Dim sum = numbers.AsParallel()
                         .Where(Function(n) n Mod 2 = 0)
                         .Sum(Function(n) CLng(n))
        sw.Stop()
        Console.WriteLine($"PLINQ 짝수 합: {sum:N0} ({sw.ElapsedMilliseconds}ms)")

        ' 순서 보장이 필요하면 AsOrdered 사용
        Dim ordered = numbers.AsParallel()
                             .AsOrdered()
                             .Where(Function(n) n Mod 777 = 0)
                             .Take(5)
                             .ToArray()
        Console.WriteLine($"AsOrdered 처음 5개: {String.Join(", ", ordered)}")

        Console.WriteLine()
        Console.WriteLine("=== 3. 동기화 (Interlocked / Concurrent) ===")
        Dim counter = 0
        Parallel.For(0, 1000000, Sub(i) Interlocked.Increment(counter))
        Console.WriteLine($"Interlocked 카운터 (안전): {counter:N0}")

        Dim dict As New ConcurrentDictionary(Of String, Integer)()
        Parallel.For(0, 1000000, Sub(i)
            dict.AddOrUpdate("key", 1, Function(k, v) v + 1)
        End Sub)
        Console.WriteLine($"ConcurrentDictionary 합계 (안전): {dict("key"):N0}")

        ' 스레드 안전하지 않은 공유 변수 (경쟁 조건 문제 데모)
        Dim bad = 0
        Parallel.For(0, 1000000, Sub(i) bad += 1)
        Console.WriteLine($"일반 변수 결과 (불완전 가능): {bad:N0}")

        Console.WriteLine()
        Console.WriteLine("=== 4. WithDegreeOfParallelism ===")
        sw.Restart()
        Dim count = numbers.AsParallel()
                           .WithDegreeOfParallelism(4)
                           .Count(Function(n) n Mod 2 = 0)
        sw.Stop()
        Console.WriteLine($"동시성 4로 짝수 개수: {count:N0} ({sw.ElapsedMilliseconds}ms)")
    End Sub
End Module

Imports System
Imports System.Collections.Generic
Imports System.Threading
Imports System.Threading.Tasks

Module Program
    Sub Main()
        ' VB는 Main을 Async로 선언할 수 없으므로 Task로 감싸 실행
        Task.Run(Async Function() Await RunDemoAsync()).GetAwaiter().GetResult()
        Console.WriteLine("모든 비동기 예제 완료")
    End Sub

    Async Function RunDemoAsync() As Task
        Console.WriteLine("=== 1. ValueTask (캐시 히트 = 동기 완료) ===")

        Dim cache As New Dictionary(Of String, Integer)() From {
            {"apple", 3}, {"banana", 6}
        }
        Dim a = Await GetLengthValueTaskAsync(cache, "apple")
        Dim b = Await GetLengthValueTaskAsync(cache, "kiwi")
        Console.WriteLine($"apple={a}, kiwi={b}")

        Console.WriteLine()
        Console.WriteLine("=== 2. IAsyncEnumerable (비동기 스트리밍) ===")

        Dim total = 0
        For Each v Await In ProduceNumbersAsync(5)
            total += v
            Console.WriteLine($"  받은 값: {v}")
        Next
        Console.WriteLine($"합계: {total}")

        Console.WriteLine()
        Console.WriteLine("=== 3. Progress(Of T) 진행률 보고 ===")

        Dim progress As New Progress(Of Integer)(Sub(p) Console.WriteLine($"  진행률: {p}%"))
        Await DownloadWithProgressAsync(progress)

        Console.WriteLine()
        Console.WriteLine("=== 4. CancellationToken 협조적 취소 ===")

        Dim cts As New CancellationTokenSource()
        Dim worker = Task.Run(Function() CountWithCancelAsync(cts.Token))
        Thread.Sleep(1200)
        cts.Cancel()
        Try
            Dim finished = Await worker
            Console.WriteLine($"완료: {finished}")
        Catch ex As OperationCanceledException
            Console.WriteLine("작업이 취소되었습니다.")
        End Try
    End Function

    ' ValueTask: 캐시 히트 시 동기 완료되어 할당이 거의 없음
    Async Function GetLengthValueTaskAsync(cache As Dictionary(Of String, Integer), key As String) As ValueTask(Of Integer)
        If cache.ContainsKey(key) Then
            Return cache(key)
        End If
        Await Task.Delay(100)          ' 캐시 미스 시에만 실제 비동기 작업
        cache(key) = key.Length
        Return key.Length
    End Function

    ' Async Iterator: IAsyncEnumerable(Of T) 스트리밍 생성
    Async Iterator Function ProduceNumbersAsync(max As Integer) As IAsyncEnumerable(Of Integer)
        For i = 1 To max
            Await Task.Delay(150)
            Yield i * 10
        Next
    End Function

    ' IProgress(Of T)로 진행률 보고
    Async Function DownloadWithProgressAsync(progress As IProgress(Of Integer)) As Task
        For i = 1 To 5
            Await Task.Delay(200)
            progress.Report(i * 20)
        Next
    End Function

    ' 취소 토큰으로 협조적 취소 지원
    Async Function CountWithCancelAsync(ct As CancellationToken) As Task(Of Integer)
        Dim count = 0
        For i = 1 To 100
            ct.ThrowIfCancellationRequested()
            Await Task.Delay(100)
            count += 1
        Next
        Return count
    End Function
End Module

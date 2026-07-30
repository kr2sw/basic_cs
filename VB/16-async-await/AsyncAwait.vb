Imports System
Imports System.Threading.Tasks

Module Program
    Sub Main()
        ' 비동기 메서드 실행
        Dim task = DoWorkAsync()
        Console.WriteLine("Main 계속 실행...")
        task.Wait()

        ' 여러 작업 병렬 처리
        Dim stopwatch = Stopwatch.StartNew()
        Dim tasks = {FetchDataAsync("A"), FetchDataAsync("B"), FetchDataAsync("C")}
        Task.WhenAll(tasks).Wait()
        stopwatch.Stop()
        Console.WriteLine($"모든 작업 완료: {stopwatch.ElapsedMilliseconds}ms")
    End Sub

    Async Function DoWorkAsync() As Task
        Console.WriteLine("작업 시작...")
        Await Task.Delay(1000)
        Console.WriteLine("1초 후 작업 완료!")
    End Function

    Async Function FetchDataAsync(name As String) As Task
        Console.WriteLine($"  {name} 다운로드 시작...")
        Await Task.Delay(500)
        Console.WriteLine($"  {name} 다운로드 완료!")
    End Function
End Module

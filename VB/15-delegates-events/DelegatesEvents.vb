Imports System

Module Program
    Sub Main()
        ' Delegate 예제
        Dim processor As New DataProcessor()

        ' AddHandler로 이벤트 연결
        AddHandler processor.OnProgress, AddressOf ShowProgress

        processor.ProcessData({10, 20, 30, 40, 50})

        ' RemoveHandler로 이벤트 해제
        RemoveHandler processor.OnProgress, AddressOf ShowProgress
        Console.WriteLine("이벤트 해제 후 재처리:")
        processor.ProcessData({1, 2, 3})

        ' WithEvents + Handles 예제
        Dim timer As New TimerWithEvents()
        timer.Start()
        Threading.Thread.Sleep(1100)
        timer.Stop()
    End Sub

    Sub ShowProgress(percent As Integer)
        Console.WriteLine($"진행률: {percent}%")
    End Sub
End Module

' Delegate 선언
Public Delegate Sub ProgressHandler(percent As Integer)

Public Class DataProcessor
    ' Event 선언
    Public Event OnProgress As ProgressHandler

    Public Sub ProcessData(data As Integer())
        Dim total = data.Length
        For i As Integer = 0 To total - 1
            Threading.Thread.Sleep(100)

            ' 진행률 이벤트 발생
            Dim percent = CInt((i + 1) / total * 100)
            RaiseEvent OnProgress(percent)
        Next
    End Sub
End Class

' WithEvents / Handles 예제
Public Class TimerWithEvents
    Private WithEvents _timer As New Timers.Timer(1000)

    Public Sub Start()
        _timer.Start()
    End Sub

    Public Sub Stop()
        _timer.Stop()
    End Sub

    ' WithEvents로 선언된 객체의 이벤트 처리
    Private Sub _timer_Elapsed(sender As Object,
                               e As Timers.ElapsedEventArgs) Handles _timer.Elapsed
        Console.WriteLine($"타이머 발생: {e.SignalTime}")
    End Sub
End Class

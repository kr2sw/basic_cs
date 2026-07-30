Option Strict On

Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows

Namespace Ch16
    Public Partial Class MainWindow
        Inherits Window

        Private _cts As CancellationTokenSource

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Async Sub StartWork_Click(sender As Object, e As RoutedEventArgs)
            _cts = New CancellationTokenSource()
            startBtn.IsEnabled = False
            logBox.Clear()
            statusText.Text = "작업 중..."
            progressBar.Value = 0

            Try
                Await Task.Run(Sub() DoWork(_cts.Token))
                statusText.Text = "완료!"
            Catch ex As OperationCanceledException
                statusText.Text = "취소됨"
                logBox.AppendText("작업이 취소되었습니다." & vbCrLf)
            Finally
                startBtn.IsEnabled = True
            End Try
        End Sub

        Private Sub DoWork(token As CancellationToken)
            For i As Integer = 1 To 100
                token.ThrowIfCancellationRequested()
                Thread.Sleep(50)

                Dim progress = i
                Dim message = $"처리 중... {i}%"

                Dispatcher.Invoke(Sub()
                    progressBar.Value = progress
                    logBox.AppendText($"스레드 {Environment.CurrentManagedThreadId}: {message}{vbCrLf}")
                    logBox.ScrollToEnd()
                End Sub)
            Next
        End Sub

        Private Sub Cancel_Click(sender As Object, e As RoutedEventArgs)
            If _cts IsNot Nothing Then _cts.Cancel()
        End Sub
    End Class
End Namespace

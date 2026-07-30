using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ch16;

public partial class MainWindow : Window
{
    private CancellationTokenSource? _cts;

    public MainWindow() => InitializeComponent();

    private async void StartWork_Click(object sender, RoutedEventArgs e)
    {
        _cts = new CancellationTokenSource();
        startBtn.IsEnabled = false;
        logBox.Clear();
        statusText.Text = "작업 중...";
        progressBar.Value = 0;

        try
        {
            await Task.Run(() => DoWork(_cts.Token));
            statusText.Text = "완료!";
        }
        catch (OperationCanceledException)
        {
            statusText.Text = "취소됨";
            logBox.AppendText("작업이 취소되었습니다.\n");
        }
        finally
        {
            startBtn.IsEnabled = true;
        }
    }

    private void DoWork(CancellationToken token)
    {
        for (int i = 1; i <= 100; i++)
        {
            token.ThrowIfCancellationRequested();
            Thread.Sleep(50);

            var progress = i;
            var message = $"처리 중... {i}%";

            Dispatcher.Invoke(() =>
            {
                progressBar.Value = progress;
                logBox.AppendText($"스레드 {Environment.CurrentManagedThreadId}: {message}\n");
                logBox.ScrollToEnd();
            });
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        _cts?.Cancel();
    }
}

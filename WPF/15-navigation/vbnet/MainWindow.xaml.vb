Option Strict On

Imports System.Windows
Imports System.Windows.Controls

Namespace Ch15
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            mainFrame.Navigate(New Page1())
            UpdateStatus()
        End Sub

        Private Sub Home_Click(sender As Object, e As RoutedEventArgs)
            mainFrame.Navigate(New Page1())
            UpdateStatus()
        End Sub

        Private Sub Settings_Click(sender As Object, e As RoutedEventArgs)
            mainFrame.Navigate(New Page2())
            UpdateStatus()
        End Sub

        Private Sub Back_Click(sender As Object, e As RoutedEventArgs)
            If mainFrame.CanGoBack Then
                mainFrame.GoBack()
                UpdateStatus()
            End If
        End Sub

        Private Sub Forward_Click(sender As Object, e As RoutedEventArgs)
            If mainFrame.CanGoForward Then
                mainFrame.GoForward()
                UpdateStatus()
            End If
        End Sub

        Private Sub UpdateStatus()
            statusText.Text = $"저널: 뒤로({mainFrame.CanGoBack}) / 앞으로({mainFrame.CanGoForward})"
        End Sub
    End Class
End Namespace

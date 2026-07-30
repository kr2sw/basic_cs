Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input

Namespace Ch04
    Partial Public Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Border_MouseEnter(sender As Object, e As MouseEventArgs)
            lblHover.Text = "MouseEnter: 마우스가 영역 안에 있습니다."
        End Sub

        Private Sub Border_MouseLeave(sender As Object, e As MouseEventArgs)
            lblHover.Text = "MouseLeave: 마우스가 영역을 벗어났습니다."
        End Sub

        Private Sub BtnClick_Click(sender As Object, e As RoutedEventArgs)
            Log($"Button Click: {DirectCast(sender, Button).Content}")
        End Sub

        Private Sub BtnDouble_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
            Log("DoubleClick: 더블클릭 발생")
        End Sub

        Private Sub Log(msg As String)
            lbLog.Items.Add($"[{Date.Now:HH:mm:ss}] {msg}")
            lbLog.ScrollIntoView(lbLog.Items(lbLog.Items.Count - 1))
        End Sub
    End Class
End Namespace

Option Strict On

Imports System.Windows
Imports System.Windows.Controls

Namespace Ch15
    Public Partial Class Page1
        Inherits Page

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub GoToSettings_Click(sender As Object, e As RoutedEventArgs)
            NavigationService?.Navigate(New Page2())
        End Sub
    End Class
End Namespace

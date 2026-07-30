Option Strict On

Imports System.Windows
Imports System.Windows.Controls

Namespace Ch15
    Public Partial Class Page2
        Inherits Page

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub GoToHome_Click(sender As Object, e As RoutedEventArgs)
            NavigationService?.Navigate(New Page1())
        End Sub
    End Class
End Namespace

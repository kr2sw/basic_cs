Option Strict On

Imports System.Windows

Namespace Ch31

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Light_Click(sender As Object, e As RoutedEventArgs)
            App.ApplyTheme("Themes/Light.xaml")
        End Sub

        Private Sub Dark_Click(sender As Object, e As RoutedEventArgs)
            App.ApplyTheme("Themes/Dark.xaml")
        End Sub
    End Class

End Namespace

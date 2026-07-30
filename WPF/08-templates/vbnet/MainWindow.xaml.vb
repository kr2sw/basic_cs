Option Strict On

Imports System.Windows

Namespace Ch08
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub
    End Class

    Public Class Person
        Public Property Name As String = ""
        Public Property Age As Integer
        Public Property Email As String = ""
    End Class
End Namespace

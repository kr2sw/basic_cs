Imports System.Windows

Namespace Ch01
    Partial Public Class MainWindow
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub BtnGreet_Click(sender As Object, e As RoutedEventArgs)
            Dim name = If(String.IsNullOrWhiteSpace(nameBox.Text), "World", nameBox.Text)
            MessageBox.Show($"Hello, {name}!", "WPF")
        End Sub
    End Class
End Namespace
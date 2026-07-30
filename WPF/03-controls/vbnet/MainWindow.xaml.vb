Imports System.Windows

Namespace Ch03
    Partial Public Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub BtnShow_Click(sender As Object, e As RoutedEventArgs)
            Dim rb = If(rbA.IsChecked = True, "A", "B")
            MessageBox.Show(
                $"TextBox: {txtInput.Text}" & vbCrLf &
                $"CheckBox: {chkOption.IsChecked}" & vbCrLf &
                $"RadioButton: {rb}" & vbCrLf &
                $"Slider: {sldValue.Value}",
                "컨트롤 값")
        End Sub
    End Class
End Namespace

Option Strict On

Imports System.Windows
Imports System.Windows.Media

Namespace Ch17
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub ColorPicker_ColorChanged(sender As Object, e As RoutedEventArgs)
            previewBorder.Background = New SolidColorBrush(colorPicker.SelectedColor)
            colorText.Text = $"RGB: {colorPicker.SelectedColor.R}, {colorPicker.SelectedColor.G}, {colorPicker.SelectedColor.B}"
        End Sub
    End Class
End Namespace

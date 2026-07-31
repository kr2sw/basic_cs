Option Strict On

Imports System.Windows
Imports System.Windows.Controls

Namespace Ch24

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub ReadWatermark_Click(sender As Object, e As RoutedEventArgs)
            ' 코드에서 첨부 속성 값을 읽습니다. 요소마다 다른 값을 가집니다.
            resultText.Text = String.Join(vbLf, New String() {
                $"nameBox  : {Watermark.GetText(nameBox)}",
                $"emailBox : {Watermark.GetText(emailBox)}",
                $"plainBox : {Watermark.GetText(plainBox)}  (스타일 기본값)"
            })
        End Sub
    End Class

    ' 첨부 속성(Attached Property) 정의 클래스
    Public NotInheritable Class Watermark
        ' RegisterAttached로 첨부 속성을 등록합니다.
        Public Shared ReadOnly TextProperty As DependencyProperty =
            DependencyProperty.RegisterAttached(
                "Text",
                GetType(String),
                GetType(Watermark),
                New FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender))

        Public Shared Function GetText(obj As DependencyObject) As String
            Return CStr(obj.GetValue(TextProperty))
        End Function

        Public Shared Sub SetText(obj As DependencyObject, value As String)
            obj.SetValue(TextProperty, value)
        End Sub
    End Class

End Namespace

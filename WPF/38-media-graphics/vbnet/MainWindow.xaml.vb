Option Strict On

Imports System
Imports System.Globalization
Imports System.IO
Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Media.Imaging

Namespace Ch38

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        ' RenderTargetBitmap으로 화면 밖(오프스크린) 렌더링 후 PNG 인코딩
        Private Sub Save_Click(sender As Object, e As RoutedEventArgs)
            Dim dlg As New Microsoft.Win32.SaveFileDialog() With {
                .Filter = "PNG 이미지|*.png",
                .FileName = "drawing.png"
            }

            If dlg.ShowDialog(Me) <> True Then Return

            Dim rtb As New RenderTargetBitmap(
                CInt(drawingHost.ActualWidth), CInt(drawingHost.ActualHeight),
                96, 96, PixelFormats.Pbgra32)
            rtb.Render(drawingHost)

            Dim encoder As New PngBitmapEncoder()
            encoder.Frames.Add(BitmapFrame.Create(rtb))

            Using fs = File.Create(dlg.FileName)
                encoder.Save(fs)
            End Using

            statusText.Text = $"저장됨: {dlg.FileName}"
        End Sub
    End Class

    ' 시각 요소(Visual)를 직접 관리하는 경량 렌더링 호스트
    Public Class DrawingVisualHost
        Inherits FrameworkElement

        Private ReadOnly _children As VisualCollection

        Public Sub New()
            _children = New VisualCollection(Me)
            AddDrawingVisual()
        End Sub

        Protected Overrides ReadOnly Property VisualChildrenCount As Integer
            Get
                Return _children.Count
            End Get
        End Property

        Protected Overrides Function GetVisualChild(index As Integer) As Visual
            Return _children(index)
        End Function

        Private Sub AddDrawingVisual()
            Dim visual As New DrawingVisual()
            Using dc = visual.RenderOpen()
                ' 그라데이션 배경 사각형
                Dim brush As New LinearGradientBrush(
                    Color.FromRgb(30, 60, 120), Color.FromRgb(90, 160, 220), 45.0)
                dc.DrawRectangle(brush, Nothing, New Rect(10, 10, 180, 160))

                ' 원
                dc.DrawEllipse(Brushes.Tomato, Nothing, New Point(300, 90), 70, 70)

                ' 선
                dc.DrawLine(New Pen(Brushes.White, 3), New Point(10, 180), New Point(390, 60))

                ' 텍스트 (DPI 보정 포함)
                Dim dpi = VisualTreeHelper.GetDpi(Me)
                Dim text As New FormattedText(
                    "DrawingVisual - 벡터 그래픽",
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    New Typeface("Malgun Gothic"), 16.0,
                    Brushes.White, dpi.PixelsPerDip)
                dc.DrawText(text, New Point(16, 190))
            End Using
            _children.Add(visual)
        End Sub
    End Class

End Namespace

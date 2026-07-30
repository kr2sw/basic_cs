Option Strict On

Imports System.Windows
Imports System.Windows.Media.Animation

Namespace Ch18
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub FadeIn_Click(sender As Object, e As RoutedEventArgs)
            Dim storyboard = DirectCast(Resources("FadeInStoryboard"), Storyboard)
            storyboard.Begin(animBox)
        End Sub

        Private Sub Scale_Click(sender As Object, e As RoutedEventArgs)
            Dim anim As New DoubleAnimation With {
                .From = 1.0,
                .To = 2.0,
                .Duration = New Duration(TimeSpan.FromSeconds(0.5)),
                .AutoReverse = True
            }
            scaleTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, anim)
            scaleTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, anim)
        End Sub

        Private Sub Rotate_Click(sender As Object, e As RoutedEventArgs)
            Dim anim As New DoubleAnimation With {
                .From = 0,
                .To = 360,
                .Duration = New Duration(TimeSpan.FromSeconds(1)),
                .RepeatBehavior = New RepeatBehavior(2)
            }
            rotateTransform.BeginAnimation(System.Windows.Media.RotateTransform.AngleProperty, anim)
        End Sub

        Private Sub Color_Click(sender As Object, e As RoutedEventArgs)
            Dim anim As New ColorAnimation With {
                .From = System.Windows.Media.Colors.DodgerBlue,
                .To = System.Windows.Media.Colors.Crimson,
                .Duration = New Duration(TimeSpan.FromSeconds(1)),
                .AutoReverse = True
            }
            Dim brush = TryCast(animBox.Background, System.Windows.Media.SolidColorBrush)
            If brush IsNot Nothing Then
                brush.BeginAnimation(System.Windows.Media.SolidColorBrush.ColorProperty, anim)
            End If
        End Sub

        Private Sub Reset_Click(sender As Object, e As RoutedEventArgs)
            animBox.Opacity = 1
            scaleTransform.ScaleX = 1
            scaleTransform.ScaleY = 1
            rotateTransform.Angle = 0
            animBox.Background = New System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DodgerBlue)
        End Sub
    End Class
End Namespace

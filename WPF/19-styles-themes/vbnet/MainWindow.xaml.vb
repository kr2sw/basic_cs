Option Strict On

Imports System.Windows

Namespace Ch19
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub LightTheme_Click(sender As Object, e As RoutedEventArgs)
            Dim dict As New ResourceDictionary With {
                .Source = New Uri("Themes/Light.xaml", UriKind.Relative)
            }
            System.Windows.Application.Current.Resources.MergedDictionaries(0) = dict
            statusText.Text = "현재 테마: 라이트"
        End Sub

        Private Sub DarkTheme_Click(sender As Object, e As RoutedEventArgs)
            Dim dict As New ResourceDictionary With {
                .Source = New Uri("Themes/Dark.xaml", UriKind.Relative)
            }
            System.Windows.Application.Current.Resources.MergedDictionaries(0) = dict
            statusText.Text = "현재 테마: 다크"
        End Sub
    End Class
End Namespace

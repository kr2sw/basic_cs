Option Strict On

Imports System
Imports System.Windows

Namespace Ch31

    Public Partial Class App
        Inherits Application

        Protected Overrides Sub OnStartup(e As StartupEventArgs)
            MyBase.OnStartup(e)
            ApplyTheme("Themes/Light.xaml")
        End Sub

        ' 실행 중 리소스 사전을 통째로 교체해 테마를 전환한다
        Public Shared Sub ApplyTheme(path As String)
            Resources.MergedDictionaries.Clear()
            Resources.MergedDictionaries.Add(New ResourceDictionary With {
                .Source = New Uri(path, UriKind.Relative)
            })
        End Sub
    End Class

End Namespace

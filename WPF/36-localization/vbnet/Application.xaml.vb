Option Strict On

Imports System
Imports System.Globalization
Imports System.Windows

Namespace Ch36

    Public Partial Class App
        Inherits Application

        Protected Overrides Sub OnStartup(e As StartupEventArgs)
            MyBase.OnStartup(e)
            Localization.SetCulture("ko-KR")
        End Sub
    End Class

    ' 문화권 변경: CurrentCulture/CurrentUICulture 설정 + 리소스 사전 교체
    Public Module Localization
        Public Sub SetCulture(name As String)
            Dim culture = CultureInfo.GetCultureInfo(name)
            CultureInfo.CurrentCulture = culture
            CultureInfo.CurrentUICulture = culture

            Application.Current.Resources.MergedDictionaries.Clear()
            Application.Current.Resources.MergedDictionaries.Add(New ResourceDictionary With {
                .Source = New Uri($"Resources/{name}.xaml", UriKind.Relative)
            })
        End Sub
    End Module

End Namespace

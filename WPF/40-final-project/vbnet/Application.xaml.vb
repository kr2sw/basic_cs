Option Strict On

Imports System
Imports System.Windows

Namespace Ch40

    Public Partial Class App
        Inherits Application

        ' 테마 전환 (31장의 패턴 재사용)
        Public Shared Sub ApplyTheme(path As String)
            Resources.MergedDictionaries.Clear()
            Resources.MergedDictionaries.Add(New ResourceDictionary With {
                .Source = New Uri(path, UriKind.Relative)
            })
        End Sub
    End Class

End Namespace

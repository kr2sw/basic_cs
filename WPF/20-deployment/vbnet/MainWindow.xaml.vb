Option Strict On

Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Windows

Namespace Ch20
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = Me
        End Sub

        Public ReadOnly Property AppName As String
            Get
                Return If(Assembly.GetExecutingAssembly().GetName().Name, "WPF App")
            End Get
        End Property

        Public ReadOnly Property Version As String
            Get
                Dim v = Assembly.GetExecutingAssembly().GetName().Version
                Return If(v Is Nothing, "1.0.0.0", v.ToString())
            End Get
        End Property

        Public ReadOnly Property Runtime As String
            Get
                Return RuntimeEnvironment.GetSystemVersion()
            End Get
        End Property

        Public ReadOnly Property OsInfo As String
            Get
                Return Environment.OSVersion.ToString()
            End Get
        End Property
    End Class
End Namespace

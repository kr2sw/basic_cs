Option Strict On

Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Input

Namespace Ch36

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New MainViewModel()
        End Sub
    End Class

    Public Class MainViewModel
        Implements INotifyPropertyChanged

        Private _name As String = "홍길동"
        Private _greeting As String = ""
        Private _cultureSample As String = ""

        Public Sub New()
            GreetCommand = New RelayCommand(Sub(p)
                Dim format = DirectCast(Application.Current.Resources("GreetingFormat"), String)
                Greeting = String.Format(CultureInfo.CurrentUICulture, format, Name)
            End Sub)
            SetKoCommand = New RelayCommand(Sub(p) SetLanguage("ko-KR"))
            SetEnCommand = New RelayCommand(Sub(p) SetLanguage("en-US"))
            UpdateCultureSample()
        End Sub

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Greeting As String
            Get
                Return _greeting
            End Get
            Set(value As String)
                _greeting = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property CultureSample As String
            Get
                Return _cultureSample
            End Get
            Set(value As String)
                _cultureSample = value
                OnPropertyChanged()
            End Set
        End Property

        Public ReadOnly Property GreetCommand As RelayCommand
        Public ReadOnly Property SetKoCommand As RelayCommand
        Public ReadOnly Property SetEnCommand As RelayCommand

        Private Sub SetLanguage(name As String)
            Localization.SetCulture(name)
            UpdateCultureSample()
        End Sub

        ' 현재 문화권 형식으로 숫자/날짜를 다시 표시
        Private Sub UpdateCultureSample()
            Dim c = CultureInfo.CurrentUICulture
            CultureSample = $"{1234567.89.ToString("N2", c)} · {DateTime.Now.ToString("d", c)}"
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

    Public Class RelayCommand
        Implements ICommand

        Private ReadOnly _execute As Action(Of Object)

        Public Sub New(execute As Action(Of Object))
            _execute = execute
        End Sub

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return True
        End Function

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            _execute(parameter)
        End Sub
    End Class

End Namespace

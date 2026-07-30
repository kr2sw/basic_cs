Option Strict On

Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Input

Namespace Ch11
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New MainViewModel()
        End Sub
    End Class

    Public Class RelayCommand
        Implements ICommand

        Private ReadOnly _execute As Action(Of Object)
        Private ReadOnly _canExecute As Func(Of Object, Boolean)

        Public Sub New(execute As Action(Of Object), Optional canExecute As Func(Of Object, Boolean) = Nothing)
            _execute = execute
            _canExecute = canExecute
        End Sub

        Public Custom Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
            AddHandler(value As EventHandler)
                AddHandler CommandManager.RequerySuggested, value
            End AddHandler
            RemoveHandler(value As EventHandler)
                RemoveHandler CommandManager.RequerySuggested, value
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
            End RaiseEvent
        End Event

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return If(_canExecute Is Nothing, True, _canExecute(parameter))
        End Function

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            _execute(parameter)
        End Sub
    End Class

    Public Class MainViewModel
        Implements INotifyPropertyChanged

        Private _name As String = ""
        Private _description As String = ""
        Private _output As String = ""

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Description As String
            Get
                Return _description
            End Get
            Set(value As String)
                _description = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Output As String
            Get
                Return _output
            End Get
            Set(value As String)
                _output = value
                OnPropertyChanged()
            End Set
        End Property

        Public ReadOnly Property ShowCommand As ICommand
        Public ReadOnly Property ClearCommand As ICommand

        Public Sub New()
            ShowCommand = New RelayCommand(AddressOf Show)
            ClearCommand = New RelayCommand(AddressOf Clear)
        End Sub

        Private Sub Show(p As Object)
            Output = $"이름: {Name}{vbCrLf}설명: {Description}"
        End Sub

        Private Sub Clear(p As Object)
            Name = ""
            Description = ""
            Output = ""
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class
End Namespace

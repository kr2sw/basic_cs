Option Strict On

Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Input

Namespace Ch06
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

        Private _text As String = "Hello WPF!"
        Private _canModify As Boolean = True

        Public Property Text As String
            Get
                Return _text
            End Get
            Set(value As String)
                _text = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property CanModify As Boolean
            Get
                Return _canModify
            End Get
            Set(value As Boolean)
                _canModify = value
                OnPropertyChanged()
                CommandManager.InvalidateRequerySuggested()
            End Set
        End Property

        Public ReadOnly Property UpperCommand As ICommand
        Public ReadOnly Property LowerCommand As ICommand
        Public ReadOnly Property ClearCommand As ICommand

        Public Sub New()
            UpperCommand = New RelayCommand(AddressOf DoUpper, AddressOf CanModifyCheck)
            LowerCommand = New RelayCommand(AddressOf DoLower, AddressOf CanModifyCheck)
            ClearCommand = New RelayCommand(AddressOf DoClear, AddressOf CanModifyCheck)
        End Sub

        Private Sub DoUpper(p As Object)
            Text = Text.ToUpper()
        End Sub

        Private Sub DoLower(p As Object)
            Text = Text.ToLower()
        End Sub

        Private Sub DoClear(p As Object)
            Text = ""
        End Sub

        Private Function CanModifyCheck(p As Object) As Boolean
            Return CanModify
        End Function

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class
End Namespace

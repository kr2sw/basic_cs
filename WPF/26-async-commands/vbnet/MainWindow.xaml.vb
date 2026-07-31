Option Strict On

Imports System
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Input

Namespace Ch26

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New MainViewModel()
        End Sub
    End Class

    ' 실행 중에는 CanExecute=False가 되어 재진입을 막는 비동기 커맨드
    Public Class AsyncRelayCommand
        Implements ICommand

        Private ReadOnly _execute As Func(Of Object, Task)
        Private ReadOnly _canExecute As Predicate(Of Object)
        Private _isRunning As Boolean

        Public ReadOnly Property IsRunning As Boolean
            Get
                Return _isRunning
            End Get
        End Property

        Public Sub New(execute As Func(Of Object, Task), Optional canExecute As Predicate(Of Object) = Nothing)
            _execute = execute
            _canExecute = canExecute
        End Sub

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return (Not _isRunning) AndAlso If(_canExecute Is Nothing, True, _canExecute(parameter))
        End Function

        Public Async Sub Execute(parameter As Object) Implements ICommand.Execute
            _isRunning = True
            RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
            Try
                Await _execute(parameter)
            Finally
                _isRunning = False
                RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
            End Try
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

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return If(_canExecute Is Nothing, True, _canExecute(parameter))
        End Function

        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            _execute(parameter)
        End Sub

        Public Sub RaiseCanExecuteChanged()
            RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
        End Sub
    End Class

    Public Class MainViewModel
        Implements INotifyPropertyChanged

        Private _cts As CancellationTokenSource
        Private _progress As Integer = 0
        Private _status As String = "대기 중"
        Private _isRunning As Boolean = False

        Public Property Progress As Integer
            Get
                Return _progress
            End Get
            Set(value As Integer)
                _progress = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Status As String
            Get
                Return _status
            End Get
            Set(value As String)
                _status = value
                OnPropertyChanged()
            End Set
        End Property

        Public ReadOnly Property IsRunning As Boolean
            Get
                Return _isRunning
            End Get
        End Property

        Public ReadOnly Property RunCommand As ICommand
        Public ReadOnly Property CancelCommand As RelayCommand

        Public Sub New()
            RunCommand = New AsyncRelayCommand(AddressOf RunAsync)
            CancelCommand = New RelayCommand(AddressOf Cancel, Function(p) IsRunning)
        End Sub

        Private Async Function RunAsync(p As Object) As Task
            _cts = New CancellationTokenSource()
            Dim token = _cts.Token

            _isRunning = True
            OnPropertyChanged(NameOf(IsRunning))
            CancelCommand.RaiseCanExecuteChanged()
            Status = "작업 실행 중..."
            Progress = 0

            Try
                For i As Integer = 0 To 100 Step 5
                    token.ThrowIfCancellationRequested()
                    Await Task.Delay(120, token)
                    Progress = i
                Next
                Status = "완료!"
            Catch ex As OperationCanceledException
                Status = "취소됨"
            Finally
                _isRunning = False
                OnPropertyChanged(NameOf(IsRunning))
                CancelCommand.RaiseCanExecuteChanged()
            End Try
        End Function

        Private Sub Cancel(p As Object)
            If _cts IsNot Nothing Then _cts.Cancel()
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

End Namespace

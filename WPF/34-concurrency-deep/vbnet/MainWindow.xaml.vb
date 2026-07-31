Option Strict On

Imports System
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Threading.Tasks.Dataflow
Imports System.Windows
Imports System.Windows.Input

Namespace Ch34

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New MainViewModel()
        End Sub
    End Class

    Public Class MainViewModel
        Implements INotifyPropertyChanged

        Private ReadOnly _runCommand As AsyncRelayCommand
        Private _cts As CancellationTokenSource
        Private _sum As Integer = 0
        Private _processed As Integer = 0
        Private _status As String = "대기 중"

        Public Sub New()
            _runCommand = New AsyncRelayCommand(AddressOf RunPipelineAsync)
            CancelCommand = New RelayCommand(Sub(p) If(_cts IsNot Nothing, _cts.Cancel()))
        End Sub

        Public ReadOnly Property RunCommand As AsyncRelayCommand
            Get
                Return _runCommand
            End Get
        End Property

        Public ReadOnly Property CancelCommand As RelayCommand

        Public Property Status As String
            Get
                Return _status
            End Get
            Set(value As String)
                _status = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Processed As Integer
            Get
                Return _processed
            End Get
            Set(value As Integer)
                _processed = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Sum As Integer
            Get
                Return _sum
            End Get
            Set(value As Integer)
                _sum = value
                OnPropertyChanged()
            End Set
        End Property

        Private Async Function RunPipelineAsync(p As Object) As Task
            _cts = New CancellationTokenSource()
            Dim token = _cts.Token
            _sum = 0
            Sum = 0
            Processed = 0
            Status = "파이프라인 실행 중..."

            ' 병렬 실행 블록과 수신 블록 옵션 (취소 토큰 포함)
            Dim pipelineOptions As New ExecutionDataflowBlockOptions() With {
                .CancellationToken = token,
                .MaxDegreeOfParallelism = 4
            }
            Dim actionOptions As New ExecutionDataflowBlockOptions() With {
                .CancellationToken = token
            }

            ' 결과를 UI 스레드로 안전하게 전달
            Dim sumProgress As New Progress(Of Integer)(Sub(s) Sum = s)
            Dim countProgress As New Progress(Of Integer)(Sub(c) Processed += c)

            Dim transform As New TransformBlock(Of Integer, Integer)(
                Async Function(n As Integer) As Task(Of Integer)
                    Await Task.Delay(10, token)
                    countProgress.Report(1)
                    Return n * n
                End Function, pipelineOptions)

            Dim action As New ActionBlock(Of Integer)(
                Sub(n As Integer)
                    Dim newSum = Interlocked.Add(_sum, n)
                    sumProgress.Report(newSum)
                End Sub, actionOptions)

            transform.LinkTo(action, New DataflowLinkOptions() With {
                .PropagateCompletion = True
            })

            Try
                For i As Integer = 1 To 100
                    Await transform.SendAsync(i, token)
                Next
                transform.Complete()
                Await action.Completion   ' 처리 완료까지 대기

                Status = $"완료 - 합: {Sum:N0}"
            Catch ex As OperationCanceledException
                Status = "취소됨"
            End Try
        End Function

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

    Public Class AsyncRelayCommand
        Implements ICommand

        Private ReadOnly _execute As Func(Of Object, Task)
        Private _isRunning As Boolean = False

        Public Sub New(execute As Func(Of Object, Task))
            _execute = execute
        End Sub

        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return Not _isRunning
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

Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Input

Namespace Ch21

    Public Partial Class MainWindow
        Inherits Window

        Public ReadOnly Property Sender As SenderViewModel = New SenderViewModel()
        Public ReadOnly Property Receiver As ReceiverViewModel = New ReceiverViewModel()

        Public Sub New()
            InitializeComponent()
            DataContext = Me
        End Sub
    End Class

    ' ===== 메신저 인프라 (Mediator 패턴) =====

    ' 전송되는 메시지의 베이스 타입
    Public MustInherit Class Message
        Public Property Sender As Object
    End Class

    ' 실제로 주고받을 메시지
    Public Class TextMessage
        Inherits Message
        Public Property Text As String
        Public Property SentAt As DateTime = DateTime.Now
    End Class

    Public Interface IMessenger
        Sub Register(recipient As Object, action As Action(Of TextMessage))
        Sub Unregister(recipient As Object)
        Sub Send(message As TextMessage)
    End Interface

    ' 약한 참조 기반 메신저. 등록된 수신자가 GC되면 등록도 함께 정리됩니다.
    Public Class Messenger
        Implements IMessenger

        Public Shared ReadOnly Instance As Messenger = New Messenger()

        Private Class Registration
            Public ReadOnly Property Recipient As WeakReference
            Public ReadOnly Property Method As MethodInfo

            Public Sub New(recipient As WeakReference, method As MethodInfo)
                Me.Recipient = recipient
                Me.Method = method
            End Sub
        End Class

        Private ReadOnly _lock As New Object()
        Private ReadOnly _registrations As New List(Of Registration)()

        Public Sub Register(recipient As Object, action As Action(Of TextMessage)) Implements IMessenger.Register
            SyncLock _lock
                ' AddressOf로 넘기면 action.Method의 대상이 recipient와 일치합니다.
                _registrations.Add(New Registration(New WeakReference(recipient), action.Method))
            End SyncLock
        End Sub

        Public Sub Unregister(recipient As Object) Implements IMessenger.Unregister
            SyncLock _lock
                _registrations.RemoveAll(Function(r) r.Recipient.Target Is recipient)
            End SyncLock
        End Sub

        Public Sub Send(message As TextMessage) Implements IMessenger.Send
            Dim snapshot As Registration()
            SyncLock _lock
                snapshot = _registrations.ToArray()
            End SyncLock

            For Each reg In snapshot
                Dim target = reg.Recipient.Target
                If target Is Nothing Then
                    ' 가비지 컬렉션된 수신자의 등록은 정리한다.
                    SyncLock _lock
                        _registrations.Remove(reg)
                    End SyncLock
                    Continue For
                End If
                reg.Method.Invoke(target, New Object() {message})
            Next
        End Sub
    End Class

    ' ===== 커맨드 =====

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
    End Class

    ' ===== ViewModel =====

    Public Class SenderViewModel
        Implements INotifyPropertyChanged

        Private _messageText As String = "안녕하세요, 수신자 여러분!"

        Public Property MessageText As String
            Get
                Return _messageText
            End Get
            Set(value As String)
                _messageText = value
                OnPropertyChanged()
            End Set
        End Property

        Public ReadOnly Property SendCommand As ICommand

        Public Sub New()
            SendCommand = New RelayCommand(AddressOf Send)
        End Sub

        Private Sub Send(p As Object)
            Messenger.Instance.Send(New TextMessage() With {.Text = MessageText})
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

    Public Class ReceiverViewModel
        Implements INotifyPropertyChanged

        Private _received As String = "아직 메시지를 받지 않았습니다."
        Private _count As Integer = 0

        Public ReadOnly Property Received As String
            Get
                Return _received
            End Get
        End Property

        Public ReadOnly Property Count As Integer
            Get
                Return _count
            End Get
        End Property

        Public Sub New()
            ' AddressOf 메서드 그룹으로 등록해야 약한 참조 메신저가 정상 동작합니다.
            Messenger.Instance.Register(Me, AddressOf OnTextMessage)
        End Sub

        Private Sub OnTextMessage(m As TextMessage)
            _count += 1
            _received = $"[{_count}] {m.Text} ({m.SentAt:HH:mm:ss})"
            OnPropertyChanged(NameOf(Received))
            OnPropertyChanged(NameOf(Count))
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

End Namespace

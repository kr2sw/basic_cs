Option Strict On

Imports System
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Input

Namespace Ch32

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New MainViewModel()
        End Sub
    End Class

    Public Interface IDialogService
        Function ShowDialog(title As String, content As Object, owner As Window) As Boolean?
    End Interface

    Public Class DialogService
        Implements IDialogService

        Public Function ShowDialog(title As String, content As Object, owner As Window) As Boolean? _
            Implements IDialogService.ShowDialog
            Dim dialog As New DialogWindow() With {
                .Title = title,
                .DataContext = content,
                .Owner = owner
            }
            Return dialog.ShowDialog()
        End Function
    End Class

    Public Class NameDialogViewModel
        Implements INotifyPropertyChanged

        Private _name As String = ""

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                OnPropertyChanged()
            End Set
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

    Public Class MainViewModel
        Implements INotifyPropertyChanged

        Private ReadOnly _dialogs As IDialogService = New DialogService()
        Private _lastResult As String = "아직 대화상자를 열지 않았습니다."

        Public Property LastResult As String
            Get
                Return _lastResult
            End Get
            Set(value As String)
                _lastResult = value
                OnPropertyChanged()
            End Set
        End Property

        Public ReadOnly Property ShowDialogCommand As RelayCommand

        Public Sub New()
            ShowDialogCommand = New RelayCommand(Sub(p)
                Dim vm As New NameDialogViewModel()
                Dim result = _dialogs.ShowDialog("이름 입력", vm, Application.Current.MainWindow)
                LastResult = If(result = True, $"확인: {vm.Name}", "취소됨")
            End Sub)
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

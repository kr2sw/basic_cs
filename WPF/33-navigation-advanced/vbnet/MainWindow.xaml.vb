Option Strict On

Imports System
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input

Namespace Ch33

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            Dim navigation As New FrameNavigationService(frame)
            Dim vm As New MainViewModel(navigation)
            DataContext = vm
            vm.NavigateHome()
        End Sub

        Private Sub Back_Click(sender As Object, e As RoutedEventArgs)
            If frame.CanGoBack Then frame.GoBack()
        End Sub

        Private Sub Forward_Click(sender As Object, e As RoutedEventArgs)
            If frame.CanGoForward Then frame.GoForward()
        End Sub
    End Class

    Public Interface INavigationService
        Sub NavigateTo(viewModel As Object)
    End Interface

    Public Class FrameNavigationService
        Implements INavigationService

        Private ReadOnly _frame As Frame

        Public Sub New(frame As Frame)
            _frame = frame
        End Sub

        Public Sub NavigateTo(viewModel As Object) Implements INavigationService.NavigateTo
            ' Page.Content에 VM을 넣으면 DataType DataTemplate이 뷰를 그린다
            Dim page As New Page() With {
                .Content = viewModel
            }
            _frame.Navigate(page)
        End Sub
    End Class

    Public Class MainViewModel

        Private ReadOnly _navigation As INavigationService

        Public ReadOnly Property GoHomeCommand As RelayCommand
        Public ReadOnly Property GoSettingsCommand As RelayCommand

        Public Sub New(navigation As INavigationService)
            _navigation = navigation
            GoHomeCommand = New RelayCommand(Sub(p) NavigateHome())
            GoSettingsCommand = New RelayCommand(Sub(p) _navigation.NavigateTo(New SettingsViewModel()))
        End Sub

        Public Sub NavigateHome()
            _navigation.NavigateTo(New HomeViewModel())
        End Sub
    End Class

    Public Class HomeViewModel

        Public ReadOnly Property Greeting As String
            Get
                Return If(DateTime.Now.Hour < 12, "좋은 아침입니다.", "반갑습니다.")
            End Get
        End Property

        Public ReadOnly Property Description As String
            Get
                Return "Frame은 저널(journal)을 유지하므로 뒤로/앞으로 버튼으로 " &
                       "이전 페이지로 이동할 수 있습니다. 페이지 콘텐츠는 VM의 " &
                       "DataType DataTemplate으로 렌더링되어 뷰와 뷰 모델이 분리됩니다."
            End Get
        End Property
    End Class

    Public Class SettingsViewModel
        Implements INotifyPropertyChanged

        Private _name As String = "홍길동"
        Private _volume As Double = 50.0

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Volume As Double
            Get
                Return _volume
            End Get
            Set(value As Double)
                _volume = value
                OnPropertyChanged()
            End Set
        End Property

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

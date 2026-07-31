Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Threading

Namespace Ch30

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New MainViewModel()
        End Sub

        ' 가상화 상태에서도 10만 번째 항목까지 즉시 이동할 수 있는지 측정
        Private Sub ScrollEnd_Click(sender As Object, e As RoutedEventArgs)
            If itemList.Items.Count = 0 Then Return
            Dim sw = Stopwatch.StartNew()
            itemList.ScrollIntoView(itemList.Items(itemList.Items.Count - 1))
            Dispatcher.BeginInvoke(
                Sub() scrollInfo.Text = $"맨 끝 스크롤: {sw.ElapsedMilliseconds} ms",
                DispatcherPriority.Loaded)
        End Sub
    End Class

    Public Class ItemModel
        Public ReadOnly Property Index As Integer
        Public ReadOnly Property Name As String

        Public Sub New(index As Integer)
            Index = index
            Name = $"항목 {index}"
        End Sub
    End Class

    Public Class MainViewModel
        Implements INotifyPropertyChanged

        Private _isVirtualizing As Boolean = True
        Private _virtualizationMode As VirtualizationMode = VirtualizationMode.Recycling
        Private _scrollUnit As ScrollUnit = ScrollUnit.Pixel
        Private _items As List(Of ItemModel) = New List(Of ItemModel)()
        Private _loadInfo As String = "아직 로드되지 않음"

        Public Property IsVirtualizing As Boolean
            Get
                Return _isVirtualizing
            End Get
            Set(value As Boolean)
                _isVirtualizing = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property VirtualizationMode As VirtualizationMode
            Get
                Return _virtualizationMode
            End Get
            Set(value As VirtualizationMode)
                _virtualizationMode = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property ScrollUnit As ScrollUnit
            Get
                Return _scrollUnit
            End Get
            Set(value As ScrollUnit)
                _scrollUnit = value
                OnPropertyChanged()
            End Set
        End Property

        ' 가상화 비교를 위해 읽기 전용 컬렉션(List) 사용 - 10만 개 알림은 부담
        Public Property Items As List(Of ItemModel)
            Get
                Return _items
            End Get
            Private Set(value As List(Of ItemModel))
                _items = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property LoadInfo As String
            Get
                Return _loadInfo
            End Get
            Set(value As String)
                _loadInfo = value
                OnPropertyChanged()
            End Set
        End Property

        Public ReadOnly Property VirtualizationModes As VirtualizationMode()
            Get
                Return Enum.GetValues(Of VirtualizationMode)()
            End Get
        End Property

        Public ReadOnly Property ScrollUnits As ScrollUnit()
            Get
                Return Enum.GetValues(Of ScrollUnit)()
            End Get
        End Property

        Public ReadOnly Property LoadCommand As RelayCommand

        Public Sub New()
            LoadCommand = New RelayCommand(Sub(p) Load())
        End Sub

        Private Sub Load()
            Dim sw = Stopwatch.StartNew()
            Items = Enumerable.Range(0, 100000).Select(Function(i) New ItemModel(i)).ToList()
            sw.Stop()
            LoadInfo = $"10만 개 로드: {sw.ElapsedMilliseconds} ms"
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

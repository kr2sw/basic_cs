Option Strict On

Imports System
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input

Namespace Ch25

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New MainViewModel()
        End Sub
    End Class

    ' ===== 미니 비헤이비어 프레임워크 =====

    ' 모든 비헤이비어의 베이스. 연결된 요소의 수명주기에 맞춰 Attach/Detach됩니다.
    Public MustInherit Class Behavior
        Inherits FrameworkElement

        Public Property AssociatedObject As DependencyObject

        Public Sub Attach(obj As DependencyObject)
            If AssociatedObject Is obj Then Return
            Detach()
            AssociatedObject = obj

            ' 비헤이비어는 시각 트리에 없으므로 DataContext를 직접 동기화합니다.
            Dim fe = TryCast(obj, FrameworkElement)
            If fe IsNot Nothing Then
                DataContext = fe.DataContext
                AddHandler fe.DataContextChanged, AddressOf OnAssociatedDataContextChanged
            End If
            OnAttached()
        End Sub

        Public Sub Detach()
            Dim fe = TryCast(AssociatedObject, FrameworkElement)
            If fe IsNot Nothing Then
                RemoveHandler fe.DataContextChanged, AddressOf OnAssociatedDataContextChanged
            End If
            If AssociatedObject Is Nothing Then Return
            OnDetaching()
            AssociatedObject = Nothing
        End Sub

        Private Sub OnAssociatedDataContextChanged(sender As Object, e As DependencyPropertyChangedEventArgs)
            Dim element = TryCast(sender, FrameworkElement)
            If element IsNot Nothing Then DataContext = element.DataContext
        End Sub

        Protected Overridable Sub OnAttached()
        End Sub

        Protected Overridable Sub OnDetaching()
        End Sub
    End Class

    ' 한 요소에 여러 비헤이비어를 담는 컬렉션 (XAML 컬렉션 구문용)
    Public Class BehaviorCollection
        Inherits ObservableCollection(Of Behavior)

        Public Property Owner As DependencyObject

        Protected Overrides Sub InsertItem(index As Integer, item As Behavior)
            MyBase.InsertItem(index, item)
            item.Attach(Owner)
        End Sub

        Protected Overrides Sub RemoveItem(index As Integer)
            Me(index).Detach()
            MyBase.RemoveItem(index)
        End Sub

        Protected Overrides Sub ClearItems()
            For Each item In Me
                item.Detach()
            Next
            MyBase.ClearItems()
        End Sub
    End Class

    ' XAML에서 local:Behaviors.Behaviors 첨부 속성으로 비헤이비어를 부착
    Public NotInheritable Class Behaviors
        Public Shared ReadOnly BehaviorsProperty As DependencyProperty =
            DependencyProperty.RegisterAttached(
                "Behaviors",
                GetType(BehaviorCollection),
                GetType(Behaviors),
                New PropertyMetadata(Nothing))

        Public Shared Function GetBehaviors(obj As DependencyObject) As BehaviorCollection
            Dim collection = TryCast(obj.GetValue(BehaviorsProperty), BehaviorCollection)
            If collection Is Nothing Then
                collection = New BehaviorCollection()
                collection.Owner = obj
                obj.SetValue(BehaviorsProperty, collection)
            End If
            Return collection
        End Function

        Public Shared Sub SetBehaviors(obj As DependencyObject, value As BehaviorCollection)
            obj.SetValue(BehaviorsProperty, value)
        End Sub
    End Class

    ' ===== 실제 비헤이비어 =====

    ' 포커스를 받으면 텍스트 전체 선택
    Public Class SelectAllOnFocusBehavior
        Inherits Behavior

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            Dim element = TryCast(AssociatedObject, UIElement)
            If element IsNot Nothing Then
                AddHandler element.GotKeyboardFocus, AddressOf OnGotKeyboardFocus
            End If
        End Sub

        Protected Overrides Sub OnDetaching()
            Dim element = TryCast(AssociatedObject, UIElement)
            If element IsNot Nothing Then
                RemoveHandler element.GotKeyboardFocus, AddressOf OnGotKeyboardFocus
            End If
            MyBase.OnDetaching()
        End Sub

        Private Sub OnGotKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
            Dim textBox = TryCast(AssociatedObject, TextBox)
            If textBox IsNot Nothing Then textBox.SelectAll()
        End Sub
    End Class

    ' Enter 키를 누르면 Command 실행 (TriggerAction과 유사한 개념)
    Public Class PressEnterCommandBehavior
        Inherits Behavior

        Public Shared ReadOnly CommandProperty As DependencyProperty =
            DependencyProperty.Register(
                "Command", GetType(ICommand), GetType(PressEnterCommandBehavior),
                New PropertyMetadata(Nothing))

        Public Shared ReadOnly ParameterProperty As DependencyProperty =
            DependencyProperty.Register(
                "Parameter", GetType(Object), GetType(PressEnterCommandBehavior),
                New PropertyMetadata(Nothing))

        Public Property Command As ICommand
            Get
                Return CType(GetValue(CommandProperty), ICommand)
            End Get
            Set(value As ICommand)
                SetValue(CommandProperty, value)
            End Set
        End Property

        Public Property Parameter As Object
            Get
                Return GetValue(ParameterProperty)
            End Get
            Set(value As Object)
                SetValue(ParameterProperty, value)
            End Set
        End Property

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            Dim element = TryCast(AssociatedObject, UIElement)
            If element IsNot Nothing Then
                AddHandler element.KeyDown, AddressOf OnKeyDown
            End If
        End Sub

        Protected Overrides Sub OnDetaching()
            Dim element = TryCast(AssociatedObject, UIElement)
            If element IsNot Nothing Then
                RemoveHandler element.KeyDown, AddressOf OnKeyDown
            End If
            MyBase.OnDetaching()
        End Sub

        Private Sub OnKeyDown(sender As Object, e As KeyEventArgs)
            If e.Key = Key.Enter AndAlso Command IsNot Nothing AndAlso Command.CanExecute(Parameter) Then
                Command.Execute(Parameter)
                e.Handled = True
            End If
        End Sub
    End Class

    ' ===== 커맨드 / ViewModel =====

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

    Public Class MainViewModel
        Implements INotifyPropertyChanged

        Private _searchText As String = "WPF"
        Private _result As String = "검색어를 입력하세요."

        Public Property SearchText As String
            Get
                Return _searchText
            End Get
            Set(value As String)
                _searchText = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Result As String
            Get
                Return _result
            End Get
            Set(value As String)
                _result = value
                OnPropertyChanged()
            End Set
        End Property

        Public ReadOnly Property SearchCommand As ICommand

        Public Sub New()
            SearchCommand = New RelayCommand(AddressOf Search)
        End Sub

        Private Sub Search(p As Object)
            Result = $"검색 결과: '{SearchText}'"
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

End Namespace

Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.IO
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Data
Imports System.Windows.Input

Namespace Ch40

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()

            ' DI (35장): 컨테이너가 뷰 모델과 저장소를 조립한다
            Dim services As New ServiceCollection()
            services.AddSingleton(Of INoteStore, JsonNoteStore)()
            services.AddSingleton(Of NotesViewModel)()
            Dim provider = services.BuildServiceProvider()
            DataContext = provider.GetService(Of NotesViewModel)()
        End Sub
    End Class

    ' ---------- 모델 ----------

    Public Class Note
        Implements INotifyPropertyChanged

        Private _title As String = ""
        Private _body As String = ""
        Private _updatedAt As Date = Date.Now

        Public Property Id As Integer

        Public Property Title As String
            Get
                Return _title
            End Get
            Set(value As String)
                If _title <> value Then
                    _title = value
                    _updatedAt = Date.Now
                    OnPropertyChanged()
                    OnPropertyChanged(NameOf(UpdatedAt))
                End If
            End Set
        End Property

        Public Property Body As String
            Get
                Return _body
            End Get
            Set(value As String)
                If _body <> value Then
                    _body = value
                    _updatedAt = Date.Now
                    OnPropertyChanged()
                    OnPropertyChanged(NameOf(UpdatedAt))
                End If
            End Set
        End Property

        Public Property UpdatedAt As Date
            Get
                Return _updatedAt
            End Get
            Set(value As Date)
                _updatedAt = value
                OnPropertyChanged()
            End Set
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

    ' ---------- 저장소 (JSON 영속화) ----------

    Public Interface INoteStore
        Function Load() As List(Of Note)
        Sub Save(notes As IEnumerable(Of Note))
    End Interface

    Public Class JsonNoteStore
        Implements INoteStore

        Private ReadOnly _path As String = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "BasicCs", "notes.json")

        Public Function Load() As List(Of Note) Implements INoteStore.Load
            If Not File.Exists(_path) Then Return New List(Of Note)()
            Dim json = File.ReadAllText(_path)
            Dim notes = JsonSerializer.Deserialize(Of List(Of Note))(json)
            Return If(notes Is Nothing, New List(Of Note)(), notes)
        End Function

        Public Sub Save(notes As IEnumerable(Of Note)) Implements INoteStore.Save
            Dim dir = Path.GetDirectoryName(_path)
            If dir IsNot Nothing Then Directory.CreateDirectory(dir)
            File.WriteAllText(_path,
                JsonSerializer.Serialize(notes, New JsonSerializerOptions() With {.WriteIndented = True}))
        End Sub
    End Class

    ' ---------- 뷰 모델 (35장의 DI + 31장의 테마 + 필터링) ----------

    Public Class NotesViewModel
        Implements INotifyPropertyChanged

        Private ReadOnly _store As INoteStore
        Private ReadOnly _view As ICollectionView
        Private _selected As Note
        Private _searchText As String = ""
        Private _status As String = ""
        Private _isDark As Boolean = False

        Public ReadOnly Property Notes As ObservableCollection(Of Note)
        Public ReadOnly Property AddCommand As RelayCommand
        Public ReadOnly Property DeleteCommand As RelayCommand
        Public ReadOnly Property SaveCommand As RelayCommand
        Public ReadOnly Property ToggleThemeCommand As RelayCommand

        Public Property Selected As Note
            Get
                Return _selected
            End Get
            Set(value As Note)
                _selected = value
                OnPropertyChanged()
                DeleteCommand.RaiseCanExecuteChanged()
            End Set
        End Property

        Public Property SearchText As String
            Get
                Return _searchText
            End Get
            Set(value As String)
                _searchText = value
                OnPropertyChanged()
                _view.Refresh()   ' 필터 재적용
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

        Public Property IsDark As Boolean
            Get
                Return _isDark
            End Get
            Set(value As Boolean)
                _isDark = value
                OnPropertyChanged()
            End Set
        End Property

        Public Sub New(store As INoteStore)
            _store = store
            Notes = New ObservableCollection(Of Note)()

            For Each note In _store.Load()
                Notes.Add(note)
            Next

            _view = CollectionViewSource.GetDefaultView(Notes)
            _view.Filter = Function(o)
                If SearchText.Length = 0 Then Return True
                Dim note = DirectCast(o, Note)
                Return note.Title.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                       note.Body.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0
            End Function

            AddCommand = New RelayCommand(Sub(p) AddNote())
            DeleteCommand = New RelayCommand(Sub(p) DeleteNote(), Function(p) Selected IsNot Nothing)
            SaveCommand = New RelayCommand(Sub(p) Save())
            ToggleThemeCommand = New RelayCommand(Sub(p) ToggleTheme())

            Status = $"메모 {Notes.Count}개"
        End Sub

        Private Sub AddNote()
            Dim id = If(Notes.Count = 0, 1, Notes.Max(Function(n) n.Id) + 1)
            Dim note As New Note() With {.Id = id, .Title = "새 메모", .Body = ""}
            Notes.Add(note)
            Selected = note
            Status = "새 메모를 추가했습니다."
        End Sub

        Private Sub DeleteNote()
            If Selected Is Nothing Then Return
            Notes.Remove(Selected)
            Selected = Nothing
            Status = "메모를 삭제했습니다."
        End Sub

        Private Sub Save()
            _store.Save(Notes)
            Status = $"저장했습니다. (경로: {Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\BasicCs\notes.json)"
        End Sub

        Private Sub ToggleTheme()
            IsDark = Not IsDark
            App.ApplyTheme(If(IsDark, "Themes/Dark.xaml", "Themes/Light.xaml"))
            Status = If(IsDark, "다크 테마로 전환", "라이트 테마로 전환")
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

    ' ---------- 미니 DI 컨테이너 (35장에서 재사용) ----------

    Public Enum ServiceLifetime
        Singleton
        Transient
    End Enum

    Public Class ServiceDescriptor
        Public Property ServiceType As Type
        Public Property ImplementationType As Type
        Public Property Lifetime As ServiceLifetime
        Public Property Instance As Object
    End Class

    Public Class ServiceCollection

        Private ReadOnly _descriptors As New List(Of ServiceDescriptor)()

        Public Sub AddSingleton(Of TService As Class)()
            _descriptors.Add(New ServiceDescriptor() With {
                .ServiceType = GetType(TService),
                .ImplementationType = GetType(TService),
                .Lifetime = ServiceLifetime.Singleton
            })
        End Sub

        Public Sub AddSingleton(Of TService As Class, TImplementation As {Class, TService})()
            _descriptors.Add(New ServiceDescriptor() With {
                .ServiceType = GetType(TService),
                .ImplementationType = GetType(TImplementation),
                .Lifetime = ServiceLifetime.Singleton
            })
        End Sub

        Public Function BuildServiceProvider() As ServiceProvider
            Return New ServiceProvider(_descriptors)
        End Function
    End Class

    Public Class ServiceProvider

        Private ReadOnly _map As Dictionary(Of Type, ServiceDescriptor)

        Public Sub New(descriptors As IEnumerable(Of ServiceDescriptor))
            _map = descriptors.ToDictionary(Function(d) d.ServiceType)
        End Sub

        Public Function GetService(Of TService As Class)() As TService
            Return DirectCast(Resolve(GetType(TService)), TService)
        End Function

        Private Function Resolve(type As Type) As Object
            Dim descriptor As ServiceDescriptor = Nothing
            If Not _map.TryGetValue(type, descriptor) Then
                Throw New InvalidOperationException($"등록되지 않은 서비스: {type.Name}")
            End If

            If descriptor.Instance IsNot Nothing Then
                Return descriptor.Instance
            End If

            Dim instance = CreateInstance(descriptor.ImplementationType)

            If descriptor.Lifetime = ServiceLifetime.Singleton Then
                descriptor.Instance = instance
            End If

            Return instance
        End Function

        Private Function CreateInstance(type As Type) As Object
            Dim ctor = type.GetConstructors().
                OrderByDescending(Function(c) c.GetParameters().Length).
                First()
            Dim args = ctor.GetParameters().
                Select(Function(p) Resolve(p.ParameterType)).
                ToArray()
            Return ctor.Invoke(args)
        End Function
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

End Namespace

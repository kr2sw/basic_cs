Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Input

Namespace Ch35

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()

            Dim services As New ServiceCollection()
            services.AddSingleton(Of IClock, SystemClock)()
            services.AddSingleton(Of IGreeter, Greeter)()
            services.AddTransient(Of MainViewModel)()

            Dim provider = services.BuildServiceProvider()
            DataContext = provider.GetService(Of MainViewModel)()
        End Sub
    End Class

    ' ---------- 미니 DI 컨테이너 (원리 학습용, 순수 BCL) ----------

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

        Public Sub AddTransient(Of TService As Class)()
            _descriptors.Add(New ServiceDescriptor() With {
                .ServiceType = GetType(TService),
                .ImplementationType = GetType(TService),
                .Lifetime = ServiceLifetime.Transient
            })
        End Sub

        Public Sub AddTransient(Of TService As Class, TImplementation As {Class, TService})()
            _descriptors.Add(New ServiceDescriptor() With {
                .ServiceType = GetType(TService),
                .ImplementationType = GetType(TImplementation),
                .Lifetime = ServiceLifetime.Transient
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
                Return descriptor.Instance   ' Singleton 재사용
            End If

            Dim instance = CreateInstance(descriptor.ImplementationType)

            If descriptor.Lifetime = ServiceLifetime.Singleton Then
                descriptor.Instance = instance
            End If

            Return instance
        End Function

        ' 생성자 주입: 파라미터가 가장 많은 생성자를 선택해 의존성을 재귀 해석
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

    ' ---------- 앱 서비스 ----------

    Public Interface IClock
        ReadOnly Property Now As Date
    End Interface

    Public Class SystemClock
        Implements IClock

        Public ReadOnly Property Now As Date Implements IClock.Now
            Get
                Return Date.Now
            End Get
        End Property
    End Class

    Public Interface IGreeter
        Function Greet(name As String) As String
    End Interface

    Public Class Greeter
        Implements IGreeter

        Private ReadOnly _clock As IClock

        ' IClock이 자동으로 주입된다 (중첩 생성자 주입)
        Public Sub New(clock As IClock)
            _clock = clock
        End Sub

        Public Function Greet(name As String) As String Implements IGreeter.Greet
            Return $"{_clock.Now:HH:mm:ss} - 안녕하세요, {name}님!"
        End Function
    End Class

    Public Class MainViewModel
        Implements INotifyPropertyChanged

        Private ReadOnly _greeter As IGreeter
        Private _name As String = "홍길동"
        Private _greeting As String = ""
        Private ReadOnly _providerInfo As String

        Public Sub New(greeter As IGreeter)
            _greeter = greeter
            _providerInfo = "MainViewModel(IGreeter) ← Greeter(IClock) ← SystemClock 순으로 주입됨"
            GreetCommand = New RelayCommand(Sub(p) Greeting = _greeter.Greet(Name))
        End Sub

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Greeting As String
            Get
                Return _greeting
            End Get
            Set(value As String)
                _greeting = value
                OnPropertyChanged()
            End Set
        End Property

        Public ReadOnly Property ProviderInfo As String
            Get
                Return _providerInfo
            End Get
        End Property

        Public ReadOnly Property GreetCommand As RelayCommand

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

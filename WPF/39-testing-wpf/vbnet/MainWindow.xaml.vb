Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Globalization
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Data
Imports System.Windows.Input

Namespace Ch39

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New MainViewModel()
        End Sub
    End Class

    ' ---------- 미니 테스트 러너 (NuGet 없이 실행 가능하도록 직접 구현) ----------

    Public Class TestCase
        Public Property Name As String
        Public Property Test As Action
    End Class

    Public Class TestResult
        Public ReadOnly Property Name As String
        Public ReadOnly Property Passed As Boolean
        Public ReadOnly Property Message As String

        Public Sub New(name As String, passed As Boolean, message As String)
            Name = name
            Passed = passed
            Message = message
        End Sub
    End Class

    Public Module Assert
        Public Sub True(condition As Boolean, Optional message As String = "")
            If Not condition Then Throw New Exception(message)
        End Sub

        Public Sub False(condition As Boolean, Optional message As String = "")
            If condition Then Throw New Exception(message)
        End Sub
    End Module

    Public Module MiniTestRunner
        Public Function Run(cases As IEnumerable(Of TestCase)) As List(Of TestResult)
            Dim results As New List(Of TestResult)()
            For Each test In cases
                Try
                    test.Test()
                    results.Add(New TestResult(test.Name, True, ""))
                Catch ex As Exception
                    results.Add(New TestResult(test.Name, False, ex.Message))
                End Try
            Next
            Return results
        End Function
    End Module

    ' ---------- 테스트 대상 뷰 모델 ----------

    Public Class LoginViewModel
        Implements INotifyPropertyChanged

        Private _name As String = ""
        Private _password As String = ""
        Private _status As String = ""

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Password As String
            Get
                Return _password
            End Get
            Set(value As String)
                _password = value
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

        ' 순수 로직: Window/Control에 의존하지 않아 테스트하기 쉽다
        Public ReadOnly Property IsValid As Boolean
            Get
                Return (Not String.IsNullOrWhiteSpace(Name)) AndAlso Password.Length >= 4
            End Get
        End Property

        Public ReadOnly Property LoginCommand As RelayCommand

        Public Sub New()
            LoginCommand = New RelayCommand(Sub(p)
                Status = If(IsValid, $"환영합니다, {Name}님!", "입력이 올바르지 않습니다.")
            End Sub)
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

    ' ---------- 테스트 케이스 ----------

    Public Module LoginTests

        Public Function All() As IEnumerable(Of TestCase)
            Dim tests As New List(Of TestCase)()

            tests.Add(New TestCase() With {
                .Name = "이름이 비어 있으면 IsValid=False",
                .Test = Sub()
                            Dim vm As New LoginViewModel()
                            Assert.False(vm.IsValid)
                        End Sub
            })

            tests.Add(New TestCase() With {
                .Name = "비밀번호가 4자 미만이면 IsValid=False",
                .Test = Sub()
                            Dim vm As New LoginViewModel() With {.Name = "홍길동", .Password = "abc"}
                            Assert.False(vm.IsValid)
                        End Sub
            })

            tests.Add(New TestCase() With {
                .Name = "유효한 입력이면 IsValid=True",
                .Test = Sub()
                            Dim vm As New LoginViewModel() With {.Name = "홍길동", .Password = "pass1234"}
                            Assert.True(vm.IsValid)
                        End Sub
            })

            tests.Add(New TestCase() With {
                .Name = "LoginCommand가 성공 상태를 설정",
                .Test = Sub()
                            Dim vm As New LoginViewModel() With {.Name = "홍길동", .Password = "pass1234"}
                            vm.LoginCommand.Execute(Nothing)
                            Assert.True(vm.Status.StartsWith("환영합니다"), $"실제: {vm.Status}")
                        End Sub
            })

            Return tests
        End Function
    End Module

    ' ---------- 앱 뷰 모델 ----------

    Public Class MainViewModel
        Implements INotifyPropertyChanged

        Private _status As String = "아직 실행되지 않음"

        Public ReadOnly Property Results As ObservableCollection(Of TestResult)
        Public ReadOnly Property RunTestsCommand As RelayCommand

        Public Property Status As String
            Get
                Return _status
            End Get
            Set(value As String)
                _status = value
                OnPropertyChanged()
            End Set
        End Property

        Public Sub New()
            Results = New ObservableCollection(Of TestResult)()
            RunTestsCommand = New RelayCommand(Sub(p)
                Results.Clear()
                For Each r In MiniTestRunner.Run(LoginTests.All())
                    Results.Add(r)
                Next
                Dim passed = Results.Count(Function(r) r.Passed)
                Status = $"{passed}/{Results.Count} 통과"
            End Sub)
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

    Public Class BoolToResultConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object,
                                culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return If(DirectCast(value, Boolean), "통과", "실패")
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object,
                                    culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotSupportedException()
        End Function
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

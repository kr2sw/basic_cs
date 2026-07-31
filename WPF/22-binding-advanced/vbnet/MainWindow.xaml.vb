Option Strict On

Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Threading

Namespace Ch22

    Public Partial Class MainWindow
        Inherits Window

        Public ReadOnly Property Person As Person = New Person()

        Public Sub New()
            InitializeComponent()
            DataContext = Me

            ' PriorityBinding 데모: 1초 후 "빠른 값", 3초 후 "느린 값"이 로드된다고 가정합니다.
            Dim fastTimer = New DispatcherTimer() With {.Interval = TimeSpan.FromSeconds(1)}
            AddHandler fastTimer.Tick, Sub(s, e)
                                           Person.NicknameFast = "번개"
                                           fastTimer.Stop()
                                       End Sub
            fastTimer.Start()

            Dim slowTimer = New DispatcherTimer() With {.Interval = TimeSpan.FromSeconds(3)}
            AddHandler slowTimer.Tick, Sub(s, e)
                                           Person.NicknameSlow = "천천히"
                                           slowTimer.Stop()
                                       End Sub
            slowTimer.Start()
        End Sub

        Private Sub CommitNote_Click(sender As Object, e As RoutedEventArgs)
            ' UpdateSourceTrigger=Explicit 바인딩은 코드에서 명시적으로 갱신해야 합니다.
            Dim expr = noteBox.GetBindingExpression(TextBox.TextProperty)
            If expr IsNot Nothing Then expr.UpdateSource()
        End Sub
    End Class

    ' 두 개 이상의 값을 하나로 합치는 컨버터
    Public Class FullNameConverter
        Implements IMultiValueConverter

        Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
            If values Is Nothing OrElse values.Length < 2 Then
                Return ""
            End If
            Return $"{values(0)} {values(1)}"
        End Function

        Public Function ConvertBack(values() As Object, targetType As Type(), parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
            Throw New NotSupportedException()
        End Function
    End Class

    Public Class Person
        Implements INotifyPropertyChanged

        Private _lastName As String = "홍"
        Private _firstName As String = "길동"
        Private _note As String = ""
        Private _nicknameFast As String
        Private _nicknameSlow As String

        Public Property LastName As String
            Get
                Return _lastName
            End Get
            Set(value As String)
                _lastName = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property FirstName As String
            Get
                Return _firstName
            End Get
            Set(value As String)
                _firstName = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Note As String
            Get
                Return _note
            End Get
            Set(value As String)
                _note = value
                OnPropertyChanged()
            End Set
        End Property

        ' PriorityBinding: 처음에는 Nothing(값 없음)이라 아래 바인딩으로 대체됩니다.
        Public Property NicknameFast As String
            Get
                Return _nicknameFast
            End Get
            Set(value As String)
                _nicknameFast = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property NicknameSlow As String
            Get
                Return _nicknameSlow
            End Get
            Set(value As String)
                _nicknameSlow = value
                OnPropertyChanged()
            End Set
        End Property

        ' 최종 폴백 값 (읽기 전용)
        Public ReadOnly Property NicknameFallback As String
            Get
                Return "닉네임 없음"
            End Get
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

End Namespace

Option Strict On

Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows

Namespace Ch05
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New Person With {
                .Name = "홍길동",
                .Age = 30,
                .Email = "hong@example.com"
            }
        End Sub

        Private Sub ShowInfo_Click(sender As Object, e As RoutedEventArgs)
            Dim p = TryCast(DataContext, Person)
            If p IsNot Nothing Then
                MessageBox.Show($"이름: {p.Name}{vbCrLf}나이: {p.Age}{vbCrLf}이메일: {p.Email}", "Person 정보")
            End If
        End Sub
    End Class

    Public Class Person
        Implements INotifyPropertyChanged

        Private _name As String = ""
        Private _age As Integer
        Private _email As String = ""

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Age As Integer
            Get
                Return _age
            End Get
            Set(value As Integer)
                _age = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Email As String
            Get
                Return _email
            End Get
            Set(value As String)
                _email = value
                OnPropertyChanged()
            End Set
        End Property

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class
End Namespace

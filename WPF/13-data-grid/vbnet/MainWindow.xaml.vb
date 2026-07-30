Option Strict On

Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows

Namespace Ch13
    Public Partial Class MainWindow
        Inherits Window

        Private ReadOnly _employees As New ObservableCollection(Of Employee)()
        Private _nextId As Integer = 6

        Public Sub New()
            InitializeComponent()
            _employees.Add(New Employee With {.Id = 1, .Name = "홍길동", .Department = "개발팀", .Position = "선임", .Salary = 5000D, .IsActive = True})
            _employees.Add(New Employee With {.Id = 2, .Name = "김철수", .Department = "개발팀", .Position = "주임", .Salary = 3500D, .IsActive = True})
            _employees.Add(New Employee With {.Id = 3, .Name = "이영희", .Department = "디자인팀", .Position = "과장", .Salary = 4500D, .IsActive = True})
            _employees.Add(New Employee With {.Id = 4, .Name = "박민수", .Department = "기획팀", .Position = "대리", .Salary = 3800D, .IsActive = False})
            _employees.Add(New Employee With {.Id = 5, .Name = "정수연", .Department = "개발팀", .Position = "사원", .Salary = 2800D, .IsActive = True})
            dataGrid.ItemsSource = _employees
            UpdateStatus()
        End Sub

        Private Sub Add_Click(sender As Object, e As RoutedEventArgs)
            _employees.Add(New Employee With {
                .Id = _nextId,
                .Name = "신입사원",
                .Department = "개발팀",
                .Position = "사원",
                .Salary = 2800D,
                .IsActive = True
            })
            _nextId += 1
            UpdateStatus()
        End Sub

        Private Sub Delete_Click(sender As Object, e As RoutedEventArgs)
            Dim emp = TryCast(dataGrid.SelectedItem, Employee)
            If emp IsNot Nothing Then _employees.Remove(emp)
            UpdateStatus()
        End Sub

        Private Sub Commit_Click(sender As Object, e As RoutedEventArgs)
            dataGrid.CommitEdit()
            UpdateStatus()
        End Sub

        Private Sub UpdateStatus()
            statusText.Text = $"총 {_employees.Count}명의 직원"
        End Sub
    End Class

    Public Class Employee
        Implements INotifyPropertyChanged

        Private _id As Integer
        Private _name As String = ""
        Private _department As String = ""
        Private _position As String = ""
        Private _salary As Decimal
        Private _isActive As Boolean

        Public Property Id As Integer
            Get
                Return _id
            End Get
            Set(value As Integer)
                _id = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Department As String
            Get
                Return _department
            End Get
            Set(value As String)
                _department = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Position As String
            Get
                Return _position
            End Get
            Set(value As String)
                _position = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Salary As Decimal
            Get
                Return _salary
            End Get
            Set(value As Decimal)
                _salary = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property IsActive As Boolean
            Get
                Return _isActive
            End Get
            Set(value As Boolean)
                _isActive = value
                OnPropertyChanged()
            End Set
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class
End Namespace

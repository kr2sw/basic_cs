Option Strict On

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Globalization
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls

Namespace Ch27

    Public Partial Class MainWindow
        Inherits Window

        Private ReadOnly _person As New Person()

        Public Sub New()
            InitializeComponent()
            DataContext = _person
        End Sub

        Private Sub Save_Click(sender As Object, e As RoutedEventArgs)
            resultText.Text = If(_person.HasErrors,
                "오류가 있어 저장할 수 없습니다.",
                $"저장됨: {_person.Name}, {_person.Age}세")
        End Sub
    End Class

    Public Class Person
        Implements INotifyPropertyChanged, INotifyDataErrorInfo

        Private ReadOnly _errors As New Dictionary(Of String, List(Of String))()
        Private _name As String = ""
        Private _age As Integer = 0

        Public Sub New()
            ValidateName()
        End Sub

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                If _name = value Then Return
                _name = value
                ValidateName()
                OnPropertyChanged()
            End Set
        End Property

        Public Property Age As Integer
            Get
                Return _age
            End Get
            Set(value As Integer)
                If _age = value Then Return
                _age = value
                OnPropertyChanged()
            End Set
        End Property

        Private Sub ValidateName()
            If String.IsNullOrWhiteSpace(Name) Then
                SetErrors(NameOf(Name), New String() {"이름은 필수 입력입니다."})
            Else
                ClearErrors(NameOf(Name))
            End If
        End Sub

        Private Sub SetErrors(propertyName As String, messages As IEnumerable(Of String))
            Dim list As List(Of String) = Nothing
            If _errors.TryGetValue(propertyName, list) AndAlso list.SequenceEqual(messages) Then Return
            _errors(propertyName) = messages.ToList()
            RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(propertyName))
            OnPropertyChanged(NameOf(HasErrors))
        End Sub

        Private Sub ClearErrors(propertyName As String)
            If _errors.Remove(propertyName) Then
                RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(propertyName))
                OnPropertyChanged(NameOf(HasErrors))
            End If
        End Sub

        Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
            Get
                Return _errors.Count > 0
            End Get
        End Property

        Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
            Dim list As List(Of String) = Nothing
            If propertyName IsNot Nothing AndAlso _errors.TryGetValue(propertyName, list) Then
                Return list
            End If
            If propertyName Is Nothing Then
                Return _errors.Values.SelectMany(Function(v) v).ToList()
            End If
            Return New String() {}
        End Function

        Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) _
            Implements INotifyDataErrorInfo.ErrorsChanged
        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

    Public Class AgeRangeRule
        Inherits ValidationRule

        Public Property Min As Integer
        Public Property Max As Integer

        Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
            Dim age As Integer
            If Integer.TryParse(If(value Is Nothing, "", value.ToString()), age) AndAlso
               age >= Min AndAlso age <= Max Then
                Return ValidationResult.ValidResult
            End If
            Return New ValidationResult(False, $"나이는 {Min}~{Max} 사이여야 합니다.")
        End Function
    End Class

End Namespace

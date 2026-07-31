Option Strict On

Imports System
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Data
Imports System.Windows.Input

Namespace Ch29

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New MainViewModel()
        End Sub
    End Class

    Public Class Product
        Implements INotifyPropertyChanged

        Private _name As String = ""
        Private _category As String = ""
        Private _price As Decimal = 0D
        Private _stock As Integer = 0
        Private _isFavorite As Boolean = False

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Category As String
            Get
                Return _category
            End Get
            Set(value As String)
                _category = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Price As Decimal
            Get
                Return _price
            End Get
            Set(value As Decimal)
                _price = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Stock As Integer
            Get
                Return _stock
            End Get
            Set(value As Integer)
                _stock = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property IsFavorite As Boolean
            Get
                Return _isFavorite
            End Get
            Set(value As Boolean)
                _isFavorite = value
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

    Public Class MainViewModel
        Implements INotifyPropertyChanged

        Private _grouped As Boolean = False
        Private _status As String = "총 4개 · 그룹핑: 사용 안 함"
        Private _selectedProduct As Product

        Public ReadOnly Property Products As ObservableCollection(Of Product)

        Public ReadOnly Property ToggleGroupCommand As RelayCommand
        Public ReadOnly Property AddCommand As RelayCommand
        Public ReadOnly Property DeleteCommand As RelayCommand

        Public Property SelectedProduct As Product
            Get
                Return _selectedProduct
            End Get
            Set(value As Product)
                _selectedProduct = value
                OnPropertyChanged()
                DeleteCommand.RaiseCanExecuteChanged()
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

        Public Sub New()
            Products = New ObservableCollection(Of Product)()
            ToggleGroupCommand = New RelayCommand(Sub(p) ToggleGroup())
            AddCommand = New RelayCommand(Sub(p) AddProduct())
            DeleteCommand = New RelayCommand(Sub(p) DeleteProduct(), Function(p) SelectedProduct IsNot Nothing)

            Products.Add(New Product With {.Name = "에스프레소", .Category = "커피", .Price = 4500D, .Stock = 30, .IsFavorite = True})
            Products.Add(New Product With {.Name = "카푸치노", .Category = "커피", .Price = 5200D, .Stock = 20})
            Products.Add(New Product With {.Name = "캐모마일 티", .Category = "차", .Price = 4800D, .Stock = 15})
            Products.Add(New Product With {.Name = "레몬 에이드", .Category = "음료", .Price = 5500D, .Stock = 12, .IsFavorite = True})
        End Sub

        Private Sub ToggleGroup()
            Dim view = CollectionViewSource.GetDefaultView(Products)
            If _grouped Then
                view.GroupDescriptions.Clear()
                _grouped = False
                Status = $"총 {Products.Count}개 · 그룹핑: 사용 안 함"
            Else
                view.GroupDescriptions.Add(New PropertyGroupDescription(NameOf(Product.Category)))
                _grouped = True
                Status = $"총 {Products.Count}개 · 그룹핑: 사용 중 (카테고리별)"
            End If
        End Sub

        Private Sub AddProduct()
            Products.Add(New Product With {.Name = "새 상품", .Category = "기타", .Price = 0D, .Stock = 0})
            Status = $"총 {Products.Count}개"
        End Sub

        Private Sub DeleteProduct()
            If SelectedProduct IsNot Nothing Then
                Products.Remove(SelectedProduct)
                SelectedProduct = Nothing
                Status = $"총 {Products.Count}개"
            End If
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

End Namespace

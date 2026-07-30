Option Strict On

Imports System.Collections.ObjectModel
Imports System.Windows

Namespace Ch12
    Public Partial Class MainWindow
        Inherits Window

        Private ReadOnly _products As New ObservableCollection(Of Product)()
        Private _nextId As Integer = 4

        Public Sub New()
            InitializeComponent()
            _products.Add(New Product With {.Id = 1, .Name = "노트북", .Price = 1500000D})
            _products.Add(New Product With {.Id = 2, .Name = "마우스", .Price = 25000D})
            _products.Add(New Product With {.Id = 3, .Name = "키보드", .Price = 45000D})
            itemList.ItemsSource = _products
        End Sub

        Private Sub AddItem_Click(sender As Object, e As RoutedEventArgs)
            _products.Add(New Product With {
                .Id = _nextId,
                .Name = $"제품 {_nextId}",
                .Price = CDec(_nextId * 10000)
            })
            _nextId += 1
        End Sub

        Private Sub RemoveItem_Click(sender As Object, e As RoutedEventArgs)
            Dim p = TryCast(itemList.SelectedItem, Product)
            If p IsNot Nothing Then _products.Remove(p)
        End Sub

        Private Sub ClearAll_Click(sender As Object, e As RoutedEventArgs)
            _products.Clear()
        End Sub
    End Class

    Public Class Product
        Public Property Id As Integer
        Public Property Name As String = ""
        Public Property Price As Decimal
    End Class
End Namespace

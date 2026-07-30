using System.Collections.ObjectModel;
using System.Windows;

namespace Ch12;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<Product> _products = new();
    private int _nextId = 4;

    public MainWindow()
    {
        InitializeComponent();
        _products.Add(new Product { Id = 1, Name = "노트북", Price = 1500000 });
        _products.Add(new Product { Id = 2, Name = "마우스", Price = 25000 });
        _products.Add(new Product { Id = 3, Name = "키보드", Price = 45000 });
        itemList.ItemsSource = _products;
    }

    private void AddItem_Click(object sender, RoutedEventArgs e)
    {
        _products.Add(new Product
        {
            Id = _nextId++,
            Name = $"제품 {_nextId - 1}",
            Price = (_nextId * 10000)
        });
    }

    private void RemoveItem_Click(object sender, RoutedEventArgs e)
    {
        if (itemList.SelectedItem is Product p)
            _products.Remove(p);
    }

    private void ClearAll_Click(object sender, RoutedEventArgs e)
    {
        _products.Clear();
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}

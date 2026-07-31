using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Ch29;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}

public class Product : INotifyPropertyChanged
{
    private string _name = "";
    private string _category = "";
    private decimal _price;
    private int _stock;
    private bool _isFavorite;

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    public string Category
    {
        get => _category;
        set { _category = value; OnPropertyChanged(); }
    }

    public decimal Price
    {
        get => _price;
        set { _price = value; OnPropertyChanged(); }
    }

    public int Stock
    {
        get => _stock;
        set { _stock = value; OnPropertyChanged(); }
    }

    public bool IsFavorite
    {
        get => _isFavorite;
        set { _isFavorite = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => _execute(parameter);

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public class MainViewModel : INotifyPropertyChanged
{
    private bool _grouped;
    private string _status = "총 4개 · 그룹핑: 사용 안 함";
    private Product? _selectedProduct;

    public ObservableCollection<Product> Products { get; } = new();
    public RelayCommand ToggleGroupCommand { get; }
    public RelayCommand AddCommand { get; }
    public RelayCommand DeleteCommand { get; }

    public Product? SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            OnPropertyChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }
    }

    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public MainViewModel()
    {
        ToggleGroupCommand = new RelayCommand(_ => ToggleGroup());
        AddCommand = new RelayCommand(_ => AddProduct());
        DeleteCommand = new RelayCommand(_ => DeleteProduct(), _ => SelectedProduct is not null);

        Products.Add(new Product { Name = "에스프레소", Category = "커피", Price = 4500, Stock = 30, IsFavorite = true });
        Products.Add(new Product { Name = "카푸치노", Category = "커피", Price = 5200, Stock = 20 });
        Products.Add(new Product { Name = "캐모마일 티", Category = "차", Price = 4800, Stock = 15 });
        Products.Add(new Product { Name = "레몬 에이드", Category = "음료", Price = 5500, Stock = 12, IsFavorite = true });
    }

    private void ToggleGroup()
    {
        var view = CollectionViewSource.GetDefaultView(Products);
        if (_grouped)
        {
            view.GroupDescriptions.Clear();
            _grouped = false;
            Status = $"총 {Products.Count}개 · 그룹핑: 사용 안 함";
        }
        else
        {
            view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Product.Category)));
            _grouped = true;
            Status = $"총 {Products.Count}개 · 그룹핑: 사용 중 (카테고리별)";
        }
    }

    private void AddProduct()
    {
        Products.Add(new Product { Name = "새 상품", Category = "기타", Price = 0, Stock = 0 });
        Status = $"총 {Products.Count}개";
    }

    private void DeleteProduct()
    {
        if (SelectedProduct is not null)
        {
            Products.Remove(SelectedProduct);
            SelectedProduct = null;
            Status = $"총 {Products.Count}개";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

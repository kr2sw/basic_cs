using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Ch30;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    // 가상화 상태에서도 10만 번째 항목까지 즉시 이동할 수 있는지 측정
    private void ScrollEnd_Click(object sender, RoutedEventArgs e)
    {
        if (itemList.Items.Count == 0) return;
        var sw = Stopwatch.StartNew();
        itemList.ScrollIntoView(itemList.Items[itemList.Items.Count - 1]);
        Dispatcher.BeginInvoke(
            () => scrollInfo.Text = $"맨 끝 스크롤: {sw.ElapsedMilliseconds} ms",
            DispatcherPriority.Loaded);
    }
}

public class ItemModel
{
    public int Index { get; }
    public string Name { get; }

    public ItemModel(int index)
    {
        Index = index;
        Name = $"항목 {index}";
    }
}

public class MainViewModel : INotifyPropertyChanged
{
    private bool _isVirtualizing = true;
    private VirtualizationMode _virtualizationMode = VirtualizationMode.Recycling;
    private ScrollUnit _scrollUnit = ScrollUnit.Pixel;
    private List<ItemModel> _items = new();
    private string _loadInfo = "아직 로드되지 않음";

    public bool IsVirtualizing
    {
        get => _isVirtualizing;
        set { _isVirtualizing = value; OnPropertyChanged(); }
    }

    public VirtualizationMode VirtualizationMode
    {
        get => _virtualizationMode;
        set { _virtualizationMode = value; OnPropertyChanged(); }
    }

    public ScrollUnit ScrollUnit
    {
        get => _scrollUnit;
        set { _scrollUnit = value; OnPropertyChanged(); }
    }

    // 가상화 비교를 위해 읽기 전용 컬렉션(List) 사용 - 10만 개 알림은 부담
    public List<ItemModel> Items
    {
        get => _items;
        private set { _items = value; OnPropertyChanged(); }
    }

    public string LoadInfo
    {
        get => _loadInfo;
        set { _loadInfo = value; OnPropertyChanged(); }
    }

    public VirtualizationMode[] VirtualizationModes { get; } = Enum.GetValues<VirtualizationMode>();
    public ScrollUnit[] ScrollUnits { get; } = Enum.GetValues<ScrollUnit>();
    public RelayCommand LoadCommand { get; }

    public MainViewModel()
    {
        LoadCommand = new RelayCommand(_ => Load());
    }

    private void Load()
    {
        var sw = Stopwatch.StartNew();
        Items = Enumerable.Range(0, 100_000).Select(i => new ItemModel(i)).ToList();
        sw.Stop();
        LoadInfo = $"10만 개 로드: {sw.ElapsedMilliseconds} ms";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;

    public RelayCommand(Action<object?> execute) => _execute = execute;

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => _execute(parameter);
}

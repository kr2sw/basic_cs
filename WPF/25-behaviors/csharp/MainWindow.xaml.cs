using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ch25;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}

// ===== 미니 비헤이비어 프레임워크 =====

// 모든 비헤이비어의 베이스. 연결된 요소의 수명주기에 맞춰 Attach/Detach됩니다.
public abstract class Behavior : FrameworkElement
{
    public DependencyObject? AssociatedObject { get; private set; }

    public void Attach(DependencyObject obj)
    {
        if (AssociatedObject == obj) return;
        Detach();
        AssociatedObject = obj;

        // 비헤이비어는 시각 트리에 없으므로 DataContext를 직접 동기화합니다.
        if (obj is FrameworkElement fe)
        {
            DataContext = fe.DataContext;
            fe.DataContextChanged += OnAssociatedDataContextChanged;
        }
        OnAttached();
    }

    public void Detach()
    {
        if (AssociatedObject is FrameworkElement fe)
        {
            fe.DataContextChanged -= OnAssociatedDataContextChanged;
        }
        if (AssociatedObject is null) return;
        OnDetaching();
        AssociatedObject = null;
    }

    private void OnAssociatedDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is FrameworkElement fe)
        {
            DataContext = fe.DataContext;
        }
    }

    protected virtual void OnAttached() { }
    protected virtual void OnDetaching() { }
}

// 한 요소에 여러 비헤이비어를 담는 컬렉션 (XAML 컬렉션 구문용)
public class BehaviorCollection : ObservableCollection<Behavior>
{
    public DependencyObject? Owner { get; set; }

    protected override void InsertItem(int index, Behavior item)
    {
        base.InsertItem(index, item);
        item.Attach(Owner!);
    }

    protected override void RemoveItem(int index)
    {
        this[index].Detach();
        base.RemoveItem(index);
    }

    protected override void ClearItems()
    {
        foreach (var item in this)
        {
            item.Detach();
        }
        base.ClearItems();
    }
}

// XAML에서 local:Behaviors.Behaviors 첨부 속성으로 비헤이비어를 부착
public static class Behaviors
{
    public static readonly DependencyProperty BehaviorsProperty =
        DependencyProperty.RegisterAttached(
            "Behaviors",
            typeof(BehaviorCollection),
            typeof(Behaviors),
            new PropertyMetadata(null));

    public static BehaviorCollection GetBehaviors(DependencyObject obj)
    {
        var collection = (BehaviorCollection?)obj.GetValue(BehaviorsProperty);
        if (collection is null)
        {
            collection = new BehaviorCollection { Owner = obj };
            obj.SetValue(BehaviorsProperty, collection);
        }
        return collection;
    }

    public static void SetBehaviors(DependencyObject obj, BehaviorCollection value)
        => obj.SetValue(BehaviorsProperty, value);
}

// ===== 실제 비헤이비어 =====

// 포커스를 받으면 텍스트 전체 선택
public class SelectAllOnFocusBehavior : Behavior
{
    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is UIElement element)
        {
            element.GotKeyboardFocus += OnGotKeyboardFocus;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is UIElement element)
        {
            element.GotKeyboardFocus -= OnGotKeyboardFocus;
        }
        base.OnDetaching();
    }

    private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (AssociatedObject is TextBox textBox)
        {
            textBox.SelectAll();
        }
    }
}

// Enter 키를 누르면 Command 실행 (TriggerAction과 유사한 개념)
public class PressEnterCommandBehavior : Behavior
{
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(
            nameof(Command), typeof(ICommand), typeof(PressEnterCommandBehavior),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ParameterProperty =
        DependencyProperty.Register(
            nameof(Parameter), typeof(object), typeof(PressEnterCommandBehavior),
            new PropertyMetadata(null));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? Parameter
    {
        get => GetValue(ParameterProperty);
        set => SetValue(ParameterProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is UIElement element)
        {
            element.KeyDown += OnKeyDown;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is UIElement element)
        {
            element.KeyDown -= OnKeyDown;
        }
        base.OnDetaching();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && Command is { } command && command.CanExecute(Parameter))
        {
            command.Execute(Parameter);
            e.Handled = true;
        }
    }
}

// ===== 커맨드 / ViewModel =====

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
    public void Execute(object? parameter) => _execute(parameter);
}

public class MainViewModel : INotifyPropertyChanged
{
    private string _searchText = "WPF";
    private string _result = "검색어를 입력하세요.";

    public string SearchText
    {
        get => _searchText;
        set { _searchText = value; OnPropertyChanged(); }
    }

    public string Result
    {
        get => _result;
        set { _result = value; OnPropertyChanged(); }
    }

    public ICommand SearchCommand { get; }

    public MainViewModel()
    {
        SearchCommand = new RelayCommand(_ => Result = $"검색 결과: '{SearchText}'");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Ch40;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // DI (35장): 컨테이너가 뷰 모델과 저장소를 조립한다
        var services = new ServiceCollection();
        services.AddSingleton<INoteStore, JsonNoteStore>();
        services.AddSingleton<NotesViewModel>();
        var provider = services.BuildServiceProvider();
        DataContext = provider.GetService<NotesViewModel>();
    }
}

// ---------- 모델 ----------

public class Note : INotifyPropertyChanged
{
    private string _title = "";
    private string _body = "";
    private DateTime _updatedAt = DateTime.Now;

    public int Id { get; set; }

    public string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                _updatedAt = DateTime.Now;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UpdatedAt));
            }
        }
    }

    public string Body
    {
        get => _body;
        set
        {
            if (_body != value)
            {
                _body = value;
                _updatedAt = DateTime.Now;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UpdatedAt));
            }
        }
    }

    public DateTime UpdatedAt
    {
        get => _updatedAt;
        set { _updatedAt = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

// ---------- 저장소 (JSON 영속화) ----------

public interface INoteStore
{
    List<Note> Load();
    void Save(IEnumerable<Note> notes);
}

public class JsonNoteStore : INoteStore
{
    private readonly string _path = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "BasicCs", "notes.json");

    public List<Note> Load()
    {
        if (!File.Exists(_path)) return new List<Note>();
        var json = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
    }

    public void Save(IEnumerable<Note> notes)
    {
        var dir = Path.GetDirectoryName(_path);
        if (dir is not null) Directory.CreateDirectory(dir);
        File.WriteAllText(_path,
            JsonSerializer.Serialize(notes, new JsonSerializerOptions { WriteIndented = true }));
    }
}

// ---------- 뷰 모델 (35장의 DI + 31장의 테마 + 필터링) ----------

public class NotesViewModel : INotifyPropertyChanged
{
    private readonly INoteStore _store;
    private readonly ICollectionView _view;
    private Note? _selected;
    private string _searchText = "";
    private string _status = "";
    private bool _isDark;

    public ObservableCollection<Note> Notes { get; } = new();
    public RelayCommand AddCommand { get; }
    public RelayCommand DeleteCommand { get; }
    public RelayCommand SaveCommand { get; }
    public RelayCommand ToggleThemeCommand { get; }

    public Note? Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            OnPropertyChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            _view.Refresh();   // 필터 재적용
        }
    }

    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public bool IsDark
    {
        get => _isDark;
        set { _isDark = value; OnPropertyChanged(); }
    }

    public NotesViewModel(INoteStore store)
    {
        _store = store;

        foreach (var note in _store.Load())
        {
            Notes.Add(note);
        }

        _view = CollectionViewSource.GetDefaultView(Notes);
        _view.Filter = o =>
        {
            if (SearchText.Length == 0) return true;
            var note = (Note)o;
            return note.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                   note.Body.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        };

        AddCommand = new RelayCommand(_ => AddNote());
        DeleteCommand = new RelayCommand(_ => DeleteNote(), _ => Selected is not null);
        SaveCommand = new RelayCommand(_ => Save());
        ToggleThemeCommand = new RelayCommand(_ => ToggleTheme());

        Status = $"메모 {Notes.Count}개";
    }

    private void AddNote()
    {
        var id = Notes.Count == 0 ? 1 : Notes.Max(n => n.Id) + 1;
        var note = new Note { Id = id, Title = "새 메모", Body = "" };
        Notes.Add(note);
        Selected = note;
        Status = "새 메모를 추가했습니다.";
    }

    private void DeleteNote()
    {
        if (Selected is null) return;
        Notes.Remove(Selected);
        Selected = null;
        Status = "메모를 삭제했습니다.";
    }

    private void Save()
    {
        _store.Save(Notes);
        Status = $"저장했습니다. (경로: {Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\BasicCs\\notes.json)";
    }

    private void ToggleTheme()
    {
        IsDark = !IsDark;
        App.ApplyTheme(IsDark ? "Themes/Dark.xaml" : "Themes/Light.xaml");
        Status = IsDark ? "다크 테마로 전환" : "라이트 테마로 전환";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

// ---------- 미니 DI 컨테이너 (35장에서 재사용) ----------

public enum ServiceLifetime { Singleton, Transient }

public class ServiceDescriptor
{
    public Type ServiceType { get; init; } = typeof(object);
    public Type ImplementationType { get; init; } = typeof(object);
    public ServiceLifetime Lifetime { get; init; }
    public object? Instance { get; set; }
}

public class ServiceCollection
{
    private readonly List<ServiceDescriptor> _descriptors = new();

    public void AddSingleton<TService>() where TService : class
        => _descriptors.Add(new ServiceDescriptor
        {
            ServiceType = typeof(TService),
            ImplementationType = typeof(TService),
            Lifetime = ServiceLifetime.Singleton
        });

    public void AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
        => _descriptors.Add(new ServiceDescriptor
        {
            ServiceType = typeof(TService),
            ImplementationType = typeof(TImplementation),
            Lifetime = ServiceLifetime.Singleton
        });

    public ServiceProvider BuildServiceProvider() => new(_descriptors);
}

public class ServiceProvider
{
    private readonly Dictionary<Type, ServiceDescriptor> _map;

    public ServiceProvider(IEnumerable<ServiceDescriptor> descriptors)
        => _map = descriptors.ToDictionary(d => d.ServiceType);

    public TService GetService<TService>() where TService : class
        => (TService)Resolve(typeof(TService));

    private object Resolve(Type type)
    {
        if (!_map.TryGetValue(type, out var descriptor))
        {
            throw new InvalidOperationException($"등록되지 않은 서비스: {type.Name}");
        }

        if (descriptor.Instance is not null)
        {
            return descriptor.Instance;
        }

        var instance = CreateInstance(descriptor.ImplementationType);

        if (descriptor.Lifetime == ServiceLifetime.Singleton)
        {
            descriptor.Instance = instance;
        }

        return instance;
    }

    private object CreateInstance(Type type)
    {
        var ctor = type.GetConstructors()
            .OrderByDescending(c => c.GetParameters().Length)
            .First();
        var args = ctor.GetParameters()
            .Select(p => Resolve(p.ParameterType))
            .ToArray();
        return ctor.Invoke(args);
    }
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

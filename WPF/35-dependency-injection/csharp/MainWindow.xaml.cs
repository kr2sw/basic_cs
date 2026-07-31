using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Ch35;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // 등록: 인터페이스 -> 구현체 매핑과 수명 설정
        var services = new ServiceCollection();
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IGreeter, Greeter>();
        services.AddTransient<MainViewModel>();

        var provider = services.BuildServiceProvider();
        DataContext = provider.GetService<MainViewModel>();
    }
}

// ---------- 미니 DI 컨테이너 (원리 학습용, 순수 BCL) ----------

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

    public void AddTransient<TService>() where TService : class
        => _descriptors.Add(new ServiceDescriptor
        {
            ServiceType = typeof(TService),
            ImplementationType = typeof(TService),
            Lifetime = ServiceLifetime.Transient
        });

    public void AddTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
        => _descriptors.Add(new ServiceDescriptor
        {
            ServiceType = typeof(TService),
            ImplementationType = typeof(TImplementation),
            Lifetime = ServiceLifetime.Transient
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
            return descriptor.Instance;   // Singleton 재사용
        }

        object instance = CreateInstance(descriptor.ImplementationType);

        if (descriptor.Lifetime == ServiceLifetime.Singleton)
        {
            descriptor.Instance = instance;
        }

        return instance;
    }

    // 생성자 주입: 파라미터가 가장 많은 생성자를 선택해 의존성을 재귀 해석
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

// ---------- 앱 서비스 ----------

public interface IClock
{
    DateTime Now { get; }
}

public class SystemClock : IClock
{
    public DateTime Now => DateTime.Now;
}

public interface IGreeter
{
    string Greet(string name);
}

public class Greeter : IGreeter
{
    private readonly IClock _clock;

    // IClock이 자동으로 주입된다 (중첩 생성자 주입)
    public Greeter(IClock clock) => _clock = clock;

    public string Greet(string name) => $"{_clock.Now:HH:mm:ss} - 안녕하세요, {name}님!";
}

public class MainViewModel : INotifyPropertyChanged
{
    private readonly IGreeter _greeter;
    private string _name = "홍길동";
    private string _greeting = "";
    private readonly string _providerInfo;

    public MainViewModel(IGreeter greeter)
    {
        _greeter = greeter;
        _providerInfo = "MainViewModel(IGreeter) ← Greeter(IClock) ← SystemClock 순으로 주입됨";
        GreetCommand = new RelayCommand(_ => Greeting = _greeter.Greet(Name));
    }

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    public string Greeting
    {
        get => _greeting;
        set { _greeting = value; OnPropertyChanged(); }
    }

    public string ProviderInfo => _providerInfo;

    public RelayCommand GreetCommand { get; }

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

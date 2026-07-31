using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Ch21;

public partial class MainWindow : Window
{
    public SenderViewModel Sender { get; } = new SenderViewModel();
    public ReceiverViewModel Receiver { get; } = new ReceiverViewModel();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }
}

// ===== 메신저 인프라 (Mediator 패턴) =====

// 전송되는 메시지의 베이스 타입
public abstract class Message
{
    public object? Sender { get; set; }
}

// 실제로 주고받을 메시지
public class TextMessage : Message
{
    public string? Text { get; set; }
    public DateTime SentAt { get; set; } = DateTime.Now;
}

public interface IMessenger
{
    void Register(object recipient, Action<TextMessage> action);
    void Unregister(object recipient);
    void Send(TextMessage message);
}

// 약한 참조 기반 메신저. 등록된 수신자가 GC되면 등록도 함께 정리됩니다.
public class Messenger : IMessenger
{
    public static readonly Messenger Instance = new Messenger();

    private sealed class Registration
    {
        public Registration(WeakReference recipient, MethodInfo method)
        {
            Recipient = recipient;
            Method = method;
        }

        public WeakReference Recipient { get; }
        public MethodInfo Method { get; }
    }

    private readonly object _lock = new object();
    private readonly List<Registration> _registrations = new List<Registration>();

    public void Register(object recipient, Action<TextMessage> action)
    {
        lock (_lock)
        {
            // 메서드 그룹을 넘기면 action.Method의 대상이 recipient와 일치합니다.
            _registrations.Add(new Registration(new WeakReference(recipient), action.Method));
        }
    }

    public void Unregister(object recipient)
    {
        lock (_lock)
        {
            _registrations.RemoveAll(r => r.Recipient.Target == recipient);
        }
    }

    public void Send(TextMessage message)
    {
        Registration[] snapshot;
        lock (_lock)
        {
            snapshot = _registrations.ToArray();
        }

        foreach (var reg in snapshot)
        {
            var target = reg.Recipient.Target;
            if (target == null)
            {
                // 가비지 컬렉션된 수신자의 등록은 정리한다.
                lock (_lock)
                {
                    _registrations.Remove(reg);
                }
                continue;
            }

            reg.Method.Invoke(target, new object[] { message });
        }
    }
}

// ===== 커맨드 =====

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

// ===== ViewModel =====

public class SenderViewModel : INotifyPropertyChanged
{
    private string _messageText = "안녕하세요, 수신자 여러분!";

    public string MessageText
    {
        get => _messageText;
        set { _messageText = value; OnPropertyChanged(); }
    }

    public ICommand SendCommand { get; }

    public SenderViewModel()
    {
        SendCommand = new RelayCommand(_ =>
            Messenger.Instance.Send(new TextMessage { Text = MessageText }));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class ReceiverViewModel : INotifyPropertyChanged
{
    private string _received = "아직 메시지를 받지 않았습니다.";
    private int _count;

    public string Received => _received;
    public int Count => _count;

    public ReceiverViewModel()
    {
        // 메서드 그룹으로 등록해야 약한 참조 메신저가 정상 동작합니다.
        Messenger.Instance.Register(this, OnTextMessage);
    }

    private void OnTextMessage(TextMessage m)
    {
        _count++;
        _received = $"[{_count}] {m.Text} ({m.SentAt:HH:mm:ss})";
        OnPropertyChanged(nameof(Received));
        OnPropertyChanged(nameof(Count));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

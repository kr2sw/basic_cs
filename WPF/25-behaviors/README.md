# 25: 비헤이비어 — Behavior/TriggerAction 개념

## 학습 목표
- 컨트롤 코드를 바꾸지 않고 동작을 붙이는 **Behavior** 개념
- `Attach`/`Detach` 수명주기와 `OnAttached`/`OnDetaching`
- XAML 컬렉션 구문(`local:Behaviors.Behaviors`)으로 부착
- 이벤트 → 동작 실행(TriggerAction) 개념 이해

> 상용 패키지 `Microsoft.Xaml.Behaviors`의 `Behavior<T>`, `TriggerAction`
> 개념을 NuGet 없이 순수 WPF로 축약 구현한 예제입니다.

## 왜 비헤이비어인가

매번 같은 로직("포커스 시 전체 선택")을 각 Window에 복붙하는 대신,
동작을 캡슐화한 객체를 원하는 요소에 **선언적으로 부착**합니다.

```xml
<TextBox>
    <local:Behaviors.Behaviors>
        <local:SelectAllOnFocusBehavior/>
    </local:Behaviors.Behaviors>
</TextBox>
```

## 미니 Behavior 베이스

```csharp
public abstract class Behavior : FrameworkElement
{
    public DependencyObject? AssociatedObject { get; private set; }

    public void Attach(DependencyObject obj)
    {
        Detach();
        AssociatedObject = obj;
        if (obj is FrameworkElement fe)
        {
            DataContext = fe.DataContext;          // 시각 트리에 없으므로 동기화
            fe.DataContextChanged += OnDataContextChanged;
        }
        OnAttached();
    }

    public void Detach()
    {
        // 이벤트 해제
        OnDetaching();
        AssociatedObject = null;
    }

    protected virtual void OnAttached() { }
    protected virtual void OnDetaching() { }
}
```

## BehaviorCollection과 첨부 속성

한 요소에 여러 비헤이비어를 넣는 컬렉션을 첨부 속성으로 노출합니다.

```csharp
public class BehaviorCollection : ObservableCollection<Behavior>
{
    public DependencyObject? Owner { get; set; }

    protected override void InsertItem(int index, Behavior item)
    {
        base.InsertItem(index, item);
        item.Attach(Owner!);   // 추가되는 즉시 부착
    }
}

public static class Behaviors
{
    public static readonly DependencyProperty BehaviorsProperty =
        DependencyProperty.RegisterAttached(
            "Behaviors", typeof(BehaviorCollection), typeof(Behaviors),
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
}
```

## 구체적인 비헤이비어

```csharp
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
```

## TriggerAction 개념

이벤트(Trigger)가 발생하면 동작(Action)을 실행하는 구조입니다.
`PressEnterCommandBehavior`는 Enter 키(트리거) → `Command` 실행(액션)으로 동작합니다.

```xml
<local:PressEnterCommandBehavior Command="{Binding SearchCommand}"/>
```

```csharp
private void OnKeyDown(object sender, KeyEventArgs e)
{
    if (e.Key == Key.Enter && Command?.CanExecute(Parameter) == true)
    {
        Command.Execute(Parameter);
        e.Handled = true;
    }
}
```

## 실행

```bash
cd csharp
dotnet run
```

```bash
cd vbnet
dotnet run
```

## 정리

| 구성 요소 | 역할 |
|-----------|------|
| `Behavior` | 부착 대상의 수명주기와 이벤트를 관리하는 베이스 |
| `BehaviorCollection` | 요소당 비헤이비어 목록 |
| `Behaviors.Get/Set` | 첨부 속성으로 컬렉션 노출 |
| `SelectAllOnFocusBehavior` | 이벤트만 가로채는 간단한 예 |
| `PressEnterCommandBehavior` | 이벤트 → 커맨드(TriggerAction) 예 |

주의: 비헤이비어는 시각/논리 트리에 포함되지 않으므로
`DataContext` 상속이 자동으로 되지 않습니다. 이 예제처럼 명시적으로 동기화해야 합니다.

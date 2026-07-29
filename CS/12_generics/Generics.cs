using System;
using System.Collections.Generic;
using System.Text;

namespace BasicCS.Chapter12;

// ---- 제네릭 클래스 (Generic Class) ----
// 타입 파라미터 T를 사용하는 박스 클래스
internal class Box<T>
{
    public T Value { get; set; }

    public Box(T value) => Value = value;

    public void Display() => Console.WriteLine($"Box<{typeof(T).Name}>: {Value}");
}

// ---- 제네릭 클래스 with 다중 타입 파라미터 ----
internal class Pair<TFirst, TSecond>
{
    public TFirst First { get; }
    public TSecond Second { get; }

    public Pair(TFirst first, TSecond second)
    {
        First = first;
        Second = second;
    }

    public void Deconstruct(out TFirst first, out TSecond second)
    {
        first = First;
        second = Second;
    }

    public override string ToString() => $"({First}, {Second})";
}

// ---- where T : class 제약 조건 ----
// 참조 타입만 허용하는 리포지토리
internal class ObjectRepository<T> where T : class
{
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);

    public T Get(int index) => _items[index];

    public int Count => _items.Count;
}

// ---- where T : struct 제약 조건 ----
// 값 타입만 허용하는 Nullable 래퍼
internal class ValueWrapper<T> where T : struct
{
    public T? Value { get; }

    public ValueWrapper(T? value) => Value = value;

    public bool HasValue => Value.HasValue;

    public string Description => HasValue ? $"Value: {Value}" : "No value";
}

// ---- where T : new() 제약 조건 ----
// 기본 생성자가 있는 타입만 허용
internal class Factory<T> where T : new()
{
    public T CreateInstance() => new T();
}

// ---- 제네릭 메서드 (Generic Method) ----
internal class Utils
{
    // 두 값을 교환하는 제네릭 메서드
    public static void Swap<T>(ref T a, ref T b)
    {
        (a, b) = (b, a);
    }

    // 배열에서 최대값 찾기 (IComparable 제약)
    public static T Max<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) >= 0 ? a : b;
    }
}

// ---- 제네릭 인터페이스 ----
internal interface IRepository<T>
{
    void Add(T item);
    T Get(int id);
    IEnumerable<T> GetAll();
}

internal class InMemoryRepository<T> : IRepository<T>
{
    private readonly Dictionary<int, T> _store = new();
    private int _nextId = 1;

    public void Add(T item) => _store[_nextId++] = item;

    public T Get(int id) => _store.TryGetValue(id, out var val) ? val : throw new KeyNotFoundException();

    public IEnumerable<T> GetAll() => _store.Values;
}

internal class Generics
{
    static void Main()
    {
        // ---- 제네릭 클래스 사용 ----
        Console.WriteLine("=== Generic Class (Box<T>) ===");
        var intBox = new Box<int>(42);
        var strBox = new Box<string>("Hello Generics");
        var listBox = new Box<List<double>>(new List<double> { 1.1, 2.2, 3.3 });
        intBox.Display();
        strBox.Display();
        listBox.Display();

        // ---- 다중 타입 파라미터 ----
        Console.WriteLine("\n=== Generic Class (Pair<TFirst, TSecond>) ===");
        var pair = new Pair<string, int>("Age", 30);
        Console.WriteLine(pair);
        var (label, value) = pair;
        Console.WriteLine($"Deconstructed: label={label}, value={value}");

        // ---- where T : class ----
        Console.WriteLine("\n=== Constraint 'where T : class' ===");
        var objRepo = new ObjectRepository<string>();
        objRepo.Add("Apple");
        objRepo.Add("Banana");
        Console.WriteLine($"ObjectRepository items: {objRepo.Count}");

        // ---- where T : struct ----
        Console.WriteLine("\n=== Constraint 'where T : struct' ===");
        var intWrapper = new ValueWrapper<int>(100);
        var doubleWrapper = new ValueWrapper<double>(3.14);
        Console.WriteLine(intWrapper.Description);
        Console.WriteLine(doubleWrapper.Description);

        // ---- where T : new() ----
        Console.WriteLine("\n=== Constraint 'where T : new()' ===");
        var factory = new Factory<StringBuilder>();
        var sb = factory.CreateInstance();
        sb.Append("Created by Factory<T>");
        Console.WriteLine(sb.ToString());

        // ---- 제네릭 메서드 ----
        Console.WriteLine("\n=== Generic Method ===");
        int x = 10, y = 20;
        Console.WriteLine($"Before Swap: x={x}, y={y}");
        Utils.Swap(ref x, ref y);
        Console.WriteLine($"After Swap:  x={x}, y={y}");

        string s1 = "Alpha", s2 = "Beta";
        Console.WriteLine($"Before Swap: s1={s1}, s2={s2}");
        Utils.Swap(ref s1, ref s2);
        Console.WriteLine($"After Swap:  s1={s1}, s2={s2}");

        Console.WriteLine($"Max(10, 20): {Utils.Max(10, 20)}");
        Console.WriteLine($"Max(\"Apple\", \"Banana\"): {Utils.Max("Apple", "Banana")}");

        // ---- 제네릭 인터페이스 ----
        Console.WriteLine("\n=== Generic Interface ===");
        var repo = new InMemoryRepository<string>();
        repo.Add("First");
        repo.Add("Second");
        repo.Add("Third");
        foreach (var item in repo.GetAll())
            Console.WriteLine($"  Repository item: {item}");

        // ---- List<T> 사용 ----
        Console.WriteLine("\n=== List<T> ===");
        var fruits = new List<string> { "Apple", "Banana", "Cherry" };
        fruits.Add("Durian");
        fruits.Remove("Banana");
        fruits.ForEach(f => Console.WriteLine($"  Fruit: {f}"));
        Console.WriteLine($"Count: {fruits.Count}, Contains Apple: {fruits.Contains("Apple")}");

        // ---- Dictionary<TKey, TValue> 사용 ----
        Console.WriteLine("\n=== Dictionary<TKey, TValue> ===");
        var scores = new Dictionary<string, int>
        {
            { "Alice", 95 },
            { "Bob", 87 },
            { "Charlie", 92 }
        };
        scores["David"] = 88;
        foreach (var kvp in scores)
            Console.WriteLine($"  {kvp.Key}: {kvp.Value}");

        if (scores.TryGetValue("Bob", out int bobScore))
            Console.WriteLine($"Bob's score: {bobScore}");

        // ---- Nullable<T> (Nullable<int> == int?) ----
        Console.WriteLine("\n=== Nullable<T> ===");
        int? nullableInt = null;
        Console.WriteLine($"nullableInt.HasValue: {nullableInt.HasValue}");
        nullableInt = 42;
        Console.WriteLine($"nullableInt.Value: {nullableInt.Value}");

        int? fromString = int.TryParse("123", out int parsed) ? parsed : null;
        Console.WriteLine($"Parsed nullable: {fromString}");

        // ?? 연산자 (null-coalescing)
        int? maybeNull = null;
        int result = maybeNull ?? -1;
        Console.WriteLine($"Null-coalescing (-1): {result}");

        // ?. 연산자 (null-conditional)
        int? length = nullableInt?.ToString()?.Length;
        Console.WriteLine($"Null-conditional Length: {length}");
    }
}

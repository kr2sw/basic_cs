# 04 배열(Array)과 컬렉션(Collection)

C#의 배열과 다양한 컬렉션 타입을 학습합니다.

## 주요 개념

- **배열**: 1차원, 2차원, 가변(jagged) 배열, 인덱스(`^1`)
- **List\<T\>** — 동적 배열
- **Dictionary\<TKey, TValue\>** — 키-값 쌍
- **HashSet\<T\>** — 중복 없는 집합
- **Queue\<T\>** / **Stack\<T\>** — FIFO / LIFO

## 예제 코드

```csharp
int[] numbers = new int[5] { 10, 20, 30, 40, 50 };
Console.WriteLine($"Last element: {numbers[^1]}");

List<string> fruits = new List<string> { "Apple", "Banana", "Cherry" };
fruits.Add("Durian");

Dictionary<string, int> ages = new() { { "Alice", 30 }, { "Bob", 25 } };
```

## 실행 방법

```bash
dotnet run --project ../04_arrays_collections
```

## 핵심 요약

- 배열은 고정 크기, `List<T>`는 가변 크기입니다.
- `^1` 연산자(C# 8.0)로 배열의 마지막 요소에 접근할 수 있습니다.
- 제네릭 컬렉션은 타입 안전성을 보장합니다.

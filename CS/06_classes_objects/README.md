# 06 클래스(Class)와 객체(Object)

C# 객체지향 프로그래밍의 기본인 클래스와 객체를 학습합니다.

## 주요 개념

- 클래스 정의, 생성자, 속성, 메서드
- `this()` 생성자 체이닝
- 자동 구현 속성 (`{ get; set; }`)
- 읽기 전용 속성 (`=>`)
- 정적(static) 멤버
- `record` 타입 (C# 9.0+)
- `readonly struct`
- 확장 메서드 (extension method)
- 소멸자 (finalizer)

## 예제 코드

```csharp
class Person
{
    public string Name { get; set; }
    public bool IsAdult => Age >= 18;
    public static int Count { get; private set; } = 0;

    public Person() { Name = ""; Age = 0; Count++; }
    public void Introduce() => Console.WriteLine($"Hi, I'm {Name}");
}

record Product(string Name, decimal Price);
```

## 실행 방법

```bash
dotnet run --project ../06_classes_objects
```

## 핵심 요약

- 클래스는 참조 타입이며, `new` 키워드로 인스턴스를 생성합니다.
- `record`는 값 기반 동등성을 가진 불변 타입입니다.
- 확장 메서드는 기존 타입에 새 메서드를 추가할 수 있습니다.

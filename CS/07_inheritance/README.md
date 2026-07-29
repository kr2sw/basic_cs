# 07 상속(Inheritance)

C#의 상속과 다형성, 추상 클래스를 학습합니다.

## 주요 개념

- 기본 클래스(Base class)와 파생 클래스(Derived class)
- `virtual` / `override` — 가상 메서드 재정의
- `base` 키워드 — 부모 생성자 호출
- `sealed` 클래스 — 상속 봉인
- `abstract` 클래스와 추상 메서드
- `is` / `as` 연산자 — 타입 검사 및 캐스팅
- 다형성 (Polymorphism)

## 예제 코드

```csharp
class Animal { public virtual void MakeSound() { } }
class Dog : Animal
{
    public override void MakeSound() => Console.WriteLine("Woof!");
}
abstract class Shape { public abstract double GetArea(); }
class Circle : Shape
{
    private double Radius { get; }
    public override double GetArea() => Math.PI * Radius * Radius;
}
```

## 실행 방법

```bash
dotnet run --project ../07_inheritance
```

## 핵심 요약

- `virtual` 메서드는 파생 클래스에서 `override`로 재정의할 수 있습니다.
- `abstract` 클래스는 직접 인스턴스화할 수 없으며, 파생 클래스가 반드시 구현해야 합니다.
- `sealed` 클래스는 더 이상 상속할 수 없습니다.

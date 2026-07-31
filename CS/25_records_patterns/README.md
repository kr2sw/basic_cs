# 25: 레코드와 패턴 매칭 — Records & Pattern Matching

C# 9에서 도입된 레코드와, C#의 강력한 패턴 매칭 문법을 학습합니다.

## 레코드 (Record)

레코드는 **불변 데이터**를 다루기 위한 참조 타입입니다.

- `==`가 값 기반(value equality)으로 동작
- `with` 표현식으로 복사본 생성
- positional record는 생성자와 `Deconstruct` 자동 생성

```csharp
record Person(string Name, int Age);
var a = new Person("홍길동", 30);
var b = a with { Age = 31 };   // 복사 후 변경
```

## 패턴 매칭

- **property pattern** — 객체의 속성으로 분기
- **positional pattern** — deconstruct 후 분기
- **relational pattern** — `>` `<` 비교
- **list pattern** — 시퀀스 패턴 (`[1, 2, ..]`)
- `switch` 식과 `is` 패턴

```csharp
string describe(Person p) => p switch
{
    { Age: >= 60 } => "노인",
    { Name: "관리자" } => "관리자",
    _ => "일반",
};
```

## 실행

```bash
dotnet run
```

## 핵심 요약

- 레코드는 값 기반 동등성과 `with`로 불변 데이터를 다룹니다.
- 패턴 매칭으로 복잡한 분기 로직을 선언적으로 표현합니다.
- 레코드는 `ToString()`/`Equals`/`GetHashCode`를 자동 구현합니다.

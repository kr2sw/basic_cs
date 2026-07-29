# 18 리플렉션(Reflection)과 특성(Attributes)

C# 리플렉션으로 타입 정보를 조회하고, 사용자 정의 특성을 활용하는 방법을 학습합니다.

## 주요 개념

- `Type` 클래스 — `typeof()`, `GetType()`
- `PropertyInfo` — 속성 정보 조회
- `MethodInfo` — 메서드 정보 조회
- `FieldInfo` — 필드 정보 조회
- 사용자 정의 특성 (`[Attribute]`) 작성 및 읽기
- 내장 특성: `[Obsolete]`, `[Conditional]`
- `Activator.CreateInstance` — 타입으로 인스턴스 생성
- 리플렉션으로 private 멤버 접근

## 예제 코드

```csharp
Type calcType = typeof(SampleCalculator);
PropertyInfo[] props = calcType.GetProperties();
MethodInfo[] methods = calcType.GetMethods();
FieldInfo[] fields = calcType.GetFields();

DescriptionAttribute? desc = calcType.GetCustomAttribute<DescriptionAttribute>();

object? instance = Activator.CreateInstance(typeof(SampleCalculator));
MethodInfo? addMethod = calcType.GetMethod("Add");
addMethod?.Invoke(calc, new object[] { 10, 20 });
```

## 실행 방법

```bash
dotnet run --project ../18_reflection_attributes
```

## 핵심 요약

- 리플렉션은 런타임에 타입 정보를 조회하고 조작합니다.
- 특성은 타입/메서드 등에 메타데이터를 추가합니다.
- 리플렉션으로 private 멤버에도 접근할 수 있지만, 캡슐화를 깨므로 주의가 필요합니다.

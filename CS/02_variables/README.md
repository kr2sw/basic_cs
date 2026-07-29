# 02 변수(Variables)와 데이터 타입

C#의 다양한 데이터 타입과 변수 선언, 형변환, nullable 타입을 학습합니다.

## 주요 개념

- **정수형**: `int`, `long`, `byte`
- **실수형**: `float`, `double`, `decimal`
- **논리형**: `bool`
- **문자/문자열**: `char`, `string`
- `var` 키워드 — 컴파일 타임 타입 추론
- 암시적/명시적 형변환 (casting)
- `nullable` 타입 (`int?`)

## 예제 코드

```csharp
int age = 25;
double pi = 3.141592653589793;
bool isActive = true;
string name = "C# Programming";
var message = "Type is inferred at compile time";
int? maybeNull = null;
```

## 실행 방법

```bash
dotnet run --project ../02_variables
```

## 핵심 요약

- C#은 강타입 언어로, 모든 변수에 명확한 타입이 있습니다.
- `var`는 우변의 타입을 컴파일러가 추론하게 합니다.
- `nullable<T>` (또는 `T?`)를 사용하면 값 타입에 `null`을 할당할 수 있습니다.

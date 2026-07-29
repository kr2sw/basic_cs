# 17 직렬화 (Serialization)

C#에서 객체를 JSON, XML 등으로 직렬화하고 역직렬화하는 방법을 학습합니다.

## 주요 개념

- `System.Text.Json` — `JsonSerializer.Serialize` / `Deserialize`
- `JsonSerializerOptions` — 들여쓰기, camelCase 정책, 대소문자 무시
- `XmlSerializer` — XML 직렬화/역직렬화
- `DataContract` / `DataMember` / `IgnoreDataMember`
- 순환 참조 처리 (`ReferenceHandler.IgnoreCycles`)

## 예제 코드

```csharp
var student = new Student { Id = 101, Name = "김철수" };
string json = JsonSerializer.Serialize(student, new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
});
Student? obj = JsonSerializer.Deserialize<Student>(json);

var xmlSerializer = new XmlSerializer(typeof(Product));
xmlSerializer.Serialize(writer, product);
```

## 실행 방법

```bash
dotnet run --project ../17_serialization
```

## 핵심 요약

- `System.Text.Json`은 .NET Core 3.0+의 기본 JSON 직렬화기로 성능이 우수합니다.
- `DataContract` 특성으로 직렬화 멤버를 세밀하게 제어할 수 있습니다.
- 순환 참조가 있는 객체는 `ReferenceHandler.IgnoreCycles`로 처리합니다.

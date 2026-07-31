# 30: XML/JSON 심화 — LINQ to XML, System.Text.Json

## 소개

기초 챕터의 XML/JSON을 확장합니다. `XDocument`/`XElement`를 활용한 LINQ to XML의 고급 사용법과, .NET 표준 JSON 라이브러리인 `System.Text.Json`의 직렬화/역직렬화 및 DOM(`JsonNode`) 처리를 다룹니다.

## 주요 개념

### 1. LINQ to XML — 생성과 조회

`XElement`/`XAttribute`로 XML을 선언적으로 만들고, LINQ로 요소를 검색합니다.

```vb
Dim doc As New XDocument(
    New XElement("inventory",
        New XElement("item",
            New XAttribute("id", 1),
            New XElement("name", "노트북"),
            New XElement("price", 1500000))))

Dim names = From el In doc.Descendants("item")
            Where CInt(el.Element("price").Value) > 50000
            Select el.Element("name").Value
```

### 2. System.Text.Json — 직렬화/역직렬화

클래스를 JSON 문자열로, 문자열을 객체로 변환합니다. `WriteIndented` 옵션으로 가독성 좋게 출력합니다.

```vb
Dim options As New JsonSerializerOptions() With {.WriteIndented = True}
Dim json = JsonSerializer.Serialize(person, options)
Dim back = JsonSerializer.Deserialize(Of PersonDto)(json)
```

### 3. JsonNode — JSON DOM

문서 구조를 그대로 다루는 DOM 방식입니다. 알 수 없는/동적인 JSON을 처리할 때 유용합니다.

```vb
Dim node = JsonNode.Parse(json)
node("age") = 31
node("extra") = "동적 추가"
```

### 4. XML 직렬화 (XmlSerializer)

`XmlSerializer`로 객체 ↔ XML 변환도 가능합니다. 데이터 전송/설정 파일에 사용됩니다.

```vb
Dim serializer As New XmlSerializer(GetType(PersonDto))
```

## 실행

```bash
dotnet run
```

## 정리

- LINQ to XML은 XML을 요소 트리로 다루고 LINQ로 검색합니다.
- `System.Text.Json`은 표준 JSON 처리로 직렬화 성능이 뛰어납니다.
- `JsonNode`는 동적 JSON DOM 처리에 적합합니다.
- XML은 문서 중심, JSON은 데이터 중심 교환 형식입니다.

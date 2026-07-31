# 33: 고급 제네릭 — 제약 조건, 공변성/반공변성

## 소개

기초 챕터의 제네릭을 심화합니다. 고급 제약 조건(New, Structure 등)과 형식 안전성을 유지하며 타입을 변환하는 **공변성(Covariance)**과 **반공변성(Contravariance)**을 다룹니다.

## 주요 개념

### 1. 제약 조건 (Constraints)

제네릭 타입 파라미터에 요구사항을 추가합니다.

```vb
Function CreateInstance(Of T As New)() As T          ' 기본 생성자 필요
Function GetMax(Of T As IComparable)(a As T, b As T) ' 인터페이스 구현 필요
' As Class, As Structure, As SomeBaseClass도 가능
```

### 2. 공변성 — `Out T`

**반환** 위치에만 `T`가 등장하는 인터페이스에 `Out`을 붙이면, `T`를 더 넓은(상위) 타입으로 대입할 수 있습니다. `IEnumerable(Of Out T)`가 대표적입니다.

```vb
Dim strings As IEnumerable(Of String) = ...
Dim objects As IEnumerable(Of Object) = strings   ' 공변: String → Object
```

직접 정의:

```vb
Public Interface IProducer(Of Out T)
    Function Create() As T
End Interface
```

### 3. 반공변성 — `In T`

**입력(매개변수)** 위치에만 `T`가 등장하는 인터페이스에 `In`을 붙이면, `T`를 더 좁은(하위) 타입으로 대입할 수 있습니다. `Action(Of In T)`, `IComparer(Of In T)`가 대표적입니다.

```vb
Public Interface IConsumer(Of In T)
    Sub Use(item As T)
End Interface

Dim c As IConsumer(Of Object) = New ObjectConsumer()
Dim cs As IConsumer(Of String) = c     ' 반공변: Object → String
```

### 4. default(Of T) / Nothing

값 타입이면 0, 참조 타입이면 Nothing을 반환합니다.

```vb
Function GetDefault(Of T)() As T
    Return Nothing
End Function
```

## 실행

```bash
dotnet run
```

## 정리

- 제약은 `As New`/`As Structure`/`As Class`/인터페이스/기반 클래스로 지정합니다.
- 공변(`Out`): 반환 전용 → 상위 타입으로 업캐스트 대입 가능.
- 반공변(`In`): 입력 전용 → 하위 타입으로 다운캐스트 대입 가능.
- `IEnumerable(Of T)`/`Action(Of T)`를 떠올리면 기억하기 쉽습니다.

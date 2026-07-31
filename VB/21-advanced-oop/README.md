# 21: 고급 OOP — Advanced OOP

## 소개

기초 챕터에서 다룬 클래스, 상속, 이벤트를 한 단계 더 발전시킵니다. 제네릭 클래스 심화, 중첩 타입(Nested Type), 이벤트 상속(Event Inheritance) 패턴은 실제 프레임워크와 대규모 애플리케이션 설계에서 필수적으로 사용되는 기법입니다.

## 주요 개념

### 1. 제네릭 클래스 심화

제네릭 타입에 제약(Constraint)을 걸어 타입 파라미터의 기능을 보장할 수 있습니다. 아래는 `Class`와 `IEntity` 제약을 동시에 요구하는 저장소입니다. 제약이 있어야 `item.Id`처럼 인터페이스 멤버에 접근할 수 있습니다.

```vb
Public Interface IEntity
    ReadOnly Property Id As Integer
End Interface

Public Class Repository(Of T As {Class, IEntity})
    Public Function GetById(id As Integer) As T
        For Each item In _items
            If item.Id = id Then Return item
        Next
        Throw New NotFoundException(id)
    End Function
End Class
```

### 2. 중첩 타입 (Nested Type)

외부 클래스가 내부 타입을 소유하는 구조로, 연관된 타입을 캡슐화합니다. 저장소 전용 예외(`Repository.NotFoundException`)나 데이터 구조의 요소 타입(`Matrix.Cell`)이 대표적인 예입니다.

```vb
Public Class Matrix
    Public Structure Cell
        Public Row As Integer
        Public Col As Integer
        Public Value As Double
    End Structure
End Class

Dim cell As Matrix.Cell = matrix.GetCell(1, 1)
```

중첩 타입은 `외부클래스.내부타입` 형태로 참조하며, `Private`로 선언하면 외부에서 완전히 숨길 수도 있습니다.

### 3. 이벤트 상속 (Event Inheritance)

기반 클래스는 이벤트를 `Public Event`로 선언하고, 발생(raise) 메서드는 `Protected Overridable`로 열어 파생 클래스가 확장하도록 하는 것이 표준 .NET 이벤트 패턴입니다.

```vb
Public Class Animal
    Public Event StateChanged(previous As String, current As String)

    Protected Overridable Sub OnStateChanged(prev As String, curr As String)
        RaiseEvent StateChanged(prev, curr)
    End Sub
End Class
```

파생 클래스는 `Protected Overrides Sub OnStateChanged(...)`로 발생 로직을 확장하거나, 발생 메서드를 그대로 호출해서 이벤트를 올립니다.

```vb
Public Class Dog
    Inherits Animal

    Public Sub SetName(value As String)
        Dim prev = _name
        _name = value
        OnStateChanged(prev, value)   ' 상속된 발생 메서드 호출
    End Sub

    Protected Overrides Sub OnStateChanged(prev As String, curr As String)
        Console.WriteLine("(개 이름 변경 감지)")
        MyBase.OnStateChanged(prev, curr)
    End Sub
End Class
```

## 실행

```bash
dotnet run
```

## 정리

- 제네릭 제약으로 타입 안전성을 높이고, 중첩 타입으로 관련 타입을 캡슐화합니다.
- 이벤트는 직접 `RaiseEvent` 하기보다 `Protected Overridable` 발생 메서드를 거치는 것이 상속에 안전합니다.
- `MyBase` 키워드로 기반 클래스의 구현을 명시적으로 호출할 수 있습니다.

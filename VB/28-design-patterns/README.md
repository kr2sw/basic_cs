# 28: 디자인 패턴 — 싱글턴, 팩토리, 전략, 옵저버

## 소개

재사용 가능한 설계 기법인 GoF(Gang of Four) 디자인 패턴 중 실무에서 가장 많이 쓰이는 4가지를 다룹니다: 싱글턴(Singleton), 팩토리(Factory), 전략(Strategy), 옵저버(Observer).

## 주요 개념

### 1. 싱글턴 (Singleton)

전체 프로세스에서 **인스턴스가 단 하나**만 존재하도록 보장합니다. 생성자는 `Private`, 접근은 공유 속성으로 합니다. 멀티스레드 안전을 위해 `SyncLock`을 사용합니다.

```vb
Public NotInheritable Class Logger
    Private Shared ReadOnly _lock As New Object()
    Private Shared _instance As Logger

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property Instance As Logger
        Get
            SyncLock _lock
                If _instance Is Nothing Then _instance = New Logger()
            End SyncLock
            Return _instance
        End Get
    End Property
End Class
```

### 2. 팩토리 (Factory)

객체 생성 로직을 호출부에서 분리합니다. 종류만 전달하면 적절한 구현체가 생성됩니다.

```vb
Public Class ShapeFactory
    Public Shared Function Create(kind As String, Optional param As Double = 0) As IShape
        Select Case kind
            Case "circle" : Return New Circle(param)
            Case "square" : Return New Square(param)
            Case Else : Throw New ArgumentException(...)
        End Select
    End Function
End Class
```

### 3. 전략 (Strategy)

알고리즘을 인터페이스로 추상화해 실행 중에 교체할 수 있게 합니다.

```vb
Dim sorter As New Sorter()
sorter.SetStrategy(New BubbleSortStrategy())
sorter.SetStrategy(New QuickSortStrategy())
```

### 4. 옵저버 (Observer)

객체 상태 변화를 구독자들에게 통지합니다. VB에서는 이벤트(`Event` + `AddHandler`)가 이 패턴의 자연스러운 구현입니다.

```vb
Public Class Stock
    Public Event PriceChanged(price As Decimal)
    Public Sub UpdatePrice(newPrice As Decimal)
        _price = newPrice
        RaiseEvent PriceChanged(_price)
    End Sub
End Class

AddHandler stock.PriceChanged, AddressOf MyAlertHandler
```

## 실행

```bash
dotnet run
```

## 정리

- 싱글턴: 전역 유일 인스턴스 (로거, 설정, 캐시)
- 팩토리: 생성 로직 중앙화 → 결합도 감소
- 전략: 알고리즘 캡슐화 → 런타임 교체
- 옵저버: 이벤트/구독으로 상태 변화 전파 → 느슨한 결합

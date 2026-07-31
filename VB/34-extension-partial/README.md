# 34: 확장 메서드와 Partial — Extension Methods, Partial Class

## 소개

VB.NET의 두 가지 코드 조직 기법을 다룹니다. 이미 존재하는 타입에 새 메서드를 추가하는 **확장 메서드(Extension Methods)**와, 하나의 클래스를 여러 파일로 나누는 **Partial 클래스**입니다.

## 주요 개념

### 1. 확장 메서드 (Extension Methods)

기존 타입을 상속 없이 확장합니다. `Module` 안에서 `<Extension>` 특성이 붙은 `Public` 메서드로 정의하며, 첫 매개변수가 확장 대상 타입입니다.

```vb
Imports System.Runtime.CompilerServices

Module StringExtensions
    <Extension>
    Public Function WordCount(str As String) As Integer
        Return str.Split(" "c, StringSplitOptions.RemoveEmptyEntries).Length
    End Function
End Module

Dim n = "Hello World".WordCount()   ' 마치 String의 인스턴스 메서드처럼 호출
```

기존 메서드와 이름이 겹치면 기존 메서드가 우선합니다.

### 2. Partial 클래스

같은 클래스의 정의를 여러 파일(부분)로 나눕니다. 컴파일 시 하나로 합쳐집니다. 코드 생성기가 만든 부분과 손으로 작성한 부분을 분리할 때 유용합니다.

```vb
Partial Public Class Person
    Public Property Name As String
End Class

Partial Public Class Person          ' 다른 파일에 있어도 됨
    Public Function SayHello() As String
        Return $"안녕하세요, {Name}"
    End Function
End Class
```

### 3. Partial 메서드

`Partial Class` 안에서 선언만 하고, 다른 부분에서 구현할 수 있습니다. 구현이 없으면 호출부가 제거되어 오버헤드가 없습니다. 반드시 `Private`이며 반환값이 없어야 합니다.

```vb
Partial Public Class Person
    Private Partial Sub OnNameChanged()      ' 선언
End Class

Partial Public Class Person
    Private Sub OnNameChanged()              ' 구현
        Console.WriteLine("이름 변경됨")
    End Sub
End Class
```

## 실행

```bash
dotnet run
```

## 정리

- 확장 메서드는 `<Extension>` + Module로 정의하며 정적 메서드 문법입니다.
- Partial 클래스는 대규모 클래스를 파일 단위로 분리합니다.
- Partial 메서드는 선택적 구현(코드 생성자 패턴)에 적합합니다.
- 확장 메서드도 Module이므로 `Imports` 네임스페이스만 있으면 어디서나 호출됩니다.

# 37: 고급 테스팅 — xUnit/NUnit, Mock, 파라미터 테스트

## 소개

기초 챕터의 MSTest를 넘어 대표적인 단위 테스트 프레임워크인 xUnit과 NUnit의 개념, 의존성을 가짜로 대체하는 **Mock**, 같은 테스트를 여러 입력으로 반복하는 **파라미터 테스트**를 다룹니다. 테스트 프레임워크는 NuGet이 필요하므로 예제는 콘솔 미니 러너로 재현합니다.

## 주요 개념

### 1. AAA 패턴

모든 단위 테스트는 Arrange(준비) → Act(실행) → Assert(검증) 3단계로 구성합니다.

```vb
' Arrange
Dim calc As New Calculator()
' Act
Dim actual = calc.Add(2, 3)
' Assert
Assert.Equal(5, actual)
```

### 2. xUnit과 NUnit

xUnit:

```vb
<Fact>
Public Sub Add_ReturnsSum()
    Assert.Equal(5, New Calculator().Add(2, 3))
End Sub

<Theory>
<InlineData(2, 4)>
<InlineData(3, 9)>
Public Sub Square_ReturnsSquare(n As Integer, expected As Integer)
    Assert.Equal(expected, n * n)
End Sub
```

NUnit:

```vb
<Test>
Public Sub Add_ReturnsSum()
    Assert.That(New Calculator().Add(2, 3), [Is].EqualTo(5))
End Sub

<TestCase(2, 4)>
<TestCase(3, 9)>
Public Sub Square_ReturnsSquare(n As Integer, expected As Integer)
    Assert.That(n * n, [Is].EqualTo(expected))
End Sub
```

### 3. Mock — 의존성 대체

테스트 대상이 메일 발송 등 외부 의존성을 가지면 가짜(mock)로 대체합니다. 실패 원인을 격리하고 호출 여부를 검증합니다.

```vb
Dim mailer As New FakeEmailSender()          ' 테스트용 가짜
Dim service As New NotificationService(mailer)
service.Notify("user@example.com", "환영합니다")
Assert.True(mailer.SentCount = 1)            ' 호출 검증
```

Moq 같은 라이브러리로 `Mock(Of IEmailSender)`을 자동 생성할 수도 있습니다.

### 4. 파라미터 테스트

입력만 바뀌는 중복 테스트를 하나로 합칩니다. xUnit `[Theory]`/NUnit `[TestCase]`로 데이터를 주입합니다.

## 실행

```bash
dotnet run
```

## 정리

- AAA 패턴으로 테스트를 명확하게 작성합니다.
- xUnit은 `[Fact]`/`[Theory]`, NUnit은 `[Test]`/`[TestCase]`.
- Mock으로 외부 의존성을 대체해 관심사를 분리합니다.
- 파라미터 테스트로 중복을 제거하고 경계값을 쉽게 추가합니다.

# 39: WPF 테스팅 — MVVM 단위 테스트, UI 자동화 개념

## 학습 목표
- MVVM 덕분에 가능한 **뷰 모델 단위 테스트**
- 미니 테스트 러너로 NuGet 없이 테스트 실행
- 실무 테스트 프레임워크(xUnit/NUnit/MSTest)와 UI 자동화 개념

## 왜 MVVM이 테스트에 유리한가

뷰 모델이 `Window`, `MessageBox`, `Dispatcher`에 의존하지 않으면
UI 없이 순수 로직만 검증할 수 있습니다.

```csharp
public class LoginViewModel
{
    // 순수 로직 - Window/Control 의존 없음
    public bool IsValid => !string.IsNullOrWhiteSpace(Name) && Password.Length >= 4;
}
```

## 미니 테스트 러너

이 챕터는 NuGet 패키지를 쓰지 않기 위해 간단한 러너를 직접 만듭니다.

```csharp
public static class MiniTestRunner
{
    public static List<TestResult> Run(IEnumerable<TestCase> cases)
    {
        var results = new List<TestResult>();
        foreach (var test in cases)
        {
            try
            {
                test.Test();
                results.Add(new TestResult(test.Name, true, ""));
            }
            catch (Exception ex)
            {
                results.Add(new TestResult(test.Name, false, ex.Message));
            }
        }
        return results;
    }
}
```

## 테스트 케이스 작성

```csharp
public static class LoginTests
{
    public static IEnumerable<TestCase> All()
    {
        yield return new TestCase("이름이 비어 있으면 IsValid=false", () =>
        {
            var vm = new LoginViewModel();
            Assert.False(vm.IsValid);
        });

        yield return new TestCase("유효한 입력이면 IsValid=true", () =>
        {
            var vm = new LoginViewModel { Name = "홍길동", Password = "pass1234" };
            Assert.True(vm.IsValid);
        });

        yield return new TestCase("LoginCommand가 성공 상태를 설정", () =>
        {
            var vm = new LoginViewModel { Name = "홍길동", Password = "pass1234" };
            vm.LoginCommand.Execute(null);
            Assert.True(vm.Status.StartsWith("환영합니다"), $"실제: {vm.Status}");
        });
    }
}
```

VB.NET:

```vb
tests.Add(New TestCase() With {
    .Name = "유효한 입력이면 IsValid=True",
    .Test = Sub()
                Dim vm As New LoginViewModel() With {.Name = "홍길동", .Password = "pass1234"}
                Assert.True(vm.IsValid)
            End Sub
})
```

## 실무: 표준 테스트 프레임워크

실무에서는 NuGet 패키지를 추가해 아래처럼 사용합니다.

```csharp
// xUnit (참고: NuGet 필요)
[Fact]
public void ValidInput_ReturnsTrue()
{
    var vm = new LoginViewModel { Name = "홍길동", Password = "pass1234" };
    Assert.True(vm.IsValid);
}
```

- 프레임워크: **xUnit**, **NUnit**, **MSTest**
- 프로젝트: 테스트용 콘솔 라이브러리 프로젝트를 별도로 생성
- 실행: `dotnet test`

## UI 자동화 개념

단위 테스트는 로직을, UI 자동화는 **실제 화면 동작**을 검증합니다.

| 도구 | 방식 |
|------|------|
| FlaUI / WinAppDriver | WPF 앱에 자동 입력·클릭 주입 |
| Appium | 크로스 플랫폼 UI 테스트 |
| Microsoft Test Manager | 수동 테스트 관리 |

핵심 패턴: `AutomationProperties.AutomationId`를 지정해
테스트 코드가 요소를 안정적으로 찾게 합니다.

```xml
<Button AutomationProperties.AutomationId="LoginButton" Content="로그인"/>
```

## 실행

```bash
cd csharp
dotnet run
```

```bash
cd vbnet
dotnet run
```

## 정리

- 테스트 가능하려면 뷰 모델이 UI에 의존하지 않아야 함
- 미니 러너로 기본 동작 검증 → 실무는 xUnit/NUnit/MSTest + `dotnet test`
- 화면 검증은 UI 자동화 도구 + `AutomationId` 사용

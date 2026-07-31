# 39: Windows 서비스 — ServiceBase 개념, 설치

## 소개

로그인 없이 백그라운드로 실행되는 **Windows 서비스**의 구조를 다룹니다. `ServiceBase` 상속과 OnStart/OnStop 라이프사이클, 설치 방법을 알아봅니다. 서비스 어셈블리는 `System.ServiceProcess` 패키지가 필요하므로 예제는 타이머 기반 시뮬레이터로 재현합니다.

## 주요 개념

### 1. ServiceBase — 서비스의 뼈대

서비스는 `ServiceBase`를 상속하고 `OnStart`/`OnStop`을 재정의합니다.

```vb
Public Class MyService
    Inherits ServiceBase

    Public Sub New()
        Me.ServiceName = "MyService"
    End Sub

    Protected Overrides Sub OnStart(args As String())
        ' 타이머 시작 등 초기화
    End Sub

    Protected Overrides Sub OnStop()
        ' 리소스 정리
    End Sub
End Class
```

### 2. 서비스 라이프사이클

- `OnStart`: 서비스 시작 시 1회 호출 → 무거운 작업은 피하고 타이머/스레드를 시작
- `OnStop`: 중지 시 호출 → 타이머 해제, 파일/연결 정리
- 필요 시 `OnPause`/`OnContinue`/`OnShutdown`도 재정의합니다.

### 3. 주기 작업 — Timer

긴 작업을 시작 루틴에 직접 넣지 않고 `Timer` 콜백으로 수행합니다.

```vb
Private _timer As New Timer(AddressOf OnTick, Nothing, Timeout.Infinite, Timeout.Infinite)
```

### 4. 서비스 등록/설치

```bash
sc create MyService binPath= "C:\...\MyService.exe"
sc start MyService
sc stop MyService
sc delete MyService
```

`sc.exe`로 등록하거나 `InstallUtil`/`ServiceInstaller` 컴포넌트로 설치할 수 있습니다.

## 실행

```bash
dotnet run
```

## 정리

- 서비스는 `ServiceBase` 상속 + `OnStart`/`OnStop` 재정의로 만듭니다.
- 주기 작업은 시작 시 타이머를 만들고 중지 시 해제합니다.
- `sc create`/`sc start`로 등록·시작합니다.
- 예제는 같은 라이프사이클을 콘솔로 재현해 동작을 확인합니다.

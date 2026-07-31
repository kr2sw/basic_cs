# 29: COM 연동 — COM 인터페이스, Marshal 개념

## 소개

VB.NET은 .NET이 아닌 기존 코드(COM 구성 요소, Win32 API)와도 연동할 수 있습니다. COM 인터페이스, RCW(런타임 콜러블 래퍼), `Marshal` 클래스와 P/Invoke 기법을 다룹니다.

## 주요 개념

### 1. P/Invoke — Win32 API 직접 호출

`Declare` 문으로 비관리 DLL 함수를 직접 호출합니다.

```vb
Declare Function GetTickCount Lib "kernel32.dll" () As UInteger
```

### 2. Marshal — 비관리 메모리 관리

비관리 힙(HGlobal)의 할당/복사/해제를 담당합니다. 비관리 리소스는 자동 해제가 안 되므로 반드시 수동 해제합니다.

```vb
Dim ptr = Marshal.StringToHGlobalAnsi(str)
Try
    Dim back = Marshal.PtrToStringAnsi(ptr)
Finally
    Marshal.FreeHGlobal(ptr)
End Try
```

### 3. RCW — 런타임 콜러블 래퍼

COM 객체(Excel, Word 등)를 .NET 객체처럼 쓰기 위해 런타임이 자동 생성하는 래퍼입니다. COM은 명시적 해제가 필요합니다.

```vb
Dim excel As Object = CreateObject("Excel.Application")
excel.Visible = True
excel.Quit()
Marshal.ReleaseComObject(excel)
```

`CreateObject`/`New`는 초기 바인딩, `CreateObject("...")`는 런타임 바인딩입니다.

### 4. GUID와 인터페이스

COM에서 각 인터페이스/클래스는 고유 GUID로 식별됩니다. `COMClassAttribute`(COMClass)로 자신의 클래스를 COM 노출할 수도 있습니다.

```vb
<ComClass(ExcelClassId, ExcelInterfaceId, ExcelEventsId)>
Public Class ExcelAutomation
End Class
```

> 참고: COM 클라이언트에게 노출하려면 `<ComVisible(True)>`, 등록(`regasm`)과 강력한 이름이 필요합니다.

## 실행

```bash
dotnet run
```

## 정리

- `Declare`(P/Invoke)로 Win32 API, `CreateObject`로 COM 객체를 사용합니다.
- 비관리 리소스는 `Marshal.ReleaseComObject`/`FreeHGlobal`로 명시 해제합니다.
- GUID는 COM 식별자이며, `ComClass` 특성으로 VB 클래스를 COM에 노출합니다.

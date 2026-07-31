# 39: 네이티브 연동 — P/Invoke

P/Invoke(Platform Invoke)는 C#에서 네이티브 DLL(Windows API, C/C++ 라이브러리)의
함수를 호출하는 기술입니다. `DllImport` 어트리뷰트로 메서드를 선언합니다.

## DllImport 기본

```csharp
[DllImport("user32.dll", CharSet = CharSet.Unicode)]
static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
```

- `extern` — 네이티브 쪽에서 구현됨을 표시
- `DllImport` — DLL 이름, 문자 인코딩, 호출 규약 지정
- 반환/매개변수 타입은 네이티브 타입과 대응해야 함 (Marshal)

## 마샬링 (Marshaling)

- `string` ↔ `char*` (CharSet로 인코딩 결정)
- `IntPtr` ↔ 포인터
- `struct` ↔ C 구조체 (`[StructLayout]`로 배치 지정)

## P/Invoke 예

- `MessageBox` (user32.dll) — Windows GUI 팝업
- `GetSystemTime` (kernel32.dll) — 네이티브 시스템 시간
- `GetLastError` / `Marshal.GetLastWin32Error`

## 실행

```bash
dotnet run
```

(Windows 전용 예제이며, 실행 환경이 Windows가 아니면 시뮬레이션 경로로 동작)

## 핵심 요약

- `DllImport` + `extern`으로 네이티브 함수를 호출합니다.
- 마샬러가 관리/비관리 타입 사이를 변환합니다.
- 반환 오류는 `GetLastWin32Error`로 확인합니다.

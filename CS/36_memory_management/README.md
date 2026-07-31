# 36: 메모리 관리 — Memory Management

C#은 가비지 컬렉터(GC)가 메모리를 자동 관리하지만, **비관리 리소스**(파일,
소켓, DB 연결)는 직접 정리해야 합니다. `IDisposable`과 `using`, GC 동작을
학습합니다.

## IDisposable과 using

비관리 리소스를 사용하는 타입은 `IDisposable`을 구현하고 `Dispose()`에서
정리합니다. `using` 문/선언은 블록이 끝날 때 자동으로 `Dispose()`를 호출합니다.

```csharp
using var file = new StreamWriter("out.txt");
file.WriteLine("내용");   // 블록 끝에서 자동 Dispose
```

## 파이널라이저 vs Dispose

- **Dispose** — 결정적(deterministic) 정리: 개발자가 명시적으로 호출
- **파이널라이저(`~Class()`)** — GC가 객체를 수집할 때 호출되는 비결정적 정리
- 표준 패턴: `Dispose(bool)` + `SuppressFinalize`

## GC 동작

- **세대(Generation)** — Gen0/1/2로 객체 수명에 따라 나눠 관리
- **Gen0(짧은 수명)** — 주로 여기서 수집됨
- 큰 객체(85KB+)는 LOH(Large Object Heap)에 따로 할당

## 실행

```bash
dotnet run
```

## 핵심 요약

- 비관리 리소스는 반드시 `IDisposable` + `using`으로 정리합니다.
- 파이널라이저는 언제 실행될지 모르므로 명시적 `Dispose`가 우선입니다.
- GC 세대 모델 덕분에 대부분의 객체는 비용이 낮게 수집됩니다.

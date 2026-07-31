# 23: 고급 비동기 — ValueTask, IAsyncEnumerable, Progress

## 소개

기초 챕터의 Task/Async/Await를 넘어 고급 비동기 타입인 `ValueTask`, `IAsyncEnumerable(Of T)`, `Progress(Of T)`를 다룹니다. 성능이 중요한 라이브러리나 데이터를 스트리밍하는 애플리케이션, UI 진행률 표시에 꼭 필요한 개념입니다.

## 주요 개념

### 1. ValueTask — 할당 없는 비동기

`Task`는 완료된 작업에도 힙에 객체를 할당합니다. 대부분의 호출이 **동기적으로 완료되는**(예: 캐시 히트) 메서드는 `ValueTask`로 선언해 할당을 줄일 수 있습니다.

```vb
Async Function GetLengthValueTaskAsync(cache As Dictionary(Of String, Integer), key As String) As ValueTask(Of Integer)
    If cache.ContainsKey(key) Then Return cache(key)   ' 동기 완료 → 할당 거의 없음
    Await Task.Delay(100)
    ...
End Function
```

주의: `ValueTask`는 여러 번 `Await`할 수 없으므로 단일 소비용입니다.

### 2. IAsyncEnumerable — 비동기 스트리밍

데이터가 순차적으로 생성되는 시나리오(대량 로그, DB 커서, 센서 스트림)에서 전체를 메모리에 올리지 않고 하나씩 비동기로 소비합니다. `Async Iterator` 함수에 `Yield`를 사용하고, 소비는 `For Each ... Await In ...` 구문입니다.

```vb
Async Iterator Function ProduceNumbersAsync(max As Integer) As IAsyncEnumerable(Of Integer)
    For i = 1 To max
        Await Task.Delay(150)
        Yield i * 10
    Next
End Function

For Each v Await In ProduceNumbersAsync(5)
    ...
Next
```

### 3. Progress(Of T) — 진행률 보고

비동기 작업이 UI 스레드(콘솔/폼)에 안전하게 진행률을 전달하는 타입입니다. 콜백은 생성 시점에 캡처된 SynchronizationContext에서 실행됩니다.

```vb
Dim progress As New Progress(Of Integer)(Sub(p) Console.WriteLine($"진행률: {p}%"))
Await DownloadWithProgressAsync(progress)
```

### 4. CancellationToken — 협조적 취소

작업 중간에 중단해야 할 때 사용합니다. `ThrowIfCancellationRequested()`를 주기적으로 호출해 협조적으로 취소합니다.

```vb
Dim cts As New CancellationTokenSource()
Dim worker = Task.Run(Function() CountWithCancelAsync(cts.Token))
cts.Cancel()
```

## 실행

```bash
dotnet run
```

## 정리

- `ValueTask`는 핫 경로(빈번 호출)에서 `Task`의 대안입니다.
- `IAsyncEnumerable` + `Await For Each`로 큰 데이터를 스트리밍합니다.
- `Progress(Of T)`로 UI에 안전하게 진행률을 보고합니다.
- 취소 토큰은 비동기 작업을 정돈하게 종료하는 표준 수단입니다.

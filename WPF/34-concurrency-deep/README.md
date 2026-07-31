# 34: 멀티스레딩 심화 — TPL Dataflow, async/await UI

## 학습 목표
- TPL Dataflow의 블록(Block) 기반 파이프라인
- `TransformBlock` / `ActionBlock` / `LinkTo` / `PropagateCompletion`
- `MaxDegreeOfParallelism` 병렬 실행
- `Progress<T>`로 UI 스레드 안전한 결과 반영
- `CancellationToken`으로 파이프라인 취소

## TPL Dataflow란

데이터가 블록을 따라 흐르는 프로그래밍 모델입니다.
각 블록은 고유한 입력 버퍼를 갖고 독립적으로 처리합니다.
`System.Threading.Tasks.Dataflow`는 .NET BCL에 포함되어 있습니다.

```
숫자 1..100 ─▶ [TransformBlock: 제곱] ─▶ [ActionBlock: 합산] ─▶ 결과
                (병렬 4개)                (순차 소비자)
```

## 파이프라인 구성

```csharp
var transform = new TransformBlock<int, int>(async n =>
{
    await Task.Delay(10, token);
    return n * n;
}, new ExecutionDataflowBlockOptions
{
    CancellationToken = token,
    MaxDegreeOfParallelism = 4      // 병렬 처리
});

var action = new ActionBlock<int>(n =>
{
    var newSum = Interlocked.Add(ref _sum, n);   // 스레드 안전 합산
    sumProgress.Report(newSum);
}, new ExecutionDataflowBlockOptions { CancellationToken = token });

transform.LinkTo(action, new DataflowLinkOptions { PropagateCompletion = true });
```

- `LinkTo`로 블록 연결, `PropagateCompletion=true`로 완료 상태 전파
- `SendAsync`로 입력 버퍼에 비동기 공급

```csharp
for (int i = 1; i <= 100; i++)
{
    await transform.SendAsync(i, token);
}
transform.Complete();
await action.Completion;    // 전체 완료 대기
```

VB.NET:

```vb
Dim transform As New TransformBlock(Of Integer, Integer)(
    Async Function(n As Integer) As Task(Of Integer)
        Await Task.Delay(10, token)
        Return n * n
    End Function, pipelineOptions)

transform.LinkTo(action, New DataflowLinkOptions() With {.PropagateCompletion = True})
```

## Progress<T>로 UI 갱신

`Progress<T>`는 생성된 스레드의 `SynchronizationContext`를 캡처해서
`Report` 호출을 UI 스레드로 마샬링합니다. 블록 몸통에서 UI 요소를
직접 건드리지 않고 항상 이 경로를 사용합니다.

```csharp
var sumProgress = new Progress<int>(s => Sum = s);
// 블록 내부 (백그라운드 스레드)
sumProgress.Report(newSum);
```

## 취소

블록 옵션에 `CancellationToken`을 넣으면 취소 요청 시 블록이
`OperationCanceledException`으로 완료됩니다.

```csharp
catch (OperationCanceledException)
{
    Status = "취소됨";
}
```

## 스레드 안전 규칙

- UI 요소 갱신 → `Progress<T>` 또는 `Dispatcher.Invoke`
- 공유 카운터 → `Interlocked`/`lock`
- `ActionBlock`은 기본적으로 **순차 실행**(`MaxDegreeOfParallelism=1`)
- `TransformBlock`은 `MaxDegreeOfParallelism`으로 병렬화

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

- 파이프라인은 블록 조합으로 유연하게 확장
- 병렬 실행과 UI 갱신 경로를 분리 (백그라운드 계산 / `Progress` 보고)
- 취소는 어디서나 토큰을 확인하므로 블록 단위로 일관 적용

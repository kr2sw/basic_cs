# 24: Concurrency & Executors — 동시성 심화

## ExecutorService (스레드 풀)

미리 만든 스레드를 재사용해 작업을 실행하는 프레임워크입니다.

```java
ExecutorService pool = Executors.newFixedThreadPool(4);
Future<Integer> f = pool.submit(() -> compute());
pool.shutdown();
```

| 팩토리 메서드 | 설명 |
|--------------|------|
| `newFixedThreadPool(n)` | 고정 크기 풀 |
| `newCachedThreadPool()` | 필요 시 생성, 유휴 스레드 재사용 |
| `newSingleThreadExecutor()` | 1개 스레드 풀 |
| `newScheduledThreadPool(n)` | 지연/주기 실행 |

`Callable<T>` 는 결과를 반환하고, `Future` 로 비동기 결과를 받습니다.

## CompletableFuture (비동기 조합)

콜백 기반으로 비동기 작업을 조합하는 API입니다.

```java
CompletableFuture.supplyAsync(() -> fetch())
    .thenApply(data -> transform(data))
    .thenAccept(System.out::println);
```

| 메서드 | 설명 |
|--------|------|
| `supplyAsync` | 비동기 실행 (결과 반환) |
| `thenApply` | 결과 변환 |
| `thenCompose` | 비동기 작업 연결 (flatMap) |
| `thenCombine` | 두 비동기 결과 조합 |
| `allOf` / `anyOf` | 여러 작업 동시 대기 |
| `exceptionally` | 실패 처리 |

## Fork/Join 프레임워크

작은 단위로 분할(divide & conquer)해 병렬 처리합니다.

```java
class SumTask extends RecursiveTask<Long> {
    protected Long compute() {
        if (범위가 작음) return 직접 합산;
        return leftTask.join() + rightTask.join();
    }
}
```

## 동시성 안전 컬렉션

- `ConcurrentHashMap`, `CopyOnWriteArrayList`
- `AtomicInteger`, `LongAdder`

## 실행

```bash
cd JAVA/24-concurrency-executors
javac Main.java && java Main
```

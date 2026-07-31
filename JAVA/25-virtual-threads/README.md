# 25: Virtual Threads — 가상 스레드

## 가상 스레드 (Java 21+)

JVM 이 관리하는 경량 스레드로, OS 스레드에 비해 매우 저렴하게 생성됩니다.

```java
Thread.ofVirtual().start(() -> System.out.println("가상 스레드 실행"));
```

- I/O 대기 중에는 자동으로 OS 스레드에서 내려와(동작 중지) 다른 작업 수행
- 플랫폼 스레드와 달리 수십만 개 생성 가능
- 기존 코드와 동일한 API (`Thread`, `Runnable`)

## 생성 방법

| 방법 | 코드 |
|------|------|
| `Thread.ofVirtual()` | `Thread.ofVirtual().start(runnable)` |
| `Executors.newVirtualThreadPerTaskExecutor()` | 작업마다 가상 스레드 생성 |
| `Thread.startVirtualThread(runnable)` | 편의 메서드 |

## 플랫폼 스레드 vs 가상 스레드

| 구분 | 플랫폼 스레드 | 가상 스레드 |
|------|--------------|-------------|
| 매핑 | OS 스레드 1:1 | JVM 에서 다대일 |
| 생성 비용 | 높음 | 매우 낮음 |
| 개수 제한 | 수천 개 수준 | 수십만 개 가능 |
| 사용처 | CPU 집약 작업, 스레드 풀 고정 | I/O 집약 작업에 적합 |

## StructuredTaskScope (구조적 동시성)

관련된 작업들을 하나의 범위로 묶어 함께 관리합니다.

```java
try (var scope = new StructuredTaskScope.ShutdownOnFailure()) {
    Future<String> user = scope.fork(() -> loadUser());
    Future<String> order = scope.fork(() -> loadOrder());
    scope.join();          // 모두 완료 대기
    scope.throwIfFailed();
}
```

- `fork` 로 하위 작업 제출, `join` 으로 완료 대기
- `ShutdownOnFailure`: 하나라도 실패하면 즉시 중단
- 작업이 범위를 벗어나면 자동으로 취소/정리됨

## 실행

```bash
cd JAVA/25-virtual-threads
javac Main.java && java Main
```

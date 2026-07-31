# 37: Performance — 성능 최적화

## 성능 측정의 중요성

"빠르게"는 측정 없이는 판단할 수 없습니다. 먼저 **측정**부터 시작합니다.

## JMH (Java Microbenchmark Harness)

JVM 벤치마크 표준 도구로, JIT 워밍업/측정 오차를 제어합니다.

```java
@Benchmark
@BenchmarkMode(Mode.AverageTime)
@Warmup(iterations = 5)
@Measurement(iterations = 5)
public void measureMethod() {
    list.contains(value);
}
```

- JIT 컴파일, GC, JVM 워밍업을 고려
- `@Benchmark`, `@BenchmarkMode`, `@Warmup`, `@Measurement` 등 사용
- Maven/Gradle 플러그인으로 실행 (외부 라이브러리)

## 프로파일링

병목 지점을 찾는 도구입니다.

| 도구 | 용도 |
|------|------|
| JFR (Java Flight Recorder) | 런타임 이벤트 기록 (CPU, 메모리, I/O) |
| JMC (Mission Control) | JFR 분석 GUI |
| `jstack` | 스레드 덤프 (교착 상태/블로킹 확인) |
| VisualVM / Async Profiler | CPU/메모리 프로파일링 |

```bash
java -XX:StartFlightRecording=duration=60s,filename=app.jfr Main
```

## 컬렉션 선택 최적화

| 상황 | 최적 컬렉션 |
|------|-------------|
| 중간 삽입/삭제 | `LinkedList` |
| 임의 접근이 잦음 | `ArrayList` |
| 중복 없음 + 빠른 포함 검사 | `HashSet` |
| 순서 유지 | `LinkedHashSet` |
| 키 기반 검색 | `HashMap` |

## 문자열 최적화

반복 결합 시 `StringBuilder` 가 `String +` 보다 훨씬 빠릅니다.

```java
StringBuilder sb = new StringBuilder();
for (int i = 0; i < n; i++) sb.append(i);
```

## 실행

```bash
cd JAVA/37-performance
javac Main.java && java Main
```

> JMH 없이 `System.nanoTime()` 으로 간단한 마이크로 벤치마크를 수행해 봅니다.

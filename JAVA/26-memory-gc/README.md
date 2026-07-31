# 26: Memory & GC — 메모리 모델과 가비지 컬렉션

## JMM (Java Memory Model)

스레드 간 메모리 가시성을 정의하는 모델입니다.

- **메인 메모리**: 모든 스레드가 공유하는 힙 영역
- **작업 메모리**: 각 스레드의 캐시 (레지스터/로컬 캐시)
- **가시성 문제**: 한 스레드의 변경이 다른 스레드에 즉시 보이지 않을 수 있음

```java
volatile boolean running = true;   // 메인 메모리 직접 읽기/쓰기
```

## 메모리 구조 (런타임)

| 영역 | 설명 |
|------|------|
| **Heap (힙)** | 객체와 배열 저장, GC 대상 |
| **Stack (스택)** | 지역 변수, 메서드 호출 프레임 (스레드별) |
| **Method Area / Metaspace** | 클래스 메타데이터, static 변수 |
| **PC Register / Native Stack** | 실행 위치, 네이티브 호출 |

힙은 다시 Young 영역(Eden, Survivor)과 Old 영역으로 나뉩니다.

## 가시성과 happens-before

`synchronized`, `volatile`, `final` 은 happens-before 규칙을 보장합니다.

```java
synchronized (lock) { shared = newValue; }  // 해제 -> 획득 순서 보장
```

## GC (Garbage Collector)

도달 불가능한(참조가 끊긴) 객체를 정리합니다.

| GC | 특징 |
|----|------|
| Serial | 단일 스레드, 소규모 |
| Parallel | 멀티 스레드 처리, 처리량 중시 |
| G1 (기본) | 힙을 Region 으로 나눠 예측 가능한 일시정지 |
| ZGC | 매우 짧은 일시정지, 대용량 힙 |

```bash
java -XX:+UseG1GC -Xms2g -Xmx2g Main
```

## 참조의 종류

| 참조 | 특징 |
|------|------|
| Strong | 기본 참조, GC 대상 안 됨 |
| Soft | 메모리 부족 시 회수 |
| Weak | 다음 GC 때 즉시 회수 |
| Phantom | 객체가 사라진 뒤 후처리 (Queue 사용) |

## 실행

```bash
cd JAVA/26-memory-gc
javac Main.java && java Main
```

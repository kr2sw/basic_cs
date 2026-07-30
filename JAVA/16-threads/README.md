# 16: Threads — 스레드

## 스레드 생성 방법

### Thread 클래스 상속
```java
class MyThread extends Thread {
    public void run() { ... }
}
```

### Runnable 인터페이스 구현
```java
class MyTask implements Runnable {
    public void run() { ... }
}
```

### 람다 표현식 (Java 8+)
```java
new Thread(() -> { ... }).start();
```

## 스레드 상태

`NEW → RUNNABLE → BLOCKED/WAITING/TIMED_WAITING → TERMINATED`

## 동기화 (Synchronization)

- `synchronized` 키워드로 임계 영역 보호
- `synchronized` 메서드 또는 블록
- `wait()`, `notify()`, `notifyAll()`: 스레드 간 통신

## ExecutorService (Java 5+)

스레드 풀을 관리하는 프레임워크입니다.

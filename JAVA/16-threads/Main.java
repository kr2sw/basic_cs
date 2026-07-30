import java.util.concurrent.*;

// 공유 자원 (동기화 필요)
class Counter {
    private int count = 0;

    public synchronized void increment() {
        count++;
    }

    public synchronized int getCount() {
        return count;
    }
}

public class Main {
    public static void main(String[] args) throws Exception {
        // 1. Thread 상속
        Thread thread1 = new Thread() {
            @Override
            public void run() {
                System.out.println("Thread 상속: " + Thread.currentThread().getName());
            }
        };
        thread1.start();

        // 2. Runnable 구현
        Thread thread2 = new Thread(new Runnable() {
            @Override
            public void run() {
                System.out.println("Runnable 구현: " + Thread.currentThread().getName());
            }
        });
        thread2.start();

        // 3. Lambda
        Thread thread3 = new Thread(() -> {
            System.out.println("Lambda 스레드: " + Thread.currentThread().getName());
        });
        thread3.start();

        // 4. 동기화 예제
        Counter counter = new Counter();
        Thread[] threads = new Thread[1000];

        for (int i = 0; i < 1000; i++) {
            threads[i] = new Thread(() -> counter.increment());
            threads[i].start();
        }

        for (Thread t : threads) {
            t.join(); // 모든 스레드 종료 대기
        }
        System.out.println("동기화 결과: " + counter.getCount()); // 1000

        // 5. ExecutorService (스레드 풀)
        ExecutorService executor = Executors.newFixedThreadPool(4);

        for (int i = 0; i < 8; i++) {
            final int taskId = i;
            executor.submit(() -> {
                System.out.println("Task " + taskId + " 실행 ("
                    + Thread.currentThread().getName() + ")");
                try {
                    Thread.sleep(500);
                } catch (InterruptedException e) {
                    Thread.currentThread().interrupt();
                }
                System.out.println("Task " + taskId + " 완료");
            });
        }

        executor.shutdown();
        executor.awaitTermination(5, TimeUnit.SECONDS);
        System.out.println("모든 작업 완료");

        // 6. Callable + Future (반환값 있음)
        ExecutorService executor2 = Executors.newSingleThreadExecutor();
        Future<Integer> future = executor2.submit(() -> {
            Thread.sleep(1000);
            return 42;
        });

        System.out.println("Future 결과: " + future.get()); // 42
        executor2.shutdown();
    }
}

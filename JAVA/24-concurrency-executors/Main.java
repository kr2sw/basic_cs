import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.atomic.*;

public class Main {

    public static void main(String[] args) throws Exception {
        System.out.println("=== ExecutorService ===");

        // 고정 크기 스레드 풀: Callable 결과를 Future 로 받기
        ExecutorService pool = Executors.newFixedThreadPool(4);
        List<Future<Integer>> futures = new ArrayList<>();
        for (int i = 1; i <= 8; i++) {
            final int task = i;
            futures.add(pool.submit(() -> {
                Thread.sleep(100);   // 작업 시뮬레이션
                return task * task;
            }));
        }
        int total = 0;
        for (Future<Integer> f : futures) {
            total += f.get();   // 블로킹하며 결과 대기
        }
        System.out.println("8개 작업 결과 합(제곱합): " + total);
        pool.shutdown();

        // invokeAll: 모든 작업 한 번에 제출하고 결과 수집
        ExecutorService batchPool = Executors.newCachedThreadPool();
        List<Callable<Integer>> tasks = new ArrayList<>();
        for (int i = 1; i <= 5; i++) {
            final int n = i;
            tasks.add(() -> n * 10);
        }
        List<Future<Integer>> results = batchPool.invokeAll(tasks);
        System.out.print("invokeAll 결과: ");
        for (Future<Integer> f : results) {
            System.out.print(f.get() + " ");
        }
        System.out.println();
        batchPool.shutdown();

        System.out.println("\n=== CompletableFuture ===");

        // supplyAsync + thenApply + thenAccept 체이닝
        CompletableFuture.supplyAsync(() -> {
            System.out.println("  [1] 데이터 가져오는 중... (스레드: " + Thread.currentThread().getName() + ")");
            return 100;
        }).thenApply(v -> {
            System.out.println("  [2] 데이터 변환: * 2");
            return v * 2;
        }).thenApply(v -> {
            System.out.println("  [3] 데이터 변환: + 10");
            return v + 10;
        }).thenAccept(v -> System.out.println("  [4] 최종 결과: " + v));

        // thenCombine: 두 비동기 작업 결과 조합
        CompletableFuture<Integer> a = CompletableFuture.supplyAsync(() -> 20);
        CompletableFuture<Integer> b = CompletableFuture.supplyAsync(() -> 22);
        int combined = a.thenCombine(b, Integer::sum).join();
        System.out.println("thenCombine(20, 22) 합: " + combined);

        // exceptionally: 실패 처리
        int withFallback = CompletableFuture
            .supplyAsync(() -> { throw new IllegalStateException("실패"); })
            .exceptionally(ex -> -1)
            .join();
        System.out.println("exceptionally 복구 값: " + withFallback);

        // allOf: 여러 작업 완료 대기
        CompletableFuture<Void> all = CompletableFuture.allOf(
            CompletableFuture.runAsync(() -> sleep(50)),
            CompletableFuture.runAsync(() -> sleep(30))
        );
        all.join();
        System.out.println("allOf: 모든 작업 완료 대기 후 종료");

        System.out.println("\n=== Fork/Join ===");

        // ForkJoinPool 로 1..100 합계를 분할 정복으로 계산
        int range = 100;
        long fkSum = ForkJoinPool.commonPool()
            .invoke(new SumTask(1, range, 10));
        System.out.println("ForkJoin 합(1..100): " + fkSum);

        System.out.println("\n=== 동시성 안전 컬렉션 ===");

        // 여러 스레드가 동시에 증가시키는 카운터
        int threads = 8;
        ExecutorService racePool = Executors.newFixedThreadPool(threads);
        AtomicInteger atomicCount = new AtomicInteger();
        CountDownLatch latch = new CountDownLatch(threads);
        for (int i = 0; i < threads; i++) {
            racePool.submit(() -> {
                for (int j = 0; j < 10_000; j++) atomicCount.incrementAndGet();
                latch.countDown();
            });
        }
        latch.await();
        racePool.shutdown();
        System.out.println("AtomicInteger 카운터 (기대 80000): " + atomicCount.get());

        // ConcurrentHashMap: 안전한 동시 접근
        ConcurrentHashMap<String, Integer> map = new ConcurrentHashMap<>();
        map.put("Java", 1);
        map.merge("Java", 1, Integer::sum);
        System.out.println("ConcurrentHashMap: " + map);
    }

    static void sleep(long ms) {
        try { Thread.sleep(ms); } catch (InterruptedException e) { Thread.currentThread().interrupt(); }
    }

    // Fork/Join: 범위를 반으로 나눠 병렬 합산하는 태스크
    static class SumTask extends RecursiveTask<Long> {
        private final int from, to, threshold;

        SumTask(int from, int to, int threshold) {
            this.from = from;
            this.to = to;
            this.threshold = threshold;
        }

        @Override
        protected Long compute() {
            if (to - from <= threshold) {
                long sum = 0;
                for (int i = from; i <= to; i++) sum += i;
                return sum;
            }
            int mid = (from + to) / 2;
            SumTask left = new SumTask(from, mid, threshold);
            SumTask right = new SumTask(mid + 1, to, threshold);
            left.fork();                    // 왼쪽은 별도 스레드로
            long rightResult = right.compute();  // 오른쪽은 현재 스레드에서
            return rightResult + left.join();    // 왼쪽 결과 대기 후 합침
        }
    }
}

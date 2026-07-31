import java.util.*;
import java.util.concurrent.*;
import java.util.stream.*;

public class Main {

    public static void main(String[] args) throws Exception {
        System.out.println("=== 가상 스레드 생성 ===");

        // Thread.ofVirtual() 로 가상 스레드 생성
        Thread vt = Thread.ofVirtual()
            .name("vt-1")
            .start(() -> {
                System.out.println("  가상 스레드에서 실행: " + Thread.currentThread());
                System.out.println("  isVirtual: " + Thread.currentThread().isVirtual());
            });
        vt.join();

        // Thread.startVirtualThread 편의 메서드
        Thread.startVirtualThread(() -> System.out.println("  startVirtualThread: 실행 완료"))
            .join();

        System.out.println("\n=== 대량 가상 스레드 (10만 개) ===");

        // 100,000개 가상 스레드를 생성해 동시에 실행
        long start = System.nanoTime();
        try (ExecutorService executor = Executors.newVirtualThreadPerTaskExecutor()) {
            CountDownLatch latch = new CountDownLatch(100_000);
            IntStream.range(0, 100_000).forEach(i ->
                executor.submit(() -> {
                    latch.countDown();
                    return i;
                }));
            latch.await();   // 모두 완료될 때까지 대기
        }
        long elapsed = (System.nanoTime() - start) / 1_000_000;
        System.out.println("100,000개 가상 스레드 실행 완료: " + elapsed + "ms");

        System.out.println("\n=== I/O 시뮬레이션 비교 ===");

        // 블로킹 I/O(100ms sleep) 200건을 가상 스레드로 처리
        int tasks = 200;
        long vtStart = System.nanoTime();
        try (ExecutorService executor = Executors.newVirtualThreadPerTaskExecutor()) {
            IntStream.range(0, tasks).forEach(i ->
                executor.submit(() -> { sleep(100); return 0; }));
        }
        long vtTime = (System.nanoTime() - vtStart) / 1_000_000;
        System.out.println("가상 스레드 200건 I/O: " + vtTime + "ms");

        long ptStart = System.nanoTime();
        try (ExecutorService executor = Executors.newFixedThreadPool(32)) {
            IntStream.range(0, tasks).forEach(i ->
                executor.submit(() -> { sleep(100); return 0; }));
        }
        long ptTime = (System.nanoTime() - ptStart) / 1_000_000;
        System.out.println("플랫폼 스레드(풀 32) 200건 I/O: " + ptTime + "ms");

        System.out.println("\n=== StructuredTaskScope ===");

        // 서로 독립적인 조회를 병렬로 수행 후 결과 조합
        try (var scope = new StructuredTaskScope.ShutdownOnFailure()) {
            Future<String> user = scope.fork(() -> fetch("사용자 조회"));
            Future<String> order = scope.fork(() -> fetch("주문 조회"));
            scope.join();            // 모든 작업 완료 대기
            scope.throwIfFailed();   // 실패가 있으면 예외 발생
            System.out.println("  병렬 결과: " + user.resultNow() + " / " + order.resultNow());
        } catch (Exception e) {
            System.out.println("  StructuredTaskScope 실패: " + e.getMessage());
        }
    }

    static String fetch(String label) {
        sleep(50);
        return label + " 완료";
    }

    static void sleep(long ms) {
        try { Thread.sleep(ms); } catch (InterruptedException e) { Thread.currentThread().interrupt(); }
    }
}

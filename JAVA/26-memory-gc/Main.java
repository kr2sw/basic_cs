import java.lang.ref.*;
import java.util.*;

public class Main {

    public static void main(String[] args) throws Exception {
        System.out.println("=== JMM 가시성 (volatile) ===");

        // volatile 로 종료 신호 공유
        VisibilityDemo demo = new VisibilityDemo();
        Thread worker = new Thread(demo::run);
        worker.start();
        Thread.sleep(100);
        demo.stop();               // 다른 스레드가 플래그 변경
        worker.join(1000);
        System.out.println("  volatile 종료 신호로 작업 스레드 종료");

        System.out.println("\n=== 힙/스택 구조 확인 ===");

        // 힙 사용량 확인
        Runtime rt = Runtime.getRuntime();
        System.out.println("  JVM 총 힙: " + rt.totalMemory() / 1024 / 1024 + "MB");
        System.out.println("  사용 중 힙: " + (rt.totalMemory() - rt.freeMemory()) / 1024 / 1024 + "MB");

        // 스택: 재귀 호출 깊이 측정 (StackOverflowError 확인)
        try {
            recurse(0);
        } catch (StackOverflowError e) {
            System.out.println("  스택 오버플로우 발생 (재귀 한계 도달)");
        }

        System.out.println("\n=== GC 요청과 Strong/Weak 참조 ===");

        // Strong 참조: 참조가 남아 있어 GC 대상이 아님
        Object strong = new Object();
        System.gc();
        System.out.println("  Strong 참조 객체 유지됨: " + strong);

        // WeakReference: GC 발생 시 회수됨
        WeakReference<Object> weak = new WeakReference<>(new Object());
        System.out.println("  GC 전 WeakReference: " + weak.get());
        forceGC();
        System.out.println("  GC 후 WeakReference: " + weak.get());

        // SoftReference: 메모리 부족 시에만 회수 (일반적으로 유지됨)
        SoftReference<Object> soft = new SoftReference<>(new Object());
        System.out.println("  SoftReference: " + soft.get());

        System.out.println("\n=== WeakHashMap ===");

        // WeakHashMap: 키가 GC 되면 엔트리 자동 제거
        Map<Key, String> weakMap = new WeakHashMap<>();
        Key key = new Key("임시키");
        weakMap.put(key, "캐시된 값");
        System.out.println("  GC 전 WeakHashMap 크기: " + weakMap.size());
        key = null;   // 키 참조 제거
        forceGC();
        System.out.println("  GC 후 WeakHashMap 크기: " + weakMap.size());

        System.out.println("\n=== PhantomReference + ReferenceQueue ===");

        // PhantomReference: 객체 소멸 후 큐를 통해 후처리
        ReferenceQueue<Object> queue = new ReferenceQueue<>();
        Object target = new Object();
        PhantomReference<Object> phantom = new PhantomReference<>(target, queue);
        target = null;
        forceGC();
        System.out.println("  PhantomReference.get(): " + phantom.get());  // 항상 null
        System.out.println("  ReferenceQueue에서 수거: " + (queue.poll() != null));

        System.out.println("\n=== 인위적 OutOfMemory 방지 및 힙 상태 ===");

        // 힙을 일부 점유하고 GC로 정리하는 과정 관찰
        List<byte[]> chunk = new ArrayList<>();
        try {
            for (int i = 0; i < 20; i++) chunk.add(new byte[8 * 1024 * 1024]);
        } catch (OutOfMemoryError e) {
            System.out.println("  OutOfMemoryError 발생!");
        }
        chunk.clear();
        forceGC();
        System.out.println("  정리 후 사용 중 힙: " + (rt.totalMemory() - rt.freeMemory()) / 1024 / 1024 + "MB");
    }

    // GC 가 실제로 동작하도록 여유를 두고 대기
    static void forceGC() throws InterruptedException {
        for (int i = 0; i < 10; i++) {
            System.gc();
            Thread.sleep(20);
        }
    }

    static int depth = 0;
    static void recurse(int n) {
        depth = n;
        recurse(n + 1);
    }

    // volatile 플래그를 공유하는 클래스
    static class VisibilityDemo {
        private volatile boolean running = true;

        void run() {
            int count = 0;
            while (running) count++;
            System.out.println("  작업 스레드 중단, 카운트: " + count);
        }

        void stop() {
            running = false;   // volatile: 메인 메모리에 즉시 반영
        }
    }

    // WeakHashMap 의 키로 사용할 클래스
    static class Key {
        private final String id;
        Key(String id) { this.id = id; }
        @Override public int hashCode() { return id.hashCode(); }
        @Override public boolean equals(Object o) {
            return o instanceof Key k && id.equals(k.id);
        }
    }
}

import java.util.*;

public class Main {

    public static void main(String[] args) {
        System.out.println("=== 벤치마크 헬퍼 ===");
        System.out.println("  System.nanoTime() 로 실행 시간 측정 (JMH 의 간단한 대체)");

        System.out.println("\n=== 1. 문자열 결합: + vs StringBuilder ===");

        int n = 50_000;
        long start = System.nanoTime();
        String plus = "";
        for (int i = 0; i < n; i++) plus += i;   // 매번 새 String 생성
        long plusTime = (System.nanoTime() - start) / 1_000_000;

        start = System.nanoTime();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < n; i++) sb.append(i);
        String built = sb.toString();
        long sbTime = (System.nanoTime() - start) / 1_000_000;

        System.out.println("  String '+'       : " + plusTime + "ms");
        System.out.println("  StringBuilder   : " + sbTime + "ms");
        System.out.println("  결과 동일: " + plus.equals(built));

        System.out.println("\n=== 2. 컬렉션 포함 검사: List vs Set ===");

        int size = 50_000;
        List<Integer> list = new ArrayList<>();
        Set<Integer> set = new HashSet<>();
        for (int i = 0; i < size; i++) { list.add(i); set.add(i); }

        // 검색 실패 경우를 반복 측정 (List 는 전체 순회)
        start = System.nanoTime();
        int found = 0;
        for (int i = 0; i < 1000; i++) if (list.contains(-1)) found++;
        long listTime = (System.nanoTime() - start) / 1_000_000;

        start = System.nanoTime();
        for (int i = 0; i < 1000; i++) if (set.contains(-1)) found++;
        long setTime = (System.nanoTime() - start) / 1_000_000;

        System.out.println("  List.contains (1000회): " + listTime + "ms");
        System.out.println("  Set.contains  (1000회): " + setTime + "ms");
        System.out.println("  => 검색이 잦으면 Set 이 유리");

        System.out.println("\n=== 3. ArrayList vs LinkedList ===");

        // 앞쪽 삽입: LinkedList 는 노드 연결만, ArrayList 는 밀어내기
        List<Integer> arrayList = new ArrayList<>();
        List<Integer> linkedList = new LinkedList<>();

        start = System.nanoTime();
        for (int i = 0; i < 50_000; i++) arrayList.add(0, i);
        long alTime = (System.nanoTime() - start) / 1_000_000;

        start = System.nanoTime();
        for (int i = 0; i < 50_000; i++) linkedList.add(0, i);
        long llTime = (System.nanoTime() - start) / 1_000_000;

        System.out.println("  ArrayList 앞 삽입 50000회 : " + alTime + "ms");
        System.out.println("  LinkedList 앞 삽입 50000회 : " + llTime + "ms");

        // 임의 접근(get): ArrayList 가 압도적으로 빠름
        start = System.nanoTime();
        long acc = 0;
        for (int i = 0; i < 100_000; i++) acc += arrayList.get(i % arrayList.size());
        long alGet = (System.nanoTime() - start) / 1_000_000;

        start = System.nanoTime();
        for (int i = 0; i < 100_000; i++) acc += linkedList.get(i % linkedList.size());
        long llGet = (System.nanoTime() - start) / 1_000_000;

        System.out.println("  ArrayList get 100000회   : " + alGet + "ms");
        System.out.println("  LinkedList get 100000회  : " + llGet + "ms");
        System.out.println("  => 임의 접근이 잦으면 ArrayList");

        System.out.println("\n=== 4. HashMap 초기 용량 ===");

        // 용량을 미리 지정하면 재해시(resize) 횟수를 줄임
        Map<Integer, Integer> noCapacity = new HashMap<>();
        start = System.nanoTime();
        for (int i = 0; i < 200_000; i++) noCapacity.put(i, i);
        long noCapTime = (System.nanoTime() - start) / 1_000_000;

        Map<Integer, Integer> withCapacity = new HashMap<>(200_000 * 4 / 3 + 1);
        start = System.nanoTime();
        for (int i = 0; i < 200_000; i++) withCapacity.put(i, i);
        long capTime = (System.nanoTime() - start) / 1_000_000;

        System.out.println("  HashMap 기본 용량       : " + noCapTime + "ms");
        System.out.println("  HashMap 초기 용량 지정  : " + capTime + "ms");

        System.out.println("\n=== 5. 스트림 vs 반복문 (단순 연산) ===");

        List<Integer> data = new ArrayList<>();
        for (int i = 0; i < 200_000; i++) data.add(i);

        start = System.nanoTime();
        int sumFor = 0;
        for (int v : data) if (v % 2 == 0) sumFor += v;
        long forTime = (System.nanoTime() - start) / 1_000_000;

        start = System.nanoTime();
        int sumStream = data.stream().filter(v -> v % 2 == 0).mapToInt(Integer::intValue).sum();
        long streamTime = (System.nanoTime() - start) / 1_000_000;

        System.out.println("  for 문        : " + forTime + "ms (합=" + sumFor + ")");
        System.out.println("  Stream(filter): " + streamTime + "ms (합=" + sumStream + ")");
        System.out.println("  => 단순 연산은 반복문이 조금 유리하지만,");
        System.out.println("     가독성과 병렬 처리는 Stream 이 유리");

        System.out.println("\n=== JMH 개념 (주석) ===");

        /*
        // 실제 JMH 벤치마크 (외부 라이브러리, 강의자료용 참고)
        @Benchmark
        @BenchmarkMode(Mode.AverageTime)
        @OutputTimeUnit(TimeUnit.NANOSECONDS)
        @Fork(1)
        @Warmup(iterations = 3)
        @Measurement(iterations = 5)
        public void measureContains(BenchmarkState state) {
            state.set.contains(-1);
        }
        // 실행: mvn clean install && java -jar target/benchmarks.jar
        */
    }
}

import java.util.*;
import java.util.stream.*;

public class Main {

    // 직원 정보를 담는 record (Java 16+)
    record Employee(String name, String dept, int age, int salary) {}

    public static void main(String[] args) {
        List<Employee> employees = List.of(
            new Employee("김철수", "개발", 30, 5000),
            new Employee("이영희", "개발", 28, 5200),
            new Employee("박민준", "영업", 35, 4800),
            new Employee("최수진", "영업", 32, 5100),
            new Employee("정하늘", "개발", 41, 6500),
            new Employee("한지민", "인사", 29, 4500)
        );

        System.out.println("=== Comparator 심화 ===");

        // thenComparing 으로 나이 기준 1차, 급여 기준 2차 정렬
        List<Employee> sorted = new ArrayList<>(employees);
        sorted.sort(Comparator.comparingInt(Employee::age)
                .thenComparingInt(Employee::salary));
        System.out.println("나이 -> 급여 오름차순:");
        sorted.forEach(e -> System.out.println("  " + e));

        // reversed() 로 역순, nullsFirst 로 null 처리
        List<String> names = new ArrayList<>(List.of("banana", null, "apple", "cherry"));
        names.sort(Comparator.nullsFirst(String::compareTo));
        System.out.println("nullsFirst 정렬: " + names);

        System.out.println("\n=== groupingBy ===");

        // 부서별 그룹핑
        Map<String, List<Employee>> byDept = employees.stream()
            .collect(Collectors.groupingBy(Employee::dept));
        System.out.println("부서별 직원 수: " +
            byDept.entrySet().stream()
                .collect(Collectors.toMap(Map.Entry::getKey,
                        e -> e.getValue().size())));

        // 부서별 인원수 counting
        Map<String, Long> countByDept = employees.stream()
            .collect(Collectors.groupingBy(Employee::dept, Collectors.counting()));
        System.out.println("부서별 인원수: " + countByDept);

        // 부서별 평균 급여 averagingDouble
        Map<String, Double> avgByDept = employees.stream()
            .collect(Collectors.groupingBy(Employee::dept,
                    Collectors.averagingDouble(Employee::salary)));
        System.out.println("부서별 평균 급여: " + avgByDept);

        // mapping 하위 수집기로 이름만 뽑아 리스트로
        Map<String, List<String>> nameByDept = employees.stream()
            .collect(Collectors.groupingBy(Employee::dept,
                    Collectors.mapping(Employee::name, Collectors.toList())));
        System.out.println("부서별 이름 목록: " + nameByDept);

        // partitioningBy : 30세 초과 여부로 두 그룹 분리
        Map<Boolean, List<Employee>> byAge = employees.stream()
            .collect(Collectors.partitioningBy(e -> e.age() > 30));
        System.out.println("30세 초과 그룹: " + byAge.get(true).size() +
            "명, 이하 그룹: " + byAge.get(false).size() + "명");

        System.out.println("\n=== parallelStream ===");

        // 병렬 스트림으로 1부터 10만까지의 합 계산
        long sum = LongStream.rangeClosed(1, 100_000)
            .parallel()
            .sum();
        System.out.println("1..100000 병렬 합: " + sum);

        // 직렬 vs 병렬 성능 비교 (단순 측정)
        long n = 5_000_000L;
        long start = System.nanoTime();
        long seqSum = LongStream.rangeClosed(1, n).sum();
        long seqTime = System.nanoTime() - start;

        start = System.nanoTime();
        long parSum = LongStream.rangeClosed(1, n).parallel().sum();
        long parTime = System.nanoTime() - start;

        System.out.println("직렬 합: " + seqSum + " (" + seqTime / 1_000_000 + "ms)");
        System.out.println("병렬 합: " + parSum + " (" + parTime / 1_000_000 + "ms)");

        System.out.println("\n=== 고급 Collectors ===");

        // joining 으로 이름을 콤마로 연결
        String joined = employees.stream()
            .map(Employee::name)
            .collect(Collectors.joining(", "));
        System.out.println("전체 이름: " + joined);

        // toMap 으로 이름 -> 급여 맵 생성
        Map<String, Integer> salaryMap = employees.stream()
            .collect(Collectors.toMap(Employee::name, Employee::salary));
        System.out.println("이름->급여: " + salaryMap);

        // collectingAndThen : 수집 후 후처리 (읽기 전용 리스트로 변환)
        List<String> unmodifiable = employees.stream()
            .map(Employee::name)
            .collect(Collectors.collectingAndThen(Collectors.toList(),
                    Collections::unmodifiableList));
        System.out.println("읽기 전용 리스트: " + unmodifiable);
    }
}

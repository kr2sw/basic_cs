import java.util.*;
import java.util.stream.*;

public class Main {
    public static void main(String[] args) {
        List<String> fruits = Arrays.asList("apple", "banana", "cherry", "date", "elderberry");

        // Lambda: 리스트 출력
        System.out.print("forEach (lambda): ");
        fruits.forEach(f -> System.out.print(f + " "));
        System.out.println();

        // Method reference
        System.out.print("forEach (method ref): ");
        fruits.forEach(System.out::println);

        // Stream API
        System.out.println("\n=== Stream API ===");

        // filter + map + collect
        List<String> filtered = fruits.stream()
            .filter(f -> f.startsWith("a") || f.startsWith("b"))
            .map(String::toUpperCase)
            .collect(Collectors.toList());
        System.out.println("filtered: " + filtered);

        // sorted
        List<String> sorted = fruits.stream()
            .sorted(Comparator.comparingInt(String::length))
            .collect(Collectors.toList());
        System.out.println("sorted by length: " + sorted);

        // distinct + limit
        List<Integer> numbers = Arrays.asList(1, 2, 2, 3, 3, 3, 4, 5, 5);
        List<Integer> distinct = numbers.stream()
            .distinct()
            .limit(4)
            .collect(Collectors.toList());
        System.out.println("distinct+limit: " + distinct);

        // reduce
        int sum = numbers.stream()
            .distinct()
            .reduce(0, Integer::sum);
        System.out.println("sum(distinct): " + sum);

        // anyMatch / allMatch
        boolean anyLong = fruits.stream().anyMatch(f -> f.length() > 6);
        boolean allShort = fruits.stream().allMatch(f -> f.length() < 10);
        System.out.println("any length > 6: " + anyLong);
        System.out.println("all length < 10: " + allShort);

        // groupingBy
        Map<Integer, List<String>> grouped = fruits.stream()
            .collect(Collectors.groupingBy(String::length));
        System.out.println("grouped by length: " + grouped);

        // flatMap
        List<List<Integer>> nested = Arrays.asList(
            Arrays.asList(1, 2), Arrays.asList(3, 4, 5), Arrays.asList(6)
        );
        List<Integer> flattened = nested.stream()
            .flatMap(Collection::stream)
            .collect(Collectors.toList());
        System.out.println("flatMap: " + flattened);

        // Optional
        Optional<String> first = fruits.stream()
            .filter(f -> f.startsWith("z"))
            .findFirst();
        System.out.println("first starting with z: " + first.orElse("없음"));

        Optional<String> found = fruits.stream()
            .filter(f -> f.startsWith("c"))
            .findFirst();
        found.ifPresent(f -> System.out.println("found: " + f));

        // IntStream
        IntStream.range(1, 6)
            .map(i -> i * i)
            .forEach(i -> System.out.print(i + " "));
        System.out.println();
    }
}

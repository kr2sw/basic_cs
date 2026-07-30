import java.util.*;

public class Main {
    public static void main(String[] args) {
        // List - ArrayList
        List<String> list = new ArrayList<>();
        list.add("Apple");
        list.add("Banana");
        list.add("Cherry");
        list.add(1, "Blueberry"); // 중간 삽입
        System.out.println("List: " + list);
        System.out.println("get(0): " + list.get(0));
        System.out.println("indexOf Banana: " + list.indexOf("Banana"));

        // List - LinkedList
        LinkedList<Integer> linkedList = new LinkedList<>();
        linkedList.addFirst(1);
        linkedList.addLast(2);
        linkedList.addFirst(0);
        System.out.println("LinkedList: " + linkedList);

        // Set - HashSet
        Set<String> set = new HashSet<>();
        set.add("Java");
        set.add("Python");
        set.add("Java"); // 중복 무시
        System.out.println("HashSet: " + set);
        System.out.println("contains Java: " + set.contains("Java"));

        // Set - TreeSet (정렬)
        Set<Integer> treeSet = new TreeSet<>();
        treeSet.add(5);
        treeSet.add(1);
        treeSet.add(3);
        treeSet.add(5); // 중복 무시
        System.out.println("TreeSet: " + treeSet);

        // Map - HashMap
        Map<String, Integer> map = new HashMap<>();
        map.put("Alice", 90);
        map.put("Bob", 85);
        map.put("Charlie", 92);
        System.out.println("Map: " + map);
        System.out.println("Alice 점수: " + map.get("Alice"));
        System.out.println("containsKey Bob: " + map.containsKey("Bob"));

        // Map 순회
        for (Map.Entry<String, Integer> entry : map.entrySet()) {
            System.out.println(entry.getKey() + " → " + entry.getValue());
        }

        // Iterator
        Iterator<String> it = list.iterator();
        System.out.print("Iterator: ");
        while (it.hasNext()) {
            System.out.print(it.next() + " ");
        }
        System.out.println();

        // Comparable (Person 클래스)
        List<Person> people = new ArrayList<>();
        people.add(new Person("Alice", 25));
        people.add(new Person("Bob", 20));
        people.add(new Person("Charlie", 30));
        Collections.sort(people);
        System.out.println("Comparable 정렬: " + people);

        // Comparator
        people.sort((p1, p2) -> p1.getName().compareTo(p2.getName()));
        System.out.println("Comparator 정렬: " + people);

        // Collections 유틸리티
        List<Integer> nums = new ArrayList<>(Arrays.asList(3, 1, 4, 1, 5));
        Collections.sort(nums);
        System.out.println("Collections.sort: " + nums);
        Collections.reverse(nums);
        System.out.println("reverse: " + nums);
        System.out.println("max: " + Collections.max(nums));
        System.out.println("min: " + Collections.min(nums));
    }
}

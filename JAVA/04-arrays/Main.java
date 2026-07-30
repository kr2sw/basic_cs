import java.util.Arrays;

public class Main {
    public static void main(String[] args) {
        // 1차원 배열
        int[] scores = {90, 85, 78, 92, 88};
        System.out.println("길이: " + scores.length);
        System.out.println("첫 번째: " + scores[0]);

        // 향상된 for문
        System.out.print("모든 점수: ");
        for (int score : scores) {
            System.out.print(score + " ");
        }
        System.out.println();

        // 2차원 배열
        int[][] matrix = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };
        System.out.println("matrix[1][2] = " + matrix[1][2]);

        // Arrays 유틸리티
        int[] arr = {5, 3, 1, 4, 2};
        Arrays.sort(arr);
        System.out.println("정렬: " + Arrays.toString(arr));

        int idx = Arrays.binarySearch(arr, 3);
        System.out.println("3의 위치: " + idx);

        int[] copy = Arrays.copyOf(arr, 3);
        System.out.println("복사 (처음 3개): " + Arrays.toString(copy));

        int[] filled = new int[5];
        Arrays.fill(filled, 7);
        System.out.println("fill: " + Arrays.toString(filled));

        // 가변 길이 배열 (ArrayList)
        String[] names = {"Alice", "Bob", "Charlie"};
        var list = Arrays.asList(names);
        System.out.println("리스트: " + list);
    }
}

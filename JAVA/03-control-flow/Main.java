public class Main {
    public static void main(String[] args) {
        int score = 85;

        // if-else if-else
        if (score >= 90) {
            System.out.println("A");
        } else if (score >= 80) {
            System.out.println("B");
        } else if (score >= 70) {
            System.out.println("C");
        } else {
            System.out.println("F");
        }

        // switch (전통적)
        int day = 3;
        switch (day) {
            case 1: System.out.println("월"); break;
            case 2: System.out.println("화"); break;
            case 3: System.out.println("수"); break;
            default: System.out.println("기타");
        }

        // switch 화살표 (Java 14+)
        String grade = switch (score / 10) {
            case 10, 9 -> "A";
            case 8 -> "B";
            case 7 -> "C";
            default -> "F";
        };
        System.out.println("학점: " + grade);

        // for문
        System.out.print("for: ");
        for (int i = 1; i <= 5; i++) {
            System.out.print(i + " ");
        }
        System.out.println();

        // while문
        System.out.print("while: ");
        int j = 1;
        while (j <= 5) {
            System.out.print(j + " ");
            j++;
        }
        System.out.println();

        // do-while문
        System.out.print("do-while: ");
        int k = 1;
        do {
            System.out.print(k + " ");
            k++;
        } while (k <= 5);
        System.out.println();

        // break / continue
        for (int i = 1; i <= 10; i++) {
            if (i % 2 == 0) continue; // 짝수 건너뛰기
            if (i > 7) break;         // 7 초과면 종료
            System.out.print(i + " "); // 1 3 5 7
        }
        System.out.println();
    }
}

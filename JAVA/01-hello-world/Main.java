// 한 줄 주석

/*
  여러 줄 주석
*/

/**
 * 문서 주석 (javadoc)
 */
public class Main {
    public static void main(String[] args) {
        // 기본 출력
        System.out.println("Hello, World!");
        System.out.print("줄바꿈 없음 ");
        System.out.print("같은 줄\n");

        // 서식 출력
        String name = "홍길동";
        int age = 25;
        System.out.printf("이름: %s, 나이: %d%n", name, age);

        // 명령줄 인수
        System.out.println("args.length = " + args.length);
        for (int i = 0; i < args.length; i++) {
            System.out.println("args[" + i + "] = " + args[i]);
        }
    }
}

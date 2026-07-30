public class Main {
    public static void main(String[] args) {
        // Wrapper 클래스
        Integer i1 = Integer.valueOf(100);
        Integer i2 = 200; // 오토박싱
        int sum = i1 + i2; // 언박싱 후 계산
        System.out.println("합계: " + sum);

        // 문자열 → 숫자
        int parsed = Integer.parseInt("456");
        double d = Double.parseDouble("3.14");
        System.out.println("parsed + 1 = " + (parsed + 1));
        System.out.println("d = " + d);

        // Wrapper 유틸리티
        System.out.println("int 최대값: " + Integer.MAX_VALUE);
        System.out.println("int 최소값: " + Integer.MIN_VALUE);

        // String
        String s1 = "Hello";
        String s2 = new String("Hello");
        System.out.println("s1 == s2: " + (s1 == s2));       // false (주소 비교)
        System.out.println("s1.equals(s2): " + s1.equals(s2)); // true (내용 비교)

        // String 메서드
        String str = "  Java Programming  ";
        System.out.println("length: " + str.length());
        System.out.println("trim: '" + str.trim() + "'");
        System.out.println("substring: " + str.trim().substring(5, 11));
        System.out.println("replace: " + str.trim().replace("Java", "Python"));
        System.out.println("split: " + String.join(", ", str.trim().split(" ")));

        // StringBuilder (단일 스레드, 빠름)
        StringBuilder sb = new StringBuilder();
        sb.append("Hello");
        sb.append(" ");
        sb.append("World");
        sb.insert(5, ",");
        sb.deleteCharAt(5);
        System.out.println("StringBuilder: " + sb);
        System.out.println("reverse: " + sb.reverse());

        // StringBuffer (멀티스레드 안전)
        StringBuffer sbf = new StringBuffer("Java");
        sbf.append(" is");
        sbf.append(" awesome");
        System.out.println("StringBuffer: " + sbf);

        // 성능 비교
        long start = System.nanoTime();
        String result = "";
        for (int i = 0; i < 10000; i++) {
            result += "a"; // 비효율적
        }
        long end = System.nanoTime();
        System.out.println("String '+' 소요시간: " + (end - start) / 1000000 + "ms");

        start = System.nanoTime();
        StringBuilder sb2 = new StringBuilder();
        for (int i = 0; i < 10000; i++) {
            sb2.append("a"); // 효율적
        }
        end = System.nanoTime();
        System.out.println("StringBuilder 소요시간: " + (end - start) / 1000000 + "ms");
    }
}

package app;

import static java.lang.Math.*;

public class Main {
    public static void main(String[] args) {
        // java.lang 패키지 (자동 import)
        String str = "Hello";
        System.out.println(str.length());

        // import static
        System.out.println("PI = " + PI);
        System.out.println("sqrt(16) = " + sqrt(16));
        System.out.println("random = " + (int)(random() * 100));

        // java.util 패키지
        java.util.Scanner scanner = new java.util.Scanner(System.in);
        // 또는 import java.util.Scanner; 후 사용
    }

    // 패키지 정보 출력
    static {
        System.out.println("패키지: " + Main.class.getPackage().getName());
    }
}

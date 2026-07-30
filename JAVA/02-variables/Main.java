public class Main {
    public static void main(String[] args) {
        // 기본형 변수
        byte b = 127;
        short s = 32767;
        int i = 2147483647;
        long l = 9223372036854775807L;
        float f = 3.14f;
        double d = 3.141592;
        char c = 'A';
        boolean bool = true;

        System.out.println("byte: " + b);
        System.out.println("int: " + i);
        System.out.println("double: " + d);
        System.out.println("char: " + c);
        System.out.println("boolean: " + bool);

        // 참조형 변수
        String str = "Hello Java";
        System.out.println("String: " + str);

        // 상수 (final)
        final double PI = 3.14159;
        System.out.println("PI = " + PI);

        // 자동 형변환 (묵시적)
        int num = 100;
        long bigNum = num;
        double doubleNum = num;
        System.out.println("int -> long: " + bigNum);
        System.out.println("int -> double: " + doubleNum);

        // 강제 형변환 (명시적)
        double pi = 3.14159;
        int intPi = (int) pi;
        System.out.println("double -> int: " + intPi); // 3 (데이터 손실)

        // 문자열 → 숫자 변환
        String numberStr = "123";
        int parsed = Integer.parseInt(numberStr);
        System.out.println("파싱 결과: " + (parsed + 1)); // 124
    }
}

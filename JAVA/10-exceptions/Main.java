import java.io.*;

// 사용자 정의 예외
class InsufficientBalanceException extends Exception {
    public InsufficientBalanceException(String message) {
        super(message);
    }
}

class BankAccount {
    private int balance;

    public BankAccount(int balance) {
        this.balance = balance;
    }

    public void withdraw(int amount) throws InsufficientBalanceException {
        if (amount > balance) {
            throw new InsufficientBalanceException(
                "잔액 부족: " + balance + "원 중 " + amount + "원 출금 시도");
        }
        balance -= amount;
        System.out.println(amount + "원 출금 완료. 잔액: " + balance + "원");
    }
}

public class Main {
    public static void main(String[] args) {
        // try-catch-finally
        try {
            int result = 10 / 0;
        } catch (ArithmeticException e) {
            System.out.println("0으로 나눌 수 없습니다: " + e.getMessage());
        } finally {
            System.out.println("finally는 항상 실행됩니다.");
        }

        // 다중 catch
        try {
            String str = null;
            System.out.println(str.length());
        } catch (NullPointerException e) {
            System.out.println("NullPointerException: " + e.getMessage());
        } catch (Exception e) {
            System.out.println("기타 예외: " + e.getMessage());
        }

        // 사용자 정의 예외
        BankAccount account = new BankAccount(10000);
        try {
            account.withdraw(5000);
            account.withdraw(8000); // 예외 발생
        } catch (InsufficientBalanceException e) {
            System.out.println("오류: " + e.getMessage());
        }

        // try-with-resources (AutoCloseable)
        try (BufferedReader br = new BufferedReader(new StringReader("Hello\nJava"))) {
            String line;
            while ((line = br.readLine()) != null) {
                System.out.println(line);
            }
        } catch (IOException e) {
            System.out.println("IO 오류: " + e.getMessage());
        }

        System.out.println("프로그램 정상 종료");
    }
}

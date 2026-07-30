<?php
// 사용자 정의 예외
class InsufficientBalanceException extends Exception {
    public function __construct(string $message, int $balance = 0) {
        parent::__construct($message);
        $this->balance = $balance;
    }

    public function getBalance(): int {
        return $this->balance;
    }
}

class BankAccount {
    private int $balance;

    public function __construct(int $balance) {
        $this->balance = $balance;
    }

    public function withdraw(int $amount): void {
        if ($amount > $this->balance) {
            throw new InsufficientBalanceException(
                "잔액 부족: {$this->balance}원 중 {$amount}원 출금 시도",
                $this->balance
            );
        }
        $this->balance -= $amount;
        echo "{$amount}원 출금 완료. 잔액: {$this->balance}원\n";
    }

    public function getBalance(): int {
        return $this->balance;
    }
}

// 기본 try-catch
echo "=== try-catch-finally ===\n";
try {
    $result = 10 / 0;
} catch (DivisionByZeroError $e) {
    echo "0으로 나눌 수 없습니다: " . $e->getMessage() . "\n";
} finally {
    echo "finally는 항상 실행됩니다.\n";
}

// 다중 catch
echo "\n=== 다중 catch ===\n";
try {
    $arr = [];
    echo $arr[0]; // Undefined array key
} catch (Throwable $e) {
    echo get_class($e) . ": " . $e->getMessage() . "\n";
}

// 사용자 정의 예외
echo "\n=== 사용자 정의 예외 ===\n";
$account = new BankAccount(10000);
try {
    $account->withdraw(5000);
    $account->withdraw(8000); // 예외 발생
} catch (InsufficientBalanceException $e) {
    echo "오류: " . $e->getMessage() . "\n";
    echo "현재 잔액: " . $e->getBalance() . "원\n";
} finally {
    echo "최종 잔액: " . $account->getBalance() . "원\n";
}

// 예외 정보
echo "\n=== 예외 정보 ===\n";
try {
    throw new Exception("Something went wrong", 42);
} catch (Exception $e) {
    echo "메시지: " . $e->getMessage() . "\n";
    echo "코드: " . $e->getCode() . "\n";
    echo "파일: " . $e->getFile() . "\n";
    echo "줄: " . $e->getLine() . "\n";
}

// set_error_handler (사용자 정의 에러 핸들러)
echo "\n=== set_error_handler ===\n";
set_error_handler(function(int $severity, string $message, string $file, int $line) {
    echo "에러 [$severity]: $message in $file:$line\n";
});

trigger_error("사용자 정의 경고", E_USER_WARNING);

restore_error_handler();

echo "\n프로그램 정상 종료\n";

# 기본 try/except
try:
    num = int(input("숫자를 입력하세요: "))
    result = 10 / num
    print(f"결과: {result}")
except ValueError:
    print("올바른 숫자를 입력하세요")
except ZeroDivisionError:
    print("0으로 나눌 수 없습니다")

# else와 finally
try:
    x = int("42")
except ValueError:
    print("변환 실패")
else:
    print(f"변환 성공: {x}")  # 예외 없을 때만 실행
finally:
    print("이 블록은 항상 실행됩니다")

# 여러 예외를 한 번에
try:
    data = [1, 2, 3]
    print(data[5])
except (IndexError, KeyError) as e:
    print(f"인덱스/키 오류: {e}")

# as e로 예외 객체 받기
try:
    print(undefined_var)
except NameError as e:
    print(f"에러 메시지: {e}")

# raise로 예외 발생
def check_age(age):
    if age < 0:
        raise ValueError("나이는 음수일 수 없습니다")
    if age > 150:
        raise ValueError("나이가 너무 많습니다")
    print(f"나이 {age}는 유효합니다")

try:
    check_age(-5)
except ValueError as e:
    print(f"오류: {e}")

# 사용자 정의 예외
class InsufficientBalanceError(Exception):
    """잔액 부족 시 발생하는 예외"""
    def __init__(self, balance, amount):
        self.balance = balance
        self.amount = amount
        super().__init__(f"잔액 부족: {balance}원 중 {amount}원 출금 시도")

def withdraw(balance, amount):
    if amount > balance:
        raise InsufficientBalanceError(balance, amount)
    return balance - amount

try:
    withdraw(5000, 10000)
except InsufficientBalanceError as e:
    print(f"사용자 정의 예외: {e}")

# assert (디버깅용)
x = 10
assert x > 0, "x는 양수여야 합니다"
# assert x < 0, "이 줄은 실행되지 않음"

print("프로그램 정상 종료")

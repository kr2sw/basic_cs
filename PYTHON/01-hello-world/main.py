# 주석: 실행되지 않는 설명문입니다

# print() 기초
print("Hello, World!")
print("파이썬", "공부", "시작!")  # 쉼표로 구분, 자동 띄어쓰기
print("A", "B", "C", sep=" -> ")  # sep 파라미터로 구분자 지정
print("첫 줄", end=" / ")
print("같은 줄")  # end 파라미터로 줄바꿈 대신 문자 지정

# input()으로 사용자 입력 받기
name = input("이름을 입력하세요: ")
print(f"반갑습니다, {name}님!")  # f-string

# f-string 표현식
age = 25
print(f"10년 후에는 {age + 10}살이 됩니다.")

# 기본 자료형
정수 = 42
실수 = 3.14
문자열 = "Python"
참거짓 = True
없음 = None

print(type(정수))      # <class 'int'>
print(type(실수))      # <class 'float'>
print(type(문자열))    # <class 'str'>
print(type(참거짓))    # <class 'bool'>
print(type(없음))      # <class 'NoneType'>

# 형변환
print(int("100") + 1)   # 101
print(str(100) + "원")  # "100원"
print(float("3.14"))    # 3.14

# 기본 함수
def greet(name):
    """사용자에게 인사말을 출력합니다."""
    print(f"안녕하세요, {name}님!")

greet("Alice")

# return 값
def add(a, b):
    """두 숫자의 합을 반환합니다."""
    return a + b

result = add(3, 5)
print(f"3 + 5 = {result}")

# 기본값 매개변수
def power(base, exp=2):
    return base ** exp

print(power(3))      # 3^2 = 9
print(power(3, 3))   # 3^3 = 27

# 키워드 인자
def introduce(name, age, city):
    print(f"{name}, {age}세, {city} 거주")

introduce(city="Seoul", age=25, name="Bob")

# *args: 가변 위치 인자
def sum_all(*args):
    return sum(args)

print(sum_all(1, 2, 3, 4, 5))  # 15

# **kwargs: 가변 키워드 인자
def print_info(**kwargs):
    for key, value in kwargs.items():
        print(f"{key}: {value}")

print_info(name="Python", version=3.12, year=2026)

# *args와 **kwargs 함께 사용
def wrapper(*args, **kwargs):
    print(f"위치 인자: {args}")
    print(f"키워드 인자: {kwargs}")

wrapper(1, 2, 3, name="test", value=100)

# lambda (익명 함수)
square = lambda x: x ** 2
print(square(5))  # 25

# lambda with map/filter
nums = [1, 2, 3, 4, 5]
doubled = list(map(lambda x: x * 2, nums))
evens = list(filter(lambda x: x % 2 == 0, nums))
print(doubled)  # [2, 4, 6, 8, 10]
print(evens)    # [2, 4]

# docstring 확인
help(greet)

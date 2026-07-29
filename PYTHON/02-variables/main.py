# 변수 선언과 할당
name = "Python"
version = 3.12
year = 2026
is_fun = True

print(name, version, year, is_fun)

# 동적 타이핑: 변수는 언제든 다른 타입으로 바뀔 수 있음
x = 10
print(x, type(x))  # int

x = "이제 문자열"
print(x, type(x))  # str

x = 3.14
print(x, type(x))  # float

# type() 활용
print(type(42))        # <class 'int'>
print(type("hello"))   # <class 'str'>
print(type([1, 2]))    # <class 'list'>

# 여러 변수 한 번에 할당
a, b, c = 1, 2, 3
print(a, b, c)

# swap (값 교환)
x, y = 10, 20
x, y = y, x
print(f"x={x}, y={y}")  # x=20, y=10

# 같은 값 여러 변수에 할당
x = y = z = 0
print(x, y, z)

# None: 값이 없음을 나타냄
result = None
print(result)          # None
print(type(result))    # <class 'NoneType'>

# None 체크
if result is None:
    print("result는 None입니다")

# 변수 삭제
temp = "곧 사라짐"
del temp
# print(temp)  # NameError

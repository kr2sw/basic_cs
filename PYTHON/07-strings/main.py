# 문자열 메서드
text = "  Hello, Python World!  "

print(text.strip())            # 양쪽 공백 제거
print(text.lower())            # 소문자
print(text.upper())            # 대문자
print(text.replace("Python", "파이썬"))  # 치환
print(text.split(","))         # 쉼표로 분할

# split과 join
csv = "apple,banana,cherry"
items = csv.split(",")
print(items)  # ['apple', 'banana', 'cherry']

joined = " | ".join(items)
print(joined)  # "apple | banana | cherry"

# 검색 메서드
s = "Hello, welcome to Python"
print(s.find("Python"))     # 18 (인덱스)
print(s.find("Java"))       # -1 (없음)
print(s.count("o"))         # 3
print(s.startswith("Hello"))  # True
print(s.endswith("Python"))   # True

# f-string
name = "Alice"
age = 30
print(f"{name} is {age} years old")
print(f"{name:*^10}")  # 가운데 정렬: **Alice***
print(f"{age:05d}")    # 5자리 0패딩: 00030

# .format() 메서드
print("{} is {} years old".format(name, age))
print("{1} is {0} years old".format(age, name))
print("{name} is {age} years old".format(name="Bob", age=25))

# % 연산자
print("%s is %d years old" % (name, age))

# 문자열 슬라이싱
s = "Python Programming"
print(s[0:6])       # "Python"
print(s[7:])        # "Programming"
print(s[:6])        # "Python"
print(s[::2])       # "Pto rgamn"
print(s[::-1])      # "gnimmargorP nohtyP"

# 이스케이프 문자
print("첫 줄\n두 번째 줄")
print("탭\t간격")
print("쌍따옴표: \"안녕\"")
print(r"Raw string: \n 은 문자 그대로")

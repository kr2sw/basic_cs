# if / elif / else
score = 85

if score >= 90:
    print("A 학점")
elif score >= 80:
    print("B 학점")
elif score >= 70:
    print("C 학점")
else:
    print("D 학점")

# 조건부 표현식 (삼항 연산자)
age = 20
status = "성인" if age >= 19 else "미성년자"
print(status)

# for 반복문
print("--- for with range ---")
for i in range(5):         # 0, 1, 2, 3, 4
    print(i, end=" ")
print()

for i in range(2, 8, 2):   # 2, 4, 6
    print(i, end=" ")
print()

# for with list
fruits = ["사과", "바나나", "체리"]
for fruit in fruits:
    print(f"{fruit} 맛있어요!")

# enumerate: 인덱스와 값 함께 가져오기
for idx, fruit in enumerate(fruits):
    print(f"{idx}번째: {fruit}")

# while 반복문
print("--- while ---")
count = 0
while count < 3:
    print(f"count: {count}")
    count += 1

# break
print("--- break ---")
for i in range(10):
    if i == 5:
        break
    print(i, end=" ")
print()

# continue
print("--- continue ---")
for i in range(10):
    if i % 2 == 0:
        continue
    print(i, end=" ")  # 1 3 5 7 9
print()

# pass: 아무것도 하지 않음
for i in range(5):
    if i == 2:
        pass  # 나중에 구현 예정
    else:
        print(i, end=" ")
print()

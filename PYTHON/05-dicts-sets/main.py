# 딕셔너리 생성
person = {
    "name": "Alice",
    "age": 25,
    "city": "Seoul"
}
print(person)

# 접근 및 수정
print(person["name"])        # Alice
person["age"] = 26
person["job"] = "Engineer"   # 새 키 추가
print(person)

# get() 안전 조회
print(person.get("name"))         # Alice
print(person.get("salary", 0))    # 0 (기본값)

# 딕셔너리 메서드
print(person.keys())    # dict_keys(['name', 'age', 'city', 'job'])
print(person.values())  # dict_values(['Alice', 26, 'Seoul', 'Engineer'])
print(person.items())   # dict_items([('name', 'Alice'), ...])

# 순회
for key, value in person.items():
    print(f"{key}: {value}")

# pop, update
age = person.pop("age")
print(f"제거된 age: {age}")
print(person)

person.update({"city": "Busan", "hobby": "Reading"})
print(person)

# 세트 생성
fruits = {"apple", "banana", "cherry", "apple"}  # 중복 제거
print(fruits)  # {'cherry', 'banana', 'apple'}

# 빈 세트는 set() 사용
empty_set = set()
print(type(empty_set))

# 세트 연산
a = {1, 2, 3, 4, 5}
b = {4, 5, 6, 7, 8}

print(a | b)  # 합집합 {1, 2, 3, 4, 5, 6, 7, 8}
print(a & b)  # 교집합 {4, 5}
print(a - b)  # 차집합 {1, 2, 3}
print(a ^ b)  # 대칭 차집합 {1, 2, 3, 6, 7, 8}

# 세트 메서드
s = {1, 2, 3}
s.add(4)
s.remove(2)         # 없으면 KeyError
s.discard(10)       # 없어도 에러 없음
print(s)            # {1, 3, 4}

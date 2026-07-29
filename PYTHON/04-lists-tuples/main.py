# 리스트 생성
nums = [10, 20, 30, 40, 50]
mixed = [1, "hello", 3.14, True]
empty = []

print(nums, mixed, empty)

# 인덱싱
print(nums[0])    # 10
print(nums[-1])   # 50
print(nums[-2])   # 40

# 슬라이싱 [start:stop:step]
print(nums[1:4])      # [20, 30, 40]
print(nums[:3])       # [10, 20, 30]
print(nums[::2])      # [10, 30, 50]
print(nums[::-1])     # [50, 40, 30, 20, 10]

# 리스트 메서드
fruits = ["apple", "banana"]
fruits.append("cherry")      # 끝에 추가
print(fruits)

fruits.insert(1, "blueberry")  # 1번 위치에 삽입
print(fruits)

fruits.remove("banana")      # 값으로 제거 (첫 번째만)
print(fruits)

last = fruits.pop()          # 마지막 요소 제거 후 반환
print(f"pop: {last}, 남은: {fruits}")

first = fruits.pop(0)        # 0번 인덱스 제거
print(f"pop(0): {first}, 남은: {fruits}")

# 정렬과 뒤집기
nums = [3, 1, 4, 1, 5, 9]
nums.sort()
print(f"sort: {nums}")
nums.reverse()
print(f"reverse: {nums}")

# 리스트 컴프리헨션
squares = [x**2 for x in range(5)]
print(f"squares: {squares}")

# 튜플: 불변 시퀀스
point = (3, 4)
print(point[0], point[1])

# 튜플 언패킹
x, y = point
print(f"x={x}, y={y}")

# 튜플은 수정 불가
# point[0] = 10  # TypeError!

# 튜플 메서드
t = (1, 2, 2, 3, 2)
print(f"count(2): {t.count(2)}")
print(f"index(3): {t.index(3)}")

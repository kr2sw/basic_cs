from pathlib import Path

# 파일 쓰기 (w: 덮어쓰기)
with open("sample.txt", "w", encoding="utf-8") as f:
    f.write("첫 번째 줄\n")
    f.write("두 번째 줄\n")
    f.write("세 번째 줄\n")

print("파일 쓰기 완료")

# 파일 읽기 (r)
with open("sample.txt", "r", encoding="utf-8") as f:
    content = f.read()
print("--- read() ---")
print(content)

# readline() - 한 줄씩 읽기
with open("sample.txt", "r", encoding="utf-8") as f:
    line = f.readline()
    while line:
        print(f"한 줄: {line.strip()}")
        line = f.readline()

# readlines() - 모든 줄을 리스트로
with open("sample.txt", "r", encoding="utf-8") as f:
    lines = f.readlines()
print("--- readlines() ---")
print(lines)

# 파일에 추가 (a: append)
with open("sample.txt", "a", encoding="utf-8") as f:
    f.write("네 번째 줄 (추가됨)\n")

# 추가 내용 확인
with open("sample.txt", "r", encoding="utf-8") as f:
    print("--- append 후 ---")
    print(f.read())

# pathlib 사용
p = Path("sample.txt")
print(f"파일명: {p.name}")
print(f"확장자: {p.suffix}")
print(f"크기: {p.stat().st_size} bytes")
print(f"존재 여부: {p.exists()}")

# Path로 파일 읽기
content = p.read_text(encoding="utf-8")
print(f"--- Path.read_text() ---\n{content}")

# Path로 파일 쓰기
p2 = Path("pathlib_output.txt")
p2.write_text("pathlib로 작성한 파일입니다.\n", encoding="utf-8")
print(f"{p2.name} 생성 완료")

# 임시 파일 정리
import os
os.remove("sample.txt")
os.remove("pathlib_output.txt")
print("임시 파일 정리 완료")

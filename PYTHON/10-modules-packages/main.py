"""
10-modules-packages 메인 예제
"""
import sys
import os
import math
from datetime import datetime
from random import randint, choice

# 표준 라이브러리 사용
print(f"파이: {math.pi:.2f}")
print(f"랜덤 1-10: {randint(1, 10)}")
print(f"현재 시간: {datetime.now()}")

# sys.argv: 명령줄 인자
print(f"스크립트명: {sys.argv[0]}")
if len(sys.argv) > 1:
    print(f"첫 번째 인자: {sys.argv[1]}")

# os 모듈
print(f"현재 디렉토리: {os.getcwd()}")
print(f"파일 목록: {os.listdir('.')}")  # 현재 폴더

# os.path
path = os.path.join("folder", "subfolder", "file.txt")
print(f"조인된 경로: {path}")
print(f"dirname: {os.path.dirname(path)}")

# __name__ == '__main__' 활용
def main():
    """진입점 함수"""
    print("=== 모듈/패키지 데모 ===")
    print(f"__name__: {__name__}")
    print(f"sys.path: {sys.path[:3]} ...")

# 직접 실행 시에만 main() 호출
if __name__ == '__main__':
    main()

# 간단한 모듈 만들기 (같은 폴더에 mymodule.py 생성)
# mymodule.py 내용:
#   def hello(name):
#       return f"Hello, {name}!"
#   PI = 3.14159
#
# 사용: import mymodule / from mymodule import hello

"""
33: 고급 정규표현식 — 전방탐색, 역참조, 이름 그룹, 플래그
"""
import re

text = "가격은 100원과 250원, 그리고 3,000원입니다."

print("=== 1) 전방탐색 (?=) : 숫자만 추출 ===")
prices = re.findall(r"\d{1,3}(?:,\d{3})*(?=원)", text)
print("  금액들:", prices)

print()
print("=== 2) 부정 전방탐색 (?!) : '원'이 뒤에 오지 않는 숫자 ===")
non_prices = re.findall(r"\b\d+\b(?!원)", text)
print("  원이 붙지 않은 숫자:", non_prices)

print()
print("=== 3) 후방탐색 (?<=) : 접두어 포함 시키지 않기 ===")
colors = "색상:빨강,색상:파랑,기타:초록"
reds = re.findall(r"(?<=색상:)\w+", colors)
print("  색상값들:", reds)

print()
print("=== 4) 역참조 \\1 : 같은 단어가 연속된 경우 ===")
dup = re.findall(r"\b(\w+)\s+\1\b", "나는 나는 정말 정말 행복해")
print("  반복 단어:", dup)

print()
print("=== 5) 이름 있는 그룹 (?P<name>) ===")
log = "2026-07-31 14:03:22 ERROR 서버 오류 발생"
m = re.search(r"(?P<date>\d{4}-\d{2}-\d{2}) (?P<time>\d{2}:\d{2}:\d{2}) (?P<level>\w+) (?P<msg>.+)", log)
if m:
    print("  date:", m.group("date"))
    print("  time:", m.group("time"))
    print("  level:", m.group("level"))
    print("  msg:", m.group("msg"))

print()
print("=== 6) re.sub 함수로 치환 ===")
def make_link(match):
    return f'<a href="{match.group(1)}">{match.group(1)}</a>'

text2 = "공식 문서는 python.org 참고하세요"
linked = re.sub(r"\b([a-z]+\.[a-z]{2,})\b", make_link, text2)
print("  ", linked)

print()
print("=== 7) 플래그 활용 ===")
print("  IGNORECASE:", re.findall(r"python", "Python PYTHON python", re.IGNORECASE))
multi = "1. 첫째\n2. 둘째\n3. 셋째"
m = re.search(r"^3\.", multi, re.MULTILINE)  # ^가 줄 단위로 동작
print("  MULTILINE ^3.:", m.group(0) if m else None)
verbose_pattern = re.compile(
    r"""
    (?P<year>\d{4})   # 연도
    [-\/]
    (?P<month>\d{2})  # 월
    [-\/]
    (?P<day>\d{2})    # 일
    """,
    re.VERBOSE,
)
m = verbose_pattern.search("기준일: 2026/07/31")
print("  VERBOSE 파싱:", (m.group("year"), m.group("month"), m.group("day")))

print()
print("=== 8) finditer와 위치 정보 ===")
for m in re.finditer(r"원", text):
    print(f"  '원' at index {m.start()}-{m.end()}")

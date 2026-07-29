# 07: Strings — 문자열 메서드, 포맷팅, 슬라이싱

## 문자열 메서드
- `split(sep)`: 구분자로 분할하여 리스트 반환
- `join(iterable)`: 리스트를 문자열로 합침
- `strip()`: 양쪽 공백 제거
- `replace(old, new)`: 문자열 치환
- `upper()` / `lower()`: 대소문자 변환
- `startswith()`, `endswith()`, `find()`, `count()`

```python
text = "  hello world  "
print(text.strip().upper())  # "HELLO WORLD"
```

## 문자열 포맷팅
- f-strings (3.6+): `f"{변수}"`
- `.format()`: `"{}".format(값)`
- `%` 연산자: `"%s %d" % (name, age)`

## 문자열 슬라이싱
문자열도 시퀀스이므로 인덱싱과 슬라이싱이 가능합니다.

```python
s = "Python"
print(s[0:3])  # "Pyt"
print(s[::-1]) # "nohtyP"
```

## 주요 이스케이프 문자
`\n` (줄바꿈), `\t` (탭), `\\` (역슬래시), `\"` (따옴표)

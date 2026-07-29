# 08: File I/O — 파일 읽기/쓰기, with 문, pathlib

## open()과 with 문
`with` 문을 사용하면 파일을 자동으로 닫아줍니다.

```python
with open("file.txt", "r", encoding="utf-8") as f:
    content = f.read()
```

## 파일 모드
- `"r"`: 읽기 (기본값)
- `"w"`: 쓰기 (기존 내용 덮어쓰기)
- `"a"`: 추가 (이어쓰기)
- `"x"`: 새 파일 생성 (이미 있으면 오류)
- `"b"`: 바이너리 모드 (`"rb"`, `"wb"`)

## 읽기 메서드
- `read()`: 전체 내용을 하나의 문자열로
- `readline()`: 한 줄 읽기
- `readlines()`: 모든 줄을 리스트로

## pathlib basics
`pathlib.Path`로 경로를 객체처럼 다룹니다.

```python
from pathlib import Path
p = Path("data.txt")
print(p.exists(), p.name, p.stem, p.suffix)
```

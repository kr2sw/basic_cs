# 03: 파일 관리 — .gitignore, 파일 이동, 삭제

## .gitignore

추적하지 않을 파일 패턴을 지정합니다.

```gitignore
# OS 파일
.DS_Store
Thumbs.db

# 빌드 결과
bin/
obj/
*.exe
*.dll

# IDE
.vs/
.idea/
*.sublime-*

# 언어/프레임워크
node_modules/
.env
__pycache__/
*.pyc

# 로그
*.log
```

## 파일 이동/삭제

```bash
# 파일 삭제 (스테이징 자동)
git rm 파일.txt

# 파일 이름 변경
git mv 이전이름 새이름

# 또는 수동으로 처리
rm 파일.txt
git add 파일.txt
git commit -m "Remove file"
```

## 추적 상태

| 상태 | 의미 |
|------|------|
| ?? | Untracked (새 파일, 추적 안 함) |
| A | Added (스테이징 됨) |
| M | Modified (수정됨) |
| D | Deleted (삭제됨) |
| R | Renamed (이름 변경) |

# 02: 기본 명령어 — add, commit, status

Git 워크플로의 핵심: Working Directory → Staging Area → Repository

## 명령어

```bash
# 파일 상태 확인
git status                    # 상세 상태
git status -s                 # 간략 상태 (M: 수정, A: 추가, ?: 추적 안 됨)

# Staging에 추가
git add 파일.txt              # 특정 파일
git add *.txt                 # 패턴 매칭
git add -p                    # 대화형으로 부분 추가 (hunk 단위)

# 커밋
git commit -m "메시지"        # 인라인 메시지
git commit                    # 에디터 열림
git commit -am "메시지"       # tracked 파일 add + commit 한 번에

# 변경 내용 보기
git diff                      # WD ↔ Staging
git diff --staged             # Staging ↔ 마지막 커밋
```

## 실습

```bash
mkdir demo && cd demo
git init
echo "hello" > file.txt
git add file.txt
git commit -m "First commit"
echo "world" >> file.txt
git add .
git commit -m "Second commit"
git log --oneline
```

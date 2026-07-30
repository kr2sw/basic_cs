# 13: Stashing — 작업 임시 저장

커밋하지 않은 변경 사항을 임시로 저장하고 나중에 다시 적용합니다.

## 기본 명령어

```bash
# 현재 작업 저장 (untracked 제외)
git stash

# 메시지와 함께 저장
git stash push -m "WIP: login feature"

# untracked 파일도 포함
git stash --include-untracked

# 모든 파일 포함 (untracked + ignored)
git stash --all
```

## 저장된 작업 확인 및 적용

```bash
# stash 목록
git stash list
# stash@{0}: WIP: login feature
# stash@{1}: On feature/login: temp work

# stash 적용 (가장 최근)
git stash pop              # 적용 + stash에서 제거
git stash apply            # 적용 + stash에 유지

# 특정 stash 적용
git stash pop stash@{1}
git stash apply stash@{1}
```

## 고급

```bash
# stash에서 브랜치 생성
git stash branch 새브랜치 stash@{0}

# stash 삭제
git stash drop stash@{0}   # 특정 삭제
git stash clear            # 전체 삭제

# stash 내용 보기
git stash show stash@{0}
git stash show -p stash@{0}  # 상세 diff
```

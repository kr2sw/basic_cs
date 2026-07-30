# 14: Rebase — 커밋 재배치

rebase는 커밋 이력을 깔끔하게 정리합니다.

## Branch Rebase

```bash
# feature 브랜치를 main 위로 재배치
git switch feature
git rebase main

# (main으로 돌아가서 fast-forward 병합)
git switch main
git merge feature
```

## merge vs rebase

```
merge:       rebase:
A-B-C       A-B
  \           \
   D-E         D'-E'
```

- **merge**: 변경 이력 보존, 병합 커밋 생성
- **rebase**: 선형 이력, 깔끔함

## 대화형 Rebase

```bash
# 최근 3개 커밋 수정
git rebase -i HEAD~3

# 대화형 명령어
# pick    = 커밋 유지
# reword  = 커밋 메시지 변경
# edit    = 커밋 내용 변경
# squash  = 이전 커밋과 병합
# fixup   = squash + 메시지 버림
# drop    = 커밋 삭제
```

## 주의사항

⚠️ **이미 푸시된 커밋은 rebase하지 마세요!**
- 다른 개발자와 공유된 커밋을 변경하면 혼란 발생
- 로컬에서만 rebase하고, 강제 푸시는 금물

```bash
# (예외) 혼자 작업하는 브랜치는 가능
git push --force-with-lease   # 안전한 강제 푸시
```

# 18: 고급 기능

## Submodule

다른 Git 저장소를 현재 저장소에 포함합니다.

```bash
# 서브모듈 추가
git submodule add https://github.com/사용자/라이브러리.git libs/라이브러리

# 서브모듈 포함 clone
git clone --recursive https://github.com/사용자/저장소.git

# 서브모듈 업데이트
git submodule update --init --recursive
```

## Worktree

같은 저장소의 여러 브랜치를 동시에 작업합니다.

```bash
git worktree add ../project-hotfix hotfix
git worktree add ../project-feature feature/login
git worktree list
git worktree remove ../project-hotfix
```

## Bisect (이진 탐색)

버그가 처음 발생한 커밋을 찾습니다.

```bash
git bisect start
git bisect bad HEAD          # 현재는 버그 있음
git bisect good v1.0.0       # 예전에는 정상
# Git이 중간 커밋으로 이동 → 테스트 후:
git bisect good              # 여기까지는 괜찮음
git bisect bad               # 여기서부터 버그
# ... 반복 → 첫 번째 문제 커밋 발견!
git bisect reset
```

## Reflog (모든 작업 기록)

```bash
git reflog                   # 모든 HEAD 이동 기록
git reset --hard HEAD@{2}    # 실수로 날린 커밋 복구
```

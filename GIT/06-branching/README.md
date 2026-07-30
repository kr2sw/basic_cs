# 06: 브랜치 — branch, checkout, switch

브랜치는 독립적인 작업 공간을 만듭니다.

## 명령어

```bash
# 브랜치 목록
git branch                    # 로컬 브랜치
git branch -a                 # 모든 브랜치 (원격 포함)
git branch -r                 # 원격 브랜치만

# 브랜치 생성
git branch feature/login      # 새 브랜치 생성
git switch feature/login      # 브랜치 이동
git switch -c feature/login   # 생성 + 이동

# 또는 (구식)
git checkout -b feature/login

# 브랜치 삭제
git branch -d feature/login   # 병합 완료된 브랜치 삭제
git branch -D feature/login   # 강제 삭제
```

## 브랜치 병합 상황

```bash
# main 브랜치로 이동 후
git switch main
git merge feature/login
```

## 브랜치 이름 컨벤션

| 패턴 | 예시 |
|------|------|
| feature/ | feature/user-auth |
| bugfix/ | bugfix/login-error |
| hotfix/ | hotfix/security-patch |
| release/ | release/v1.2.0 |

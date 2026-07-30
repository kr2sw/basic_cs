# 09: 원격 저장소 — remote, push, pull, fetch

## 원격 저장소 연결

```bash
# 새 원격 추가
git remote add origin https://github.com/사용자/저장소.git

# SSH 사용
git remote add origin git@github.com:사용자/저장소.git

# 원격 목록
git remote -v
```

## push (로컬 → 원격)

```bash
git push origin main              # main 브랜치 푸시
git push -u origin main           # -u: 추적 브랜치 설정 (처음)
git push origin feature/login     # 특정 브랜치 푸시
git push --all                    # 모든 브랜치 푸시
git push --tags                   # 태그 푸시
```

## pull/push (원격 → 로컬)

```bash
git pull                          # fetch + merge
git pull --rebase                 # fetch + rebase (깔끔한 이력)
git fetch                         # 원격 변경사항만 확인
git fetch origin main             # 특정 브랜치 fetch
```

## 원격 브랜치

```bash
git switch -c feature origin/feature   # 원격 브랜치로 로컬 생성
git branch -d origin/feature           # 원격 브랜치 삭제
git push origin --delete feature       # 원격에서 삭제
```

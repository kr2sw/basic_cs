# 17: 협업 워크플로우

## GitHub Flow (단순)

```
main ─── feature1 ─── PR ─── merge
     └── feature2 ─── PR ─── merge
```

```bash
# 1. main에서 브랜치 생성
git switch -c feature/awesome
# 2. 작업 후 커밋
git add . && git commit -m "Add awesome feature"
# 3. 푸시하고 PR 생성
git push -u origin feature/awesome
# 4. 리뷰 후 main에 merge
# 5. 브랜치 삭제
git branch -d feature/awesome
```

## GitFlow (복잡/대규모)

```
master ─── v1.0 ─── v1.1 ─── v2.0
   └── develop ─── feature1 ─── PR
                └── release/v1.0 ─── bugfix
```

| 브랜치 | 용도 |
|--------|------|
| master | 배포된 코드 |
| develop | 다음 릴리즈 개발 |
| feature/* | 새 기능 개발 |
| release/* | 릴리즈 준비 |
| hotfix/* | 긴급 버그 수정 |

## 컨벤션

### 커밋 메시지 (Conventional Commits)
```
feat: add user login
fix: fix null reference in parser
docs: update API docs
refactor: extract validation logic
test: add unit tests for parser
chore: update dependencies
```

### 브랜치 보호 규칙 (GitHub)
- PR 필수
- 리뷰 승인 필수
- CI 통과 필수
- force push 금지

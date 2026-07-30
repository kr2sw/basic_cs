# 20: 실전 프로젝트 — GitHub 협업 워크플로

## 시나리오: 오픈소스 기여하기

### 1. Fork & Clone

```bash
# GitHub에서 프로젝트 Fork
git clone https://github.com/내계정/프로젝트.git
cd 프로젝트
git remote add upstream https://github.com/원본/프로젝트.git
```

### 2. 작업 브랜치 생성

```bash
git switch -c feature/awesome-feature
```

### 3. 개발 & 커밋

```bash
# Conventional Commits 사용
git add .
git commit -m "feat: add awesome feature"
git commit -m "fix: handle edge case"
```

### 4. 최신 상태 유지

```bash
git fetch upstream
git rebase upstream/main
# (충돌 발생 시 해결 후 git rebase --continue)
```

### 5. PR 생성

```bash
git push -u origin feature/awesome-feature
# GitHub에서 PR 생성
```

### 6. 리뷰 반영

```bash
# 리뷰어의 피드백 반영
git add . && git commit -m "refactor: apply review feedback"
git push
# PR에 자동 반영
```

### 7. Squash & Merge

```bash
# PR 머지 후 로컬 정리
git switch main
git pull upstream main
git branch -d feature/awesome-feature
```

## 추천 도구

| 도구 | 용도 |
|------|------|
| VS Code + GitLens | 시각적 Git 관리 |
| gh (GitHub CLI) | CLI로 GitHub 작업 |
| lazygit | TUI Git 클라이언트 |
| pre-commit | 커밋 전 자동 검사 |

## GitHub CLI

```bash
gh repo create my-project --public --clone
gh pr create --base main --title "PR 제목" --body "설명"
gh pr review --approve
gh pr merge --squash
gh issue list
gh run watch
```

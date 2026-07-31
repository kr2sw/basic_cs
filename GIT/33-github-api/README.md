# 33: GitHub API — gh CLI, REST API, 자동화 스크립트

GitHub CLI(`gh`)와 REST API를 사용해 저장소 작업을 자동화합니다.

## gh CLI

```bash
gh repo view
gh pr create --title "..." --body "..."
gh pr merge --squash
gh workflow run <workflow>
```

## REST API

```bash
curl -H "Authorization: token $TOKEN" https://api.github.com/repos/{owner}/{repo}/issues
```

## 자동화 시나리오

- 이슈 생성/라벨 지정
- PR 목록 조회 및 상태 확인
- CI 결과 조회 (`gh run watch`)
- 릴리즈 생성

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```

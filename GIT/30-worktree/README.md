# 30: 워크트리 — 병렬 작업, 컨텍스트 전환

`git worktree`는 한 저장소에서 여러 브랜치를 **동시에** 다른 디렉터리에서 체크아웃하는 기능입니다.

## 기본 사용법

```bash
git worktree add ../feature-hotfix feature/hotfix
git worktree list
git worktree remove ../feature-hotfix
```

## 장점

- 브랜치 전환 없이 병렬 작업 가능
- 리뷰 중인 브랜치 유지 + 새 작업 동시 진행
- 백그라운드 빌드 시 유용

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```

# 21: 브랜치 전략 — trunk-based, GitFlow, feature branch

팀의 규모와 배포 주기에 따라 브랜치 전략을 선택합니다.

## trunk-based (트렁크 기반)

- 모든 개발자가 `main`(트렁크)에 직접 커밋
- 짧은 브랜치 수명, PR 병합 후 즉시 배포
- 소규모 팀 / CI/CD 자동화에 적합

## GitFlow

- `main`, `develop`, `feature/*`, `release/*`, `hotfix/*` 브랜치 사용
- 체계적이지만 복잡, 정기 릴리즈 제품에 적합

## Feature branch

- 기능별 브랜치를 만들어 작업 후 PR로 병합
- 충돌 최소화, 코드 리뷰 용이

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```

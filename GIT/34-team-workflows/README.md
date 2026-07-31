# 34: 팀 워크플로 — 보호된 브랜치, 코드 리뷰 규칙

여러 개발자가 협업할 때 저장소 설정으로 코드 품질을 강제합니다.

## 보호된 브랜치 (Protected Branches)

GitHub → Settings → Branches → Branch protection rules

- **Require PR review**: 병합 전 승인 1~2명
- **Require status checks**: CI 테스트 통과 필수
- **Require conversation resolution**: 코멘트 해결 필수
- **Restrict force pushes / deletions**

## 코드 리뷰 규칙

- PR 제목은 컨벤션 준수 (`feat:`, `fix:`)
- 작은 PR 단위 유지 (200줄 미만)
- 승인 전 CI 상태 확인
- 리뷰 댓글은 난독성 없는 코멘트 사용

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```

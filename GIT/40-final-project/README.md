# 40: 실전 프로젝트 — 전체 CI/CD 워크플로 자동화 스크립트

중급 과정의 모든 내용(브랜치 전략, Actions, 보안, 릴리즈)을 결합한 최종 실습입니다.

## 프로젝트 구성

- `.github/workflows/ci.yml` — lint/테스트/빌드/게이트
- `.github/workflows/deploy.yml` — staging → production
- `.githooks/pre-commit` — 시크릿 검사 + 포맷
- `release.ps1` — SemVer 기반 버전/태그 자동화

## 흐름

```
push/PR → CI(린트·테스트·빌드) → 병합 → 배포(승인) → 릴리즈 태그
```

`demo.ps1`이 이 전체 구조를 로컬 저장소에 생성합니다.

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```

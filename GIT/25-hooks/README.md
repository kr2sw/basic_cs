# 25: Git 훅 — pre-commit, prepare-commit-msg, 커스텀 훅

Git 훅(Hook)은 특정 git 이벤트 발생 시 자동으로 실행되는 스크립트입니다. `.git/hooks/`에 위치하며 확장자가 없어야 합니다.

## 주요 훅

- **pre-commit**: 커밋 전에 실행. 실패 시 커밋 중단 (린트, 테스트)
- **prepare-commit-msg**: 커밋 메시지 템플릿/접두어 주입
- **commit-msg**: 커밋 메시지 검증
- **pre-push**: push 전 검사

## 샘플 pre-commit

```bash
#!/bin/sh
# 실행 파일 권한 필요 (Windows는 git bash 기준)
npx eslint src/
```

팀 전체에 훅을 공유하려면 `core.hooksPath`를 `.githooks/`로 지정하거나, husky/lint-staged를 사용합니다.

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```

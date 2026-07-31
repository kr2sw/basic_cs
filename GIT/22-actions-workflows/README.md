# 22: GitHub Actions — 워크플로우, 이벤트, 잡 구조

GitHub Actions는 저장소 이벤트(push, PR 등)에 반응해 CI/CD 작업을 자동 실행합니다.

## 워크플로우 기본 구조

```yaml
name: CI
on:
  push:
    branches: [main]
  pull_request:

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
      - run: npm ci && npm test
```

## 핵심 요소

- **Workflow**: `.github/workflows/*.yml` 파일
- **Event**: 트리거 (`push`, `pull_request`, `schedule` 등)
- **Job**: 독립 실행 단위 (병렬 가능)
- **Step**: 잡 내부의 개별 명령
- **Runner**: 작업 실행 환경 (ubuntu/windows/macos)

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```

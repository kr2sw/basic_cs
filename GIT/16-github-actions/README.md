# 16: GitHub Actions — CI/CD 기초

GitHub Actions는 자동화된 워크플로우를 실행합니다.

## 기본 구조

```yaml
# .github/workflows/ci.yml
name: CI

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4
      - name: Run a script
        run: echo "Hello, GitHub Actions!"
```

## 언어별 예제

### Node.js
```yaml
      - uses: actions/setup-node@v4
        with:
          node-version: 20
      - run: npm ci
      - run: npm test
```

### Python
```yaml
      - uses: actions/setup-python@v5
        with:
          python-version: "3.12"
      - run: pip install -r requirements.txt
      - run: pytest
```

### .NET
```yaml
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: "8.0"
      - run: dotnet restore
      - run: dotnet test
```

## 주요 기능

- **Matrix Build**: 여러 OS/버전에서 동시 테스트
- **Cache**: 의존성 캐싱으로 속도 향상
- **Secrets**: 민감한 정보 저장 (`${{ secrets.MY_SECRET }}`)
- **Artifacts**: 빌드 결과물 업로드/다운로드
- **Scheduled**: cron으로 정기 실행

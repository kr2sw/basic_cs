# 32: CI/CD 파이프라인 설계 데모
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-32"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root -Force | Out-Null

try {
  Set-Location $root
  git init | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"
  New-Item -ItemType Directory -Path ".github\workflows" -Force | Out-Null

  Write-Host "== 풀 파이프라인 워크플로우 설계 ==" -ForegroundColor Cyan
  $pipeline = @'
name: Full Pipeline
on: push

jobs:
  lint:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: echo "lint"
  unit-test:
    runs-on: ubuntu-latest
    needs: lint
    steps:
      - uses: actions/checkout@v4
      - run: echo "unit test"
  build:
    runs-on: ubuntu-latest
    needs: unit-test
    steps:
      - run: mkdir dist && echo "build" > dist/app.js
      - uses: actions/upload-artifact@v4
        with:
          name: build-output
          path: dist/
  e2e:
    runs-on: ubuntu-latest
    needs: build
    steps:
      - uses: actions/download-artifact@v4
        with:
          name: build-output
      - run: echo "e2e test"
  deploy-staging:
    runs-on: ubuntu-latest
    needs: e2e
    environment: staging
    steps:
      - run: echo "staging 배포"
  deploy-prod:
    runs-on: ubuntu-latest
    needs: deploy-staging
    environment: production
    steps:
      - run: echo "production 배포 (승인 필요)"
'@
  Set-Content ".github\workflows\pipeline.yml" $pipeline
  git add .; git commit -m "CI/CD 파이프라인 추가" | Out-Null

  Write-Host "파이프라인 흐름:"
  Write-Host "  lint → unit-test → build → e2e → staging → production"
  Write-Host "  (각 단계가 게이트 역할, needs로 의존성 표현)"
  Write-Host "  아티팩트: build에서 upload, e2e에서 download"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

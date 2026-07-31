# 22: GitHub Actions 워크플로우 구조 데모
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-22"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== GitHub Actions 워크플로우 생성 ==" -ForegroundColor Cyan
  New-Item -ItemType Directory -Path ".github\workflows" -Force | Out-Null
  $workflow = @'
name: CI
on:
  push:
    branches: [main]
  pull_request:

jobs:
  lint:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: 린트
        run: echo "lint 통과"
  test:
    runs-on: ubuntu-latest
    needs: lint
    steps:
      - uses: actions/checkout@v4
      - name: 테스트
        run: echo "test 통과"
  build:
    runs-on: ubuntu-latest
    needs: test
    steps:
      - uses: actions/checkout@v4
      - name: 빌드
        run: echo "build 통과"
'@
  Set-Content ".github\workflows\ci.yml" $workflow
  git add .; git commit -m "CI 워크플로우 추가" | Out-Null

  Write-Host "== 워크플로우 파일 미리보기 ==" -ForegroundColor Cyan
  Get-Content ".github\workflows\ci.yml"
  Write-Host ""
  Write-Host "== 구조 요약 ==" -ForegroundColor Cyan
  Write-Host "  - Event: push/pull_request"
  Write-Host "  - Jobs: lint -> test -> build (needs로 순서 강제)"
  Write-Host "  - GitHub에 push하면 Actions 탭에서 자동 실행됨"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

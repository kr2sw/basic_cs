# 40: 실전 프로젝트 데모 — CI/CD 전체 파이프라인 구성
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-40"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root -Force | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"
  New-Item -ItemType Directory -Path ".github\workflows" -Force | Out-Null
  New-Item -ItemType Directory -Path ".githooks" -Force | Out-Null

  Write-Host "== 1. CI 워크플로우 ==" -ForegroundColor Cyan
  $ci = @'
name: CI
on:
  push:
    branches: [main]
  pull_request:

jobs:
  quality:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: 시크릿 스캔
        run: powershell -ExecutionPolicy Bypass -File .githooks/scan-secrets.ps1
      - name: 테스트
        run: echo "unit tests passed"
  build:
    runs-on: ubuntu-latest
    needs: quality
    steps:
      - run: mkdir dist && echo "app" > dist/app.js
      - uses: actions/upload-artifact@v4
        with:
          name: build
          path: dist/
'@
  Set-Content ".github\workflows\ci.yml" $ci

  Write-Host "== 2. 배포 워크플로우 ==" -ForegroundColor Cyan
  $deploy = @'
name: Deploy
on:
  push:
    tags: ["v*"]

jobs:
  deploy:
    runs-on: ubuntu-latest
    environment: production
    steps:
      - uses: actions/checkout@v4
      - run: echo "태그 ${{ github.ref_name }} 배포"
'@
  Set-Content ".github\workflows\deploy.yml" $deploy

  Write-Host "== 3. 시크릿 검사 훅 ==" -ForegroundColor Cyan
  $hook = @'
#!/bin/sh
# .githooks/pre-commit — 시크릿 패턴 검사
echo "[pre-commit] 시크릿 검사"
'@
  Set-Content ".githooks\pre-commit" $hook

  Write-Host "== 4. 릴리즈 스크립트 ==" -ForegroundColor Cyan
  $release = @'
param([string]$Kind = "patch")
# SemVer bump + 태그
$tag = git describe --tags --abbrev=0 2>$null
if (-not $tag) { $tag = "0.0.0" }
$parts = $tag.TrimStart('v').Split('.')
$maj=[int]$parts[0]; $min=[int]$parts[1]; $pat=[int]$parts[2]
switch ($Kind) {
  "major" { $new = "$($maj+1).0.0" }
  "minor" { $new = "$maj.$($min+1).0" }
  default { $new = "$maj.$min.$($pat+1)" }
}
git add .; git commit -m "chore: release v$new"
git tag -a "v$new" -m "release v$new"
Write-Host "릴리즈 태그 생성: v$new"
'@
  Set-Content release.ps1 $release

  git add .; git commit -m "CI/CD 파이프라인 구축" | Out-Null
  git config core.hooksPath .githooks

  Write-Host "== 완성된 프로젝트 구조 ==" -ForegroundColor Cyan
  git ls-files | ForEach-Object { Write-Host "  $_" }
  Write-Host ""
  Write-Host "릴리즈 절차:"
  Write-Host "  1. PR 병합 → CI 통과"
  Write-Host "  2. .\release.ps1 -Kind minor  (버전 + 태그)"
  Write-Host "  3. push --tags → Deploy 워크플로우가 태그 기반 배포"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

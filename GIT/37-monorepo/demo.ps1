# 37: 모노레포 데모 — 경로 제한과 영향 분석
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-37"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root -Force | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. 모노레포 구조 생성 ==" -ForegroundColor Cyan
  foreach ($pkg in @('packages/core','packages/ui','apps/web','apps/api')) {
    New-Item -ItemType Directory -Path $pkg -Force | Out-Null
    Set-Content "$pkg\package.json" "{ `"name`": `"$pkg`" }"
  }
  git add .; git commit -m "모노레포 초기 구조" | Out-Null

  Write-Host "  구조:"
  git ls-files | ForEach-Object { Write-Host "    $_" }

  Write-Host ""
  Write-Host "== 2. CODEOWNERS 파일 (경로별 담당팀) ==" -ForegroundColor Cyan
  New-Item -ItemType Directory -Path ".github" -Force | Out-Null
  $codeowners = @"
packages/core/  @team-core
packages/ui/    @team-ui
apps/web/       @team-web
apps/api/       @team-api
"@
  Set-Content ".github\CODEOWNERS" $codeowners
  Write-Host "  변경되는 경로에 따라 리뷰어 자동 지정"

  Write-Host ""
  Write-Host "== 3. 영향 범위 분석 (변경 경로에서 테스트 대상 결정) ==" -ForegroundColor Cyan
  function Get-AffectedPackages([string]$changedPath) {
    $top = ($changedPath -split '/')[0..1] -join '/'
    Write-Host "  변경: $changedPath → 영향 패키지: $top"
  }
  Get-AffectedPackages "packages/core/index.ts"
  Get-AffectedPackages "apps/web/App.tsx"

  Write-Host ""
  Write-Host "== 4. 경로 필터로 CI 최적화 ==" -ForegroundColor Cyan
  $filter = @'
name: CI
on:
  push:
    paths:
      - "packages/**"
      - "apps/**"
      - ".github/**"
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: echo "관련 경로만 CI 실행"
'@
  Set-Content ".github\workflows\ci.yml" $filter
  Write-Host "  paths 필터로 특정 경로 변경 시에만 워크플로우 실행"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

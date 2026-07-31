# 21: 브랜치 전략 데모 — feature branch 기반 협업 시뮬레이션
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-21"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. main 브랜치에 기초 커밋 ==" -ForegroundColor Cyan
  Set-Content main.txt "v1.0"
  git add .; git commit -m "v1.0 초기 커밋" | Out-Null

  Write-Host "== 2. feature/login 브랜치 생성 (기능 개발) ==" -ForegroundColor Cyan
  git checkout -b feature/login
  Set-Content login.txt "로그인 기능"
  git add .; git commit -m "로그인 기능 추가" | Out-Null

  Write-Host "== 3. hotfix 브랜치 (main 기반 긴급 수정) ==" -ForegroundColor Cyan
  git checkout main
  git checkout -b hotfix/security
  Set-Content security.txt "보안 패치"
  git add .; git commit -m "보안 패치 적용" | Out-Null
  git checkout main
  git merge hotfix/security --no-edit | Out-Null

  Write-Host "== 4. feature 병합 ==" -ForegroundColor Cyan
  git merge feature/login --no-edit | Out-Null

  Write-Host "== 5. 브랜치 그래프 확인 ==" -ForegroundColor Cyan
  git log --oneline --graph --all
  Write-Host ""
  Write-Host "== 6. 전략 정리 ==" -ForegroundColor Cyan
  Write-Host "  - main:      배포 가능한 상태"
  Write-Host "  - feature/*: 기능 개발용"
  Write-Host "  - hotfix/*:  긴급 수정용"
  Write-Host "  - trunk-based 대비 GitFlow는 develop 브랜치가 추가됨"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

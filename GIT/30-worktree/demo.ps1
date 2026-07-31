# 30: git worktree 데모 — 여러 브랜치 병렬 작업
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-30"
$wt = Join-Path $root "hotfix-worktree"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"
  Set-Content app.js "console.log('main')"
  git add .; git commit -m "main 초기" | Out-Null

  Write-Host "== 1. main에서 작업 중 ==" -ForegroundColor Cyan
  git branch

  Write-Host ""
  Write-Host "== 2. 워크트리 추가 (다른 디렉터리에서 hotfix 브랜치) ==" -ForegroundColor Cyan
  git worktree add $wt -b hotfix/urgent 2>&1 | Out-String

  Write-Host "== 3. 워크트리 목록 ==" -ForegroundColor Cyan
  git worktree list

  Write-Host ""
  Write-Host "== 4. 메인 워크트리는 main 유지, hotfix에서 수정 ==" -ForegroundColor Cyan
  Set-Content app.js "console.log('main v2')"
  git commit -am "main 진행" | Out-Null

  Set-Location $wt
  Set-Content hotfix.txt "긴급 수정"
  git add .; git commit -m "긴급 수정" | Out-Null

  Write-Host "  main 커밋:" (git log main --oneline | Select-Object -First 1)
  Write-Host "  hotfix 커밋:" (git log hotfix/urgent --oneline | Select-Object -First 1)

  Write-Host ""
  Write-Host "== 5. 워크트리 제거 ==" -ForegroundColor Cyan
  Set-Location $root
  git worktree remove $wt
  git worktree list
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

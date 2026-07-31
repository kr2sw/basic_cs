# 27: 고급 리베이스 데모 — squash, fixup, autosquash
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-27"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. 작업 커밋 여러 개 만들기 ==" -ForegroundColor Cyan
  Set-Content app.js "console.log(1)"
  git add .; git commit -m "feat: 기능 구현" | Out-Null
  Set-Content app.js "console.log(1); console.log(2)"
  git add .; git commit -m "fixup! feat: 기능 구현" | Out-Null
  Set-Content app.js "console.log(1); console.log(2); console.log(3)"
  git add .; git commit -m "feat: 기능 구현" | Out-Null
  git log --oneline

  Write-Host ""
  Write-Host "== 2. 커밋 목록 (fixup이 1번째에 붙어있음) ==" -ForegroundColor Cyan
  # 실제로는 git rebase -i HEAD~2 --autosquash로 fixup! 커밋을 위로 자동 이동

  Write-Host "== 3. squash 동작 원리 (수동) ==" -ForegroundColor Cyan
  git reset --soft HEAD~1   # 마지막 커밋을 스테이징으로 되돌림
  git commit --amend -m "feat: 기능 구현 (통합)" | Out-Null
  git log --oneline
  Write-Host "→ 3개 커밋이 2개로 통합됨"

  Write-Host ""
  Write-Host "== 4. rerere 설정 확인 ==" -ForegroundColor Cyan
  git config --get rerere.enabled
  if (-not (git config --get rerere.enabled)) {
    Write-Host "(설정 안 됨) 아래 명령으로 활성화:"
    Write-Host "  git config --global rerere.enabled true"
  }

  Write-Host ""
  Write-Host "== 5. autosquash 요약 ==" -ForegroundColor Cyan
  Write-Host "  git commit --fixup=<sha>   → 자동 fixup! 커밋 생성"
  Write-Host "  git rebase -i --autosquash  → fixup을 원래 커밋에 자동 병합"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

# 34: 팀 워크플로 데모 — 보호된 브랜치와 PR 흐름
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-34"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. main 브랜치 보호 규칙 예시 (GitHub 설정) ==" -ForegroundColor Cyan
  $rules = @"
- Require a pull request before merging (승인 1명)
- Require status checks to pass (CI 필수)
- Require conversation resolution (댓글 해결 필수)
- Restrict force pushes
"@
  Write-Host $rules

  Write-Host ""
  Write-Host "== 2. 로컬에서 PR 흐름 시뮬레이션 ==" -ForegroundColor Cyan
  Set-Content app.js "console.log('v1')"
  git add .; git commit -m "feat: v1" | Out-Null

  git checkout -b feature/statistics
  Set-Content stats.js "export const avg = (a) => a.length ? a.reduce((x,y)=>x+y)/a.length : 0"
  git add .; git commit -m "feat: 평균 통계 추가" | Out-Null

  Write-Host "  PR 체크리스트:"
  Write-Host "   [ ] CI (test) 통과"
  Write-Host "   [ ] 리뷰어 1명 승인"
  Write-Host "   [ ] 커밋 메시지 컨벤션 준수 (feat:/fix:)"
  Write-Host "   [ ] 변경 범위 200줄 이하"

  Write-Host ""
  Write-Host "== 3. 리뷰 반영 (squash merge 가정) ==" -ForegroundColor Cyan
  git checkout main
  git merge feature/statistics --squash | Out-Null
  git commit -m "feat: 평균 통계 추가 (#42)" | Out-Null
  git log --oneline
  Write-Host "  → 팀 단위로 squash merge하면 main 히스토리가 깔끔함"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

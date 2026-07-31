# 31: 히스토리 편집 데모 — 시크릿 제거와 재작성
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-31"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. 실수로 시크릿을 커밋한 상황 재현 ==" -ForegroundColor Cyan
  Set-Content config.js "const KEY = 'supersecret123'"
  git add .; git commit -m "설정 추가" | Out-Null
  Set-Content config.js "const KEY = process.env.KEY"
  git add .; git commit -m "환경변수로 교체" | Out-Null

  Write-Host "== 2. 현재 히스토리에는 시크릿이 남아있음 ==" -ForegroundColor Cyan
  git log --oneline
  Write-Host "  과거 커밋 내용:"
  git show HEAD~1:config.js

  Write-Host ""
  Write-Host "== 3. filter-branch로 파일 내용에서 시크릿 제거 (레거시 방식) ==" -ForegroundColor Cyan
  git filter-branch -f --tree-filter "if (Test-Path config.js) { (Get-Content config.js -Raw) -replace 'supersecret123','REMOVED' | Set-Content config.js }" HEAD 2>&1 | Out-String

  Write-Host "  수정 후 과거 커밋 확인:"
  git show HEAD~1:config.js

  Write-Host ""
  Write-Host "== 4. 경고 ==" -ForegroundColor Yellow
  Write-Host "  - 히스토리 재작성은 해시를 바꾸므로 원격 push -f 또는 저장소 재생성 필요"
  Write-Host "  - git filter-repo (권장) / BFG Repo-Cleaner 도구도 사용 가능"
  Write-Host "  - 반드시 시크릿을 revoke 후 공유"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

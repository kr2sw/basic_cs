# 38: Git 성능 데모 — shallow clone, gc, 저장소 크기
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-38"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. 커밋 50개 생성 ==" -ForegroundColor Cyan
  foreach ($i in 1..50) {
    Set-Content file.txt "내용 $i"
    git add .; git commit -m "커밋 $i" | Out-Null
  }
  Write-Host "  전체 커밋 수: $((git rev-list --count HEAD))"

  Write-Host ""
  Write-Host "== 2. shallow clone 개념 (--depth) ==" -ForegroundColor Cyan
  Write-Host "  git clone --depth 1 <url>  → 최근 1개만"
  Write-Host "  git fetch --unshallow      → 전체 이력으로 확장"

  Write-Host ""
  Write-Host "== 3. .git 크기와 gc ==" -ForegroundColor Cyan
  $before = (Get-ChildItem .git -Recurse -File | Measure-Object Length -Sum).Sum / 1KB
  Write-Host ("  .git 크기: {0:N1} KB" -f $before)
  git gc --aggressive 2>&1 | Out-String
  $after = (Get-ChildItem .git -Recurse -File | Measure-Object Length -Sum).Sum / 1KB
  Write-Host ("  gc 후: {0:N1} KB" -f $after)

  Write-Host ""
  Write-Host "== 4. LFS / partial clone 권장 사례 ==" -ForegroundColor Cyan
  Write-Host "  - 100MB+ 바이너리: Git LFS (git lfs track '*.bin')"
  Write-Host "  - blob만 늦게 받기: git clone --filter=blob:none"
  Write-Host "  - CI용: git clone --depth 1"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

# 26: Git 내부 구조 데모 — objects, refs, HEAD
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-26"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. blob 객체 생성 (hash-object) ==" -ForegroundColor Cyan
  $hash = echo "hello git" | git hash-object --stdin
  Write-Host "blob 해시: $hash"
  Write-Host "해시 값은 내용 기반 → 같은 내용이면 같은 해시"

  Write-Host ""
  Write-Host "== 2. 커밋 후 .git 구조 확인 ==" -ForegroundColor Cyan
  Set-Content hello.txt "hello git"
  git add .; git commit -m "첫 커밋" | Out-Null

  Write-Host "  .git/refs/heads/main:"
  git rev-parse main
  Write-Host "  .git/HEAD:"
  Get-Content ".git\HEAD"

  Write-Host ""
  Write-Host "== 3. 객체 종류 확인 (cat-file -t) ==" -ForegroundColor Cyan
  $commitSha = git rev-parse main
  git cat-file -t $commitSha
  $treeSha = git rev-parse "$commitSha^{tree}"
  Write-Host "tree: $treeSha"
  git cat-file -t $treeSha
  $blobSha = git ls-tree $treeSha | ForEach-Object { ($_ -split '\s+')[2] }
  git cat-file -t $blobSha

  Write-Host ""
  Write-Host "== 4. tree 내용 ==" -ForegroundColor Cyan
  git ls-tree -r $commitSha

  Write-Host ""
  Write-Host "== 5. commit 객체 내용 ==" -ForegroundColor Cyan
  git cat-file -p $commitSha
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

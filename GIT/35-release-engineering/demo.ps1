# 35: 릴리즈 엔지니어링 데모 — SemVer와 태그
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-35"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. SemVer 계산 함수 ==" -ForegroundColor Cyan
  function Bump-Version([string]$version, [string]$kind) {
    $parts = $version.Split('.')
    $maj = [int]$parts[0]; $min = [int]$parts[1]; $pat = [int]$parts[2]
    switch ($kind) {
      "major" { return "$($maj+1).0.0" }
      "minor" { return "$maj.$($min+1).0" }
      default { return "$maj.$min.$($pat+1)" }
    }
  }

  Write-Host "  1.2.3 + minor → $(Bump-Version '1.2.3' 'minor')"
  Write-Host "  1.2.3 + major → $(Bump-Version '1.2.3' 'major')"
  Write-Host "  1.2.3 + patch → $(Bump-Version '1.2.3' 'patch')"

  Write-Host ""
  Write-Host "== 2. 릴리즈 커밋 + 태그 ==" -ForegroundColor Cyan
  Set-Content app.js "console.log('1.0.0')"
  git add .; git commit -m "chore: release 1.0.0" | Out-Null
  git tag -a v1.0.0 -m "첫 공개 릴리즈"
  Write-Host "  태그 생성: v1.0.0"

  Set-Content app.js "console.log('1.1.0')"
  git add .; git commit -m "feat: 통계 기능" | Out-Null
  $v = Bump-Version '1.0.0' 'minor'
  git tag -a "v$v" -m "기능 추가 릴리즈"
  Write-Host "  태그 생성: v$v"

  Write-Host ""
  Write-Host "== 3. 태그 목록과 내용 ==" -ForegroundColor Cyan
  git tag -l
  Write-Host "  태그 정보 (v1.1.0):"
  git show v1.1.0 --stat --format="%H %s"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

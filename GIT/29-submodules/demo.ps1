# 29: 서브모듈 데모 — 로컬 저장소 두 개로 실습
$ErrorActionPreference = 'Stop'
$base = Join-Path $env:TEMP "git-intermediate-29"
if (Test-Path $base) { Remove-Item $base -Recurse -Force }
New-Item -ItemType Directory -Path $base | Out-Null

try {
  # 공용 라이브러리 저장소
  $lib = Join-Path $base "shared-lib"
  New-Item -ItemType Directory -Path $lib | Out-Null
  Set-Location $lib
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"
  Set-Content utils.js "export const greet = (n) => `Hello ${n}`"
  git add .; git commit -m "라이브러리 초기화" | Out-Null

  # 앱 저장소
  $app = Join-Path $base "my-app"
  New-Item -ItemType Directory -Path $app | Out-Null
  Set-Location $app
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"
  Set-Content package.json "{}"
  git add .; git commit -m "앱 초기화" | Out-Null

  Write-Host "== 1. 서브모듈 추가 ==" -ForegroundColor Cyan
  git submodule add $lib libs/shared 2>&1 | Out-String
  git commit -am "shared 라이브러리 서브모듈 추가" | Out-Null

  Write-Host "== 2. .gitmodules 파일 확인 ==" -ForegroundColor Cyan
  Get-Content ".gitmodules"
  Write-Host ""
  Write-Host "== 3. 서브모듈 상태 ==" -ForegroundColor Cyan
  git submodule status
  Write-Host ""
  Write-Host "== 4. 자식 저장소의 커밋이 부모에 기록됨 ==" -ForegroundColor Cyan
  git ls-tree HEAD libs/shared
  Write-Host "→ 서브모듈은 특정 커밋을 가리키는 포인터"
  Write-Host ""
  Write-Host "== 요약 ==" -ForegroundColor Cyan
  Write-Host "  클론 후: git submodule update --init --recursive"
  Write-Host "  업데이트: git submodule foreach git pull"
  Write-Host "  서브트리는 파일을 직접 복사하는 대안"
} finally {
  Set-Location $env:TEMP
  Remove-Item $base -Recurse -Force
}

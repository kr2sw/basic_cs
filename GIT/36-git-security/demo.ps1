# 36: Git 보안 데모 — 시크릿 보호와 서명 커밋
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-36"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. .gitignore로 시크릿 사전 차단 ==" -ForegroundColor Cyan
  $gitignore = @"
# 환경 변수 / 시크릿 파일
.env
.env.*
*.pem
credentials.json
config/tokens.js
"@
  Set-Content .gitignore $gitignore
  Set-Content .env "API_KEY=super-secret"          # 무시 대상
  Set-Content app.js "console.log('ok')"           # 추적 대상
  git add .; git commit -m "초기 설정" | Out-Null

  Write-Host "  추적 중인 파일:"
  git ls-files
  Write-Host "  (.env는 .gitignore로 제외됨)"

  Write-Host ""
  Write-Host "== 2. 시크릿 검사 스크립트 (pre-commit 개념) ==" -ForegroundColor Cyan
  $scan = @'
# 시크릿 패턴 검사
$patterns = @('API_KEY\s*=\s*["'']?[A-Za-z0-9]{10,}', 'password\s*[:=]\s*["'']')
foreach ($file in (git diff --cached --name-only)) {
  if (Test-Path $file) {
    foreach ($line in Get-Content $file) {
      foreach ($pat in $patterns) {
        if ($line -match $pat) {
          Write-Error "시크릿 의심 패턴 발견: $file ($pat)"
          exit 1
        }
      }
    }
  }
}
Write-Host "검사 통과"
'@
  Set-Content scan-secrets.ps1 $scan
  Write-Host "  - pre-commit 훅 또는 CI에서 scan-secrets.ps1 실행"
  Write-Host "  - GitHub Secret Scanning은 커밋된 시크릿 자동 감지"

  Write-Host ""
  Write-Host "== 3. 서명 커밋 설정 확인 ==" -ForegroundColor Cyan
  $signing = git config --get user.signingkey
  if ($signing) { Write-Host "  GPG 키: $signing" } else {
    Write-Host "  GPG 키 미설정. 생성/등록 후:"
    Write-Host "    git config user.signingkey <KEY>"
    Write-Host "    git config commit.gpgsign true"
    Write-Host "    git commit -S -m \"서명 커밋\""
  }
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

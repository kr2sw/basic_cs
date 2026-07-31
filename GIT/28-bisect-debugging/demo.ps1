# 28: git bisect 데모 — 버그 유발 커밋 찾기
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-28"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init -b main | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. 10개 커밋 생성 (5번째에 버그 도입) ==" -ForegroundColor Cyan
  foreach ($i in 1..10) {
    Set-Content data.txt "value=$($i - 3)"   # value = i-3
    git add .; git commit -m "커밋 $i" | Out-Null
  }
  # 버그: value가 음수인 시점(i<3, 즉 커밋 1~2)과 커밋5에서 "bad" 마커
  git checkout -q HEAD~6   # 커밋 4 상태
  Set-Content bugmark.txt "ok"
  git add .; git commit -m "커밋 4 이후 버그 예비" | Out-Null

  Write-Host "== 2. 테스트 스크립트 정의 ==" -ForegroundColor Cyan
  # test.ps1: data.txt의 value가 5이면 통과(0), 아니면 실패(1)
  $test = @'
$ErrorActionPreference = 'Stop'
$v = (Get-Content data.txt -Raw).Trim()
if ($v -eq "value=5") { exit 0 } else { exit 1 }
'@
  Set-Content test.ps1 $test

  Write-Host "== 3. bisect 시작 ==" -ForegroundColor Cyan
  git bisect start
  git bisect bad        # 현재(최신)는 실패
  git bisect good HEAD~7  # 오래된 커밋은 정상
  Write-Host "중간 커밋 자동 판정 진행..."

  # bisect run으로 자동 판정
  git bisect run powershell -ExecutionPolicy Bypass -File test.ps1 | Out-String | Write-Host
  Write-Host "→ 버그를 처음 도입한 커밋을 찾았습니다"
  git bisect log
  git bisect reset | Out-Null
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

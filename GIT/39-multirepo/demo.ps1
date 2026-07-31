# 39: 멀티레포 데모 — 저장소 간 자동화 패턴
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-39"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root -Force | Out-Null

try {
  Set-Location $root
  git config --global user.email "demo@example.com"
  git config --global user.name "Demo"

  Write-Host "== 1. 저장소 3개 생성 (service-a, service-b, shared) ==" -ForegroundColor Cyan
  foreach ($repo in @('service-a', 'service-b', 'shared')) {
    New-Item -ItemType Directory -Path $repo | Out-Null
    Set-Location $repo
    git init -b main | Out-Null
    Set-Content package.json "{}"
    git add .; git commit -m "init $repo" | Out-Null
    Set-Location $root
  }

  Write-Host "== 2. 공유 패키지 버전과 의존 ==" -ForegroundColor Cyan
  Set-Location shared
  git tag v1.0.0 | Out-Null
  Write-Host "  shared에 v1.0.0 태그 → npm publish 후 각 서비스가 의존"

  Write-Host ""
  Write-Host "== 3. repository_dispatch 이벤트 트리거 스크립트 ==" -ForegroundColor Cyan
  $dispatch = @'
# service-a → service-b 빌드 트리거
$repo = "owner/service-b"
$event = "build-requested"
Write-Host "gh api repos/$repo/dispatches -f event_type=$event -F client_payload[ref]=main"
Write-Host "  → service-b의 repository_dispatch 워크플로우가 실행됨"
'@
  Set-Content dispatch.ps1 $dispatch
  Write-Host "  - GITHUB_TOKEN: 저장소 내 자동 인증"
  Write-Host "  - PAT: 저장소 간 dispatch에 필요 (최소 범위)"

  Write-Host ""
  Write-Host "== 4. 멀티레포 운영 요약 ==" -ForegroundColor Cyan
  Write-Host "  - 공통 코드: 별도 패키지로 버전 관리"
  Write-Host "  - 변경 전파: package 버전 업데이트 + dependabot"
  Write-Host "  - CI: 저장소별 독립 + dispatch로 연쇄 실행"
} finally {
  git config --global --unset user.email
  git config --global --unset user.name
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

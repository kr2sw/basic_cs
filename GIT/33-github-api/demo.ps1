# 33: GitHub API / gh CLI 데모 — 자동화 스크립트 패턴
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-33"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. gh CLI 사용 가능 여부 확인 ==" -ForegroundColor Cyan
  $gh = Get-Command gh -ErrorAction SilentlyContinue
  if ($gh) {
    Write-Host "  gh CLI 설치됨: $($gh.Source)"
    Write-Host "  주요 명령: gh repo view, gh pr create, gh issue list"
  } else {
    Write-Host "  gh CLI 미설치 (다운로드: https://cli.github.com)"
  }

  Write-Host ""
  Write-Host "== 2. REST API 호출 패턴 (Invoke-RestMethod) ==" -ForegroundColor Cyan
  $script = @'
$headers = @{ "Accept" = "application/vnd.github+json" }
# 실제 사용 시: $headers.Authorization = "token $env:GITHUB_TOKEN"
# 예: 최근 이슈 조회
# Invoke-RestMethod -Headers $headers "https://api.github.com/repos/octocat/Hello-World/issues"
Write-Host "REST API 예제 준비됨"
'@
  Set-Content github-api-example.ps1 $script

  Write-Host "== 3. 자동화 스크립트 템플릿 ==" -ForegroundColor Cyan
  $template = @'
# 일일 PR 요약 스크립트
param([string]$Repo)
if (-not $Repo) { Write-Error "사용법: .\pr-summary.ps1 -Repo owner/repo"; exit 1 }

# gh CLI 사용
$prs = gh pr list --repo $Repo --state open --json number,title,author --limit 10
$prs | ConvertFrom-Json | ForEach-Object {
  Write-Host "#$($_.number) $($_.title) by $($_.author.login)"
}
'@
  Set-Content pr-summary.ps1 $template
  Write-Host "  - gh pr list 로 열린 PR 요약"
  Write-Host "  - GITHUB_TOKEN 환경변수로 REST API 인증"
  Write-Host "  - Actions에서 스케줄 트리거로 일일 자동 실행 가능"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

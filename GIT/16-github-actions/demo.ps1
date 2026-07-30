# 16: GitHub Actions 데모
$demo = "$env:TEMP\git-demo-16"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir "$demo\.github\workflows" -Force | Out-Null
Set-Location $demo

# .NET CI 워크플로우 생성
@"
name: .NET CI

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0'
      - run: dotnet restore
      - run: dotnet build --no-restore
      - run: dotnet test --no-build --verbosity normal
"@ > .github\workflows\ci.yml

Write-Host "=== .github/workflows/ci.yml ===" -ForegroundColor Cyan
Get-Content .github\workflows\ci.yml

Write-Host "`nGitHub Actions 주요 기능:" -ForegroundColor Cyan
Write-Host "- on: push / pull_request / schedule (cron)" -ForegroundColor White
Write-Host "- jobs: 병렬/순차 실행" -ForegroundColor White
Write-Host "- steps: checkout → setup → build → test" -ForegroundColor White
Write-Host "- matrix: 여러 OS/버전 동시 테스트" -ForegroundColor White
Write-Host "- cache: 의존성 캐싱" -ForegroundColor White
Write-Host "- secrets: \${{ secrets.MY_SECRET }}" -ForegroundColor White

Write-Host "`n사용법: .github/workflows/*.yml 파일을 저장소에 push" -ForegroundColor Yellow

Write-Host "Done!" -ForegroundColor Green

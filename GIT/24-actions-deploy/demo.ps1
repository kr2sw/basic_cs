# 24: Actions 배포(environments, secrets) 데모
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-24"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root -Force | Out-Null

try {
  Set-Location $root
  git init | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"
  New-Item -ItemType Directory -Path ".github\workflows" -Force | Out-Null

  Write-Host "== 스테이징/프로덕션 배포 워크플로우 ==" -ForegroundColor Cyan
  $deploy = @'
name: Deploy
on:
  push:
    branches: [main]

jobs:
  deploy-staging:
    runs-on: ubuntu-latest
    environment: staging
    steps:
      - uses: actions/checkout@v4
      - run: echo "스테이징 배포 (자동)"
      - run: echo "시크릿 사용: ${{ secrets.STAGING_TOKEN }}"  # 실제로는 로그 출력 금지

  deploy-production:
    runs-on: ubuntu-latest
    needs: deploy-staging
    environment:
      name: production
      url: https://app.example.com
    steps:
      - uses: actions/checkout@v4
      - run: echo "프로덕션 배포 (승인 대기)"
      - run: echo "운영 시크릿: ${{ secrets.PROD_TOKEN }}"
'@
  Set-Content ".github\workflows\deploy.yml" $deploy
  git add .; git commit -m "배포 워크플로우 추가" | Out-Null

  Write-Host ""
  Write-Host "== 보안 규칙 ==" -ForegroundColor Cyan
  Write-Host " 1. secrets는 Settings > Secrets and variables에 등록"
  Write-Host " 2. 프로덕션 환경에 'Required reviewers' 설정 시 승인 필요"
  Write-Host " 3. 로그에 시크릿을 echo하지 않는다 (마스킹됨)"
  Write-Host ""
  Write-Host "== 요약 ==" -ForegroundColor Cyan
  Write-Host "  staging:      push 시 자동 배포"
  Write-Host "  production:   staging 성공 후 승인하면 배포"
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

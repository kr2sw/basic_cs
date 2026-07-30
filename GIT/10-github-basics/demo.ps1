# 10: GitHub 기초 데모
# 참고: GitHub 계정과 네트워크 연결 필요

$repoName = "git-demo-$(Get-Random -Maximum 9999)"
$demoDir = "$env:TEMP\$repoName"

Write-Host "GitHub 저장소 만들기:" -ForegroundColor Cyan
Write-Host "1. github.com 로그인 → New repository" -ForegroundColor White
Write-Host "2. 저장소 이름: $repoName" -ForegroundColor White
Write-Host "3. Public 선택, README 없음" -ForegroundColor White

# 로컬 저장소
mkdir $demoDir | Out-Null
Set-Location $demoDir
git init
"# $repoName" > README.md
git add .; git commit -m "Initial commit"

Write-Host "`n로컬 → GitHub 연결:" -ForegroundColor Cyan
Write-Host "git remote add origin https://github.com/YOUR_USER/$repoName.git" -ForegroundColor White
Write-Host "git push -u origin main" -ForegroundColor White

Write-Host "`nFork 워크플로:" -ForegroundColor Cyan
Write-Host "1. 대상 저장소 Fork (GitHub UI)" -ForegroundColor White
Write-Host "2. Fork한 저장소 clone" -ForegroundColor White
Write-Host "3. git remote add upstream https://github.com/ORIGINAL/repo.git" -ForegroundColor White
Write-Host "4. 작업 후 PR 생성" -ForegroundColor White

Write-Host "`nDone!" -ForegroundColor Green

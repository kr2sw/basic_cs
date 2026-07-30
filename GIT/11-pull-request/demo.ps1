# 11: Pull Request 데모
$demo = "$env:TEMP\git-demo-11"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

# main 브랜치
"# Project" > README.md
git add .; git commit -m "Initial commit"

# PR을 위한 브랜치 작업
git switch -c feature/new-feature

@"
## New Feature
This is a new feature.
"@ >> README.md
git add .
git commit -m "feat: add new feature description"

# 로그 확인
git log --oneline --graph --all

Write-Host "`nPR 워크플로:" -ForegroundColor Cyan
Write-Host "1. git push -u origin feature/new-feature (원격 필요)" -ForegroundColor White
Write-Host "2. GitHub에서 PR 생성 (base: main ← compare: feature/new-feature)" -ForegroundColor White
Write-Host "3. 리뷰어 지정, 설명 작성" -ForegroundColor White
Write-Host "4. PR 머지 (merge commit / squash / rebase)" -ForegroundColor White

Write-Host "`nPR 템플릿 (PR 생성 시 자동 적용):" -ForegroundColor Cyan
Write-Host ".github/PULL_REQUEST_TEMPLATE.md 파일 생성" -ForegroundColor White

Write-Host "Done!" -ForegroundColor Green

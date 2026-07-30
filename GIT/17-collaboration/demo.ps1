# 17: 협업 워크플로우 데모
$demo = "$env:TEMP\git-demo-17"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

# GitHub Flow 시뮬레이션
"# Project" > README.md
git add .; git commit -m "Initial commit"

# 기능 개발
git switch -c feature/user-auth
"user auth module" > auth.txt
git add .; git commit -m "feat: add user authentication"
git log --oneline --graph --all

Write-Host "=== GitHub Flow ===" -ForegroundColor Cyan
Write-Host "1. main에서 브랜치 생성" -ForegroundColor White
Write-Host "2. 커밋" -ForegroundColor White
Write-Host "3. PR 생성" -ForegroundColor White
Write-Host "4. 리뷰" -ForegroundColor White
Write-Host "5. main에 merge" -ForegroundColor White
Write-Host "6. 브랜치 삭제" -ForegroundColor White

Write-Host "`n=== GitFlow ===" -ForegroundColor Cyan
Write-Host "master ─── develop ─── feature" -ForegroundColor White
Write-Host "                       └── release" -ForegroundColor White
Write-Host "                       └── hotfix" -ForegroundColor White

Write-Host "`n=== Conventional Commits ===" -ForegroundColor Cyan
Write-Host "feat:     새로운 기능" -ForegroundColor White
Write-Host "fix:      버그 수정" -ForegroundColor White
Write-Host "docs:     문서 변경" -ForegroundColor White
Write-Host "refactor: 리팩토링" -ForegroundColor White
Write-Host "test:     테스트 추가/수정" -ForegroundColor White
Write-Host "chore:    기타 (빌드, 의존성 등)" -ForegroundColor White

Write-Host "Done!" -ForegroundColor Green

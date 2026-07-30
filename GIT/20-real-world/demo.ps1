# 20: 실전 프로젝트 — 전체 워크플로
$demo = "$env:TEMP\git-demo-20"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

# ============================================
# 시나리오: 오픈소스 저장소에 기여하기
# ============================================

# 1. 원본 저장소 (upstream)
mkdir "$demo\upstream" | Out-Null
Set-Location "$demo\upstream"
git init --bare

# 2. Fork한 내 저장소 (origin)
Set-Location $demo
git clone "$demo\upstream" origin
Set-Location "$demo\origin"
"# Awesome Project" > README.md
git add .; git commit -m "Initial commit"
git push

# 3. 로컬 clone
Set-Location $demo
git clone "$demo\origin" local
Set-Location "$demo\local"

# 4. upstream 추가
git remote add upstream "$demo\upstream"

# 5. 기능 브랜치
git switch -c feature/improve-readme

# 6. 작업
@"
# Awesome Project

## Getting Started
Instructions here.

## Contributing
PRs welcome!
"@ > README.md

git add .
git commit -m "feat: improve README with contributing guide"

# 7. 최신 upstream 반영 (rebase)
git fetch upstream
try {
    git rebase upstream/main
} catch {
    Write-Host "Rebase conflict (expected in demo)" -ForegroundColor Yellow
}

# 8. PR 생성 (시뮬레이션)
git push -u origin feature/improve-readme

Write-Host "=== 최종 상태 ===" -ForegroundColor Cyan
git log --oneline --graph --all

Write-Host "`n=== 전체 워크플로 요약 ===" -ForegroundColor Green
Write-Host "1. Fork (GitHub UI)" -ForegroundColor White
Write-Host "2. git clone 내-저장소" -ForegroundColor White
Write-Host "3. git remote add upstream 원본-저장소" -ForegroundColor White
Write-Host "4. git switch -c feature/my-feature" -ForegroundColor White
Write-Host "5. 작업 → 커밋" -ForegroundColor White
Write-Host "6. git fetch upstream + git rebase upstream/main" -ForegroundColor White
Write-Host "7. git push -u origin feature/my-feature" -ForegroundColor White
Write-Host "8. GitHub에서 PR 생성" -ForegroundColor White
Write-Host "9. 리뷰 반영 → push (자동 반영)" -ForegroundColor White
Write-Host "10. PR 머지 → 로컬 브랜치 삭제" -ForegroundColor White

Write-Host "`nGitHub CLI (gh):" -ForegroundColor Cyan
Write-Host "gh repo create my-project --public" -ForegroundColor White
Write-Host "gh pr create --base main --title 'PR' --body 'desc'" -ForegroundColor White
Write-Host "gh pr merge --squash" -ForegroundColor White

Write-Host "`nDone!" -ForegroundColor Green

# 18: 고급 기능 데모

# Submodule 데모
$demo = "$env:TEMP\git-demo-18"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir "$demo\main" -Force | Out-Null
mkdir "$demo\lib" -Force | Out-Null

# 라이브러리 저장소
Set-Location "$demo\lib"
git init
"# Library" > README.md; git add .; git commit -m "Initial lib"
"function doSomething() { }" > lib.js; git add .; git commit -m "Add lib"

# 메인 프로젝트 + 서브모듈
Set-Location "$demo\main"
git init
"# Main Project" > README.md; git add .; git commit -m "Initial main"
git submodule add "$demo\lib" libs/my-lib
git status

# Worktree 데모
git worktree add "$demo\hotfix-worktree" -b hotfix/critical
git worktree list

# Bisect 시뮬레이션
Write-Host "=== Bisect ===" -ForegroundColor Cyan
Write-Host "git bisect start" -ForegroundColor White
Write-Host "git bisect bad HEAD     # 현재 버그 있음" -ForegroundColor White
Write-Host "git bisect good v1.0    # 예전에는 정상" -ForegroundColor White
Write-Host "# 중간 커밋 테스트 후:" -ForegroundColor Gray
Write-Host "git bisect good         # 아직 정상" -ForegroundColor White
Write-Host "git bisect bad          # 버그 발생" -ForegroundColor White
Write-Host "# 반복 → 첫 버그 커밋 발견" -ForegroundColor Green
Write-Host "git bisect reset        # 종료" -ForegroundColor White

Write-Host "`n=== Reflog ===" -ForegroundColor Cyan
Write-Host "git reflog              # 모든 HEAD 이동 기록" -ForegroundColor White
Write-Host "git reset --hard HEAD@{2}  # 실수 복구" -ForegroundColor White

Write-Host "Done!" -ForegroundColor Green

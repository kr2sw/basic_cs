# 13: Stashing 데모
$demo = "$env:TEMP\git-demo-13"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

"base" > file.txt; git add .; git commit -m "Initial"

# 작업 중
"work in progress" >> file.txt
Write-Host "=== Before stash ===" -ForegroundColor Cyan
git status -s

# 임시 저장
git stash push -m "WIP: feature in progress"
Write-Host "`n=== After stash ===" -ForegroundColor Cyan
git status -s

# 다른 작업
"hotfix" > hotfix.txt; git add .; git commit -m "Hotfix"

# stash 복원
git stash pop
Write-Host "`n=== After stash pop ===" -ForegroundColor Cyan
git status -s
Get-Content file.txt

# 여러 stash
"wip2" >> file.txt; git stash push -m "WIP2"
"wip3" >> file.txt; git stash push -m "WIP3"
Write-Host "`n=== Stash List ===" -ForegroundColor Cyan
git stash list

Write-Host "Done!" -ForegroundColor Green

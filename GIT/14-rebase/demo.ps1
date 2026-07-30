# 14: Rebase 데모
$demo = "$env:TEMP\git-demo-14"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

"base" > file.txt; git add .; git commit -m "Base commit"

# feature 브랜치
git switch -c feature
"feature 1" >> file.txt; git add .; git commit -m "Feature step 1"
"feature 2" >> file.txt; git add .; git commit -m "Feature step 2"

# main에 새 커밋
git switch main
"main update" >> file.txt; git add .; git commit -m "Main update"

# rebase
git switch feature
Write-Host "=== Before Rebase ===" -ForegroundColor Cyan
git log --oneline --graph --all

git rebase main
Write-Host "`n=== After Rebase ===" -ForegroundColor Cyan
git log --oneline --graph --all

Write-Host "`n=== Interactive Rebase ===" -ForegroundColor Cyan
Write-Host "git rebase -i HEAD~3" -ForegroundColor White
Write-Host "  pick     = keep" -ForegroundColor Gray
Write-Host "  reword   = change message" -ForegroundColor Gray
Write-Host "  squash   = combine with previous" -ForegroundColor Gray
Write-Host "  fixup    = squash, discard message" -ForegroundColor Gray
Write-Host "  edit     = stop to amend" -ForegroundColor Gray
Write-Host "  drop     = remove commit" -ForegroundColor Gray

Write-Host "`nDone!" -ForegroundColor Green

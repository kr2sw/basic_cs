# 05: 되돌리기 데모
$demo = "$env:TEMP\git-demo-05"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

"v1" > file.txt
git add .; git commit -m "First"

# restore (WD 변경 취소)
"v2" > file.txt
git restore file.txt
Get-Content file.txt  # v1

# restore --staged (staging 취소)
"v3" > file.txt
git add file.txt
git restore --staged file.txt
git status -s  # modified, not staged

# commit --amend
git add file.txt
git commit -m "v3"
git commit --amend -m "Version 3 (corrected)"
git log --oneline  # 커밋이 수정됨

# reset --soft
"v4" > file.txt; git add .; git commit -m "v4"
"v5" > file2.txt; git add .; git commit -m "v5"
git reset --soft HEAD~1  # v5 취소, staging에 남음
git status -s

# revert
git revert HEAD --no-edit
git log --oneline

Write-Host "Done!" -ForegroundColor Green

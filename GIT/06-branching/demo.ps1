# 06: 브랜치 데모
$demo = "$env:TEMP\git-demo-06"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

"main" > file.txt; git add .; git commit -m "Initial"

# 브랜치 생성 및 이동
git branch feature
git switch feature
"feature work" >> file.txt
git add .; git commit -m "Feature work"

# main으로 돌아가기
git switch main
Get-Content file.txt  # feature 변경사항 없음

# 브랜치 목록
git branch -a

# switch -c (생성 + 이동)
git switch -c hotfix
"hotfix" > hotfix.txt; git add .; git commit -m "Hotfix"
git switch main

git log --oneline --graph --all
Write-Host "Done!" -ForegroundColor Green

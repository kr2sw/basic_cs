# 07: 병합 데모
$demo = "$env:TEMP\git-demo-07"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

# 공통 커밋
"base" > file.txt; git add .; git commit -m "Base"

# feature 브랜치
git switch -c feature
"feature work" > feature.txt
git add .; git commit -m "Add feature"

# main에 merge
git switch main
git merge feature  # fast-forward
git log --oneline --graph

# 새로운 브랜치로 3-way merge 테스트
git switch -c feature2
"work2" >> file.txt; git add .; git commit -m "Feature 2 work"

git switch main
"main work" >> file.txt; git add .; git commit -m "Main work"

git merge feature2  # 3-way merge
git log --oneline --graph

Write-Host "Done!" -ForegroundColor Green

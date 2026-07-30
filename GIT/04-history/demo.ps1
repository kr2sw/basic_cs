# 04: 변경 이력 데모
$demo = "$env:TEMP\git-demo-04"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

# 여러 커밋 생성
"Initial" > README.md; git add .; git commit -m "Initial commit"
"Feature A" > a.txt; git add .; git commit -m "Add feature A"
"Feature B" > b.txt; git add .; git commit -m "Add feature B"
"Fix bug" | Set-Content a.txt; git add .; git commit -m "Fix bug in A"

# 로그 보기
git log --oneline
git log --oneline --graph
git log --oneline --since="2024-01-01"

# diff
git diff HEAD~2..HEAD
git show HEAD

# blame
git blame a.txt

Write-Host "Done!" -ForegroundColor Green

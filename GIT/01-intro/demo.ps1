# 01: Git 저장소 만들기 데모
$demo = "$env:TEMP\git-demo-01"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null
Set-Location $demo

git init
"# My Project" > README.md
git add README.md
git commit -m "Initial commit"
echo "file1.txt" > file1.txt
git add .
git commit -m "Add file1"

git log --oneline
Write-Host "`nDemo complete! Check $demo" -ForegroundColor Green

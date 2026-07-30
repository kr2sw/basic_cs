# 03: 파일 관리 데모
$demo = "$env:TEMP\git-demo-03"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

# .gitignore 생성
@"
*.log
build/
.DS_Store
"@ > .gitignore
git add .gitignore
git commit -m "Add .gitignore"

# 테스트
"debug" > debug.log
mkdir build | Out-Null
"binary" > build/output.exe
git status -s  # .gitignore 파일은 표시 안 됨

# 파일 삭제/이동
"content" > app.txt
git add app.txt
git commit -m "Add app.txt"
git rm app.txt
git commit -m "Remove app.txt"

Write-Host "Done!" -ForegroundColor Green

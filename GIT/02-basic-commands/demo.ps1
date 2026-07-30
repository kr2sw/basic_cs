# 02: 기본 명령어 데모
$demo = "$env:TEMP\git-demo-02"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

# 파일 생성
"line 1" > file.txt
"line A" > another.txt

# 상태 확인
git status -s

# staging
git add file.txt
git status -s

# diff
"line 2" >> file.txt
git diff

# commit
git commit -m "Add file.txt"
git add another.txt
git commit -m "Add another.txt"

git log --oneline
Write-Host "`nDone!" -ForegroundColor Green

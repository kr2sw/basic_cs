# 19: .gitignore와 보안 데모
$demo = "$env:TEMP\git-demo-19"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

# .gitignore 생성
@"
# 보안/민감 파일
secrets.json
.env
*.key
*.pem
config.local.*

# 빌드 결과
bin/
obj/
*.exe
*.dll

# OS 파일
.DS_Store
Thumbs.db

# IDE
.vs/
.vscode/
*.sublime-project

# 로그
*.log

# 예외
!important.log
"@ > .gitignore

git add .gitignore
git commit -m "Add .gitignore"

# 테스트: 민감 파일 생성
"API_KEY=secret123" > .env
"password_hash" > secrets.json
"build output" > output.exe
git status -s  # 민감 파일이 표시되지 않음

Write-Host "=== .gitignore 패턴 예시 ===" -ForegroundColor Cyan
Write-Host "secrets.json   → 특정 파일" -ForegroundColor White
Write-Host "*.log          → 모든 .log 파일" -ForegroundColor White
Write-Host "bin/           → bin 디렉토리" -ForegroundColor White
Write-Host "!important.log → 예외" -ForegroundColor White
Write-Host "[abc].txt      → a.txt, b.txt, c.txt" -ForegroundColor White

Write-Host "`n=== 실수로 커밋한 경우 ===" -ForegroundColor Yellow
Write-Host "git filter-branch --force --index-filter \"git rm --cached --ignore-unmatch secrets.json\"" -ForegroundColor White
Write-Host "또는 git filter-repo 사용 (권장)" -ForegroundColor White

Write-Host "`n=== .gitattributes ===" -ForegroundColor Cyan
Write-Host "*.cs text eol=crlf   # Windows" -ForegroundColor White
Write-Host "*.sh text eof=lf     # Linux/Mac" -ForegroundColor White
Write-Host "*.png binary          # 바이너리" -ForegroundColor White

Write-Host "Done!" -ForegroundColor Green

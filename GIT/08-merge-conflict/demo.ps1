# 08: 병합 충돌 해결 데모
$demo = "$env:TEMP\git-demo-08"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

# 공통 커밋
"line 1" > conflict.txt; git add .; git commit -m "Initial"

# feature 브랜치
git switch -c feature
@"
line 1
feature line 2
feature line 3
"@ > conflict.txt
git add .; git commit -m "Feature changes"

# main에서 다른 변경
git switch main
@"
line 1
main line 2
main line 3
"@ > conflict.txt
git add .; git commit -m "Main changes"

# merge → 충돌!
try {
    git merge feature
} catch {
    Write-Host "Merge conflict 발생!" -ForegroundColor Yellow
}

# 충돌 내용 확인
Get-Content conflict.txt
Write-Host "`n--- 해결 방법 ---" -ForegroundColor Cyan
Write-Host "1. 파일 열어서 충돌 부분 수정" -ForegroundColor White
Write-Host "2. <<<<<<< / ======= / >>>>>>> 제거" -ForegroundColor White
Write-Host "3. git add conflict.txt" -ForegroundColor White
Write-Host "4. git commit" -ForegroundColor White
Write-Host "`n또는: git merge --abort 로 취소" -ForegroundColor Cyan

Write-Host "Done!" -ForegroundColor Green

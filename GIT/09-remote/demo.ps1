# 09: 원격 저장소 데모
$demo = "$env:TEMP\git-demo-09"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir "$demo-local" | Out-Null

# 로컬 저장소 생성
Set-Location "$demo-local"
git init
"README" > README.md; git add .; git commit -m "Initial"

# 베어 저장소 생성 (원격 역할)
mkdir "$demo-remote" | Out-Null
Set-Location "$demo-remote"
git init --bare

# 원격 연결
Set-Location "$demo-local"
git remote add origin "$demo-remote"
git remote -v

# push
git push -u origin main

# clone으로 검증
Set-Location "$demo"
git clone "$demo-remote" clone-test
Set-Location clone-test
git log --oneline

Set-Location "$demo-local"
"new file" > new.txt; git add .; git commit -m "Add new"
git push

Set-Location "$demo\clone-test"
git pull
git log --oneline

Write-Host "Done!" -ForegroundColor Green

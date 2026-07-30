# 15: Cherry-pick 데모
$demo = "$env:TEMP\git-demo-15"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

"base" > file.txt; git add .; git commit -m "Initial"

# feature 브랜치에서 여러 커밋
git switch -c feature
"feat1" > f1.txt; git add .; git commit -m "Feature 1"
"feat2" > f2.txt; git add .; git commit -m "Feature 2"  # 이 커밋만 가져오기
"feat3" > f3.txt; git add .; git commit -m "Feature 3"

git switch main
"main update" > m.txt; git add .; git commit -m "Main update"

# feature의 두 번째 커밋만 cherry-pick
$commit2 = git log feature --oneline | Select-Object -Skip 1 -First 1 | ForEach-Object { $_.Split(' ')[0] }
git cherry-pick $commit2 -X theirs --no-commit  # 충돌 시 theirs 사용
if ($LASTEXITCODE -eq 0) {
    git commit -m "Cherry-pick: Feature 2"
}

Write-Host "=== Log ===" -ForegroundColor Cyan
git log --oneline --graph --all

Write-Host "`nDone!" -ForegroundColor Green

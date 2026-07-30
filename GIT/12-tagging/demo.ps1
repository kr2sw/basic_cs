# 12: 태그 데모
$demo = "$env:TEMP\git-demo-12"
Remove-Item -Recurse -Force $demo -ErrorAction SilentlyContinue
mkdir $demo | Out-Null; Set-Location $demo
git init

# 여러 커밋
"v1 content" > app.txt; git add .; git commit -m "Initial commit"

# Lightweight 태그
git tag v0.1.0

# Annotated 태그 (권장)
git tag -a v1.0.0 -m "First stable release"

# 기능 추가 후 태그
"v2 content" >> app.txt; git add .; git commit -m "Add feature"
git tag -a v1.1.0 -m "Add new feature"

# 버그 수정 후 태그
"fix" >> app.txt; git add .; git commit -m "Fix bug"
git tag -a v1.1.1 -m "Bug fix release"

# 태그 목록
Write-Host "=== Tags ===" -ForegroundColor Cyan
git tag -l "v*"

# 태그 상세
Write-Host "`n=== Tag Details ===" -ForegroundColor Cyan
git show v1.0.0 --no-patch

Write-Host "`n=== SemVer ===" -ForegroundColor Cyan
Write-Host "v1.0.0 - First stable" -ForegroundColor White
Write-Host "v1.1.0 - New feature (minor)" -ForegroundColor White
Write-Host "v1.1.1 - Bug fix (patch)" -ForegroundColor White
Write-Host "v2.0.0 - Breaking change (major)" -ForegroundColor White

Write-Host "`nDone!" -ForegroundColor Green

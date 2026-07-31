# 25: Git 훅 데모 — pre-commit으로 코드 품질 검사
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-25"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root | Out-Null

try {
  Set-Location $root
  git init | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"

  Write-Host "== 1. hooksPath를 .githooks로 지정 ==" -ForegroundColor Cyan
  git config core.hooksPath .githooks
  New-Item -ItemType Directory -Path ".githooks" | Out-Null

  Write-Host "== 2. pre-commit 훅 작성 ==" -ForegroundColor Cyan
  $hook = @'
#!/bin/sh
# .githooks/pre-commit
echo "[pre-commit] 코드 검사 시작"
if git diff --cached --name-only | grep -E "\.(txt|md)$"; then
  echo "[pre-commit] 실수로 .txt/.md 파일을 커밋하는지 감지"
  echo "→ 규칙 예시: 모든 코드는 review 필요"
fi
echo "[pre-commit] 통과"
'@
  Set-Content ".githooks\pre-commit" $hook

  Write-Host "== 3. commit-msg 훅 (메시지 형식 검증) ==" -ForegroundColor Cyan
  $commitHook = @'
#!/bin/sh
# .githooks/commit-msg
MSG=$(cat "$1")
echo "$MSG" | grep -E "^(feat|fix|docs|refactor):" > /dev/null
if [ $? -ne 0 ]; then
  echo "[commit-msg] 메시지는 'feat:|fix:|docs:|refactor:' 형식이어야 합니다."
  exit 1
fi
'@
  Set-Content ".githooks\commit-msg" $commitHook

  Write-Host "== 4. 정상 커밋 (feat: 형식) ==" -ForegroundColor Cyan
  Set-Content app.js "console.log('hello')"
  git add app.js
  git commit -m "feat: 앱 추가"

  Write-Host "== 5. 잘못된 형식 커밋 시도 ==" -ForegroundColor Cyan
  Set-Content app.js "console.log('v2')"
  git add app.js
  git commit -m "그냥 수정" 2>&1 | Out-String
  Write-Host "→ commit-msg 훅이 실패하여 커밋이 차단됨"

  Write-Host "== 6. 형식에 맞춰 다시 커밋 ==" -ForegroundColor Cyan
  git commit -m "fix: 버그 수정"
  git log --oneline
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

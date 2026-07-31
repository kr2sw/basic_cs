# 23: Actions 매트릭스/재사용 워크플로우 데모
$ErrorActionPreference = 'Stop'
$root = Join-Path $env:TEMP "git-intermediate-23"
if (Test-Path $root) { Remove-Item $root -Recurse -Force }
New-Item -ItemType Directory -Path $root -Force | Out-Null

try {
  Set-Location $root
  git init | Out-Null
  git config user.email "demo@example.com"
  git config user.name "Demo"
  New-Item -ItemType Directory -Path ".github\workflows" -Force | Out-Null

  Write-Host "== 1. 매트릭스 빌드 워크플로우 ==" -ForegroundColor Cyan
  $matrix = @'
name: Matrix CI
on: push

jobs:
  test:
    runs-on: ${{ matrix.os }}
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest]
        node: [18, 22]
    steps:
      - uses: actions/checkout@v4
      - name: Node ${{ matrix.node }} 테스트
        run: echo "os=${{ matrix.os }} node=${{ matrix.node }}"
'@
  Set-Content ".github\workflows\matrix.yml" $matrix

  Write-Host "== 2. 재사용 워크플로우(템플릿) ==" -ForegroundColor Cyan
  $template = @'
name: Test Template
on:
  workflow_call:
    inputs:
      node-version:
        type: string
        required: true

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: echo "노드 버전 ${{ inputs.node-version }}로 테스트"
'@
  Set-Content ".github\workflows\test-template.yml" $template

  Write-Host "== 3. 템플릿 호출 워크플로우 ==" -ForegroundColor Cyan
  $caller = @'
name: Caller
on: push

jobs:
  call-test:
    uses: ./.github/workflows/test-template.yml
    with:
      node-version: "22"
'@
  Set-Content ".github\workflows\caller.yml" $caller

  git add .; git commit -m "매트릭스 + 재사용 워크플로우" | Out-Null
  Write-Host "생성된 워크플로우 파일:" -ForegroundColor Cyan
  Get-ChildItem ".github\workflows" | Select-Object -ExpandProperty Name
} finally {
  Set-Location $env:TEMP
  Remove-Item $root -Recurse -Force
}

# 23: Actions 고급 — 매트릭스 빌드, 재사용 워크플로우

매트릭스(Matrix)로 여러 OS/버전 조합을 병렬 테스트하고, 재사용 워크플로우(reusable workflow)로 중복을 제거합니다.

## 매트릭스

```yaml
strategy:
  matrix:
    os: [ubuntu-latest, windows-latest]
    node: [18, 20, 22]
```

6가지 조합(2 OS × 3 Node)이 각각 실행됩니다.

## 재사용 워크플로우

```yaml
# .github/workflows/test-template.yml
on:
  workflow_call:
    inputs:
      node-version:
        type: string
        required: true
```

다른 워크플로우에서 `uses: ./.github/workflows/test-template.yml`로 호출합니다.

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```

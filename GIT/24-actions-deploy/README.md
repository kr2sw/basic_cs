# 24: Actions 배포 — environments, secrets, approve

배포 워크플로우는 환경(Environment)과 시크릿(Secret)으로 보호합니다.

## Environments

```yaml
jobs:
  deploy:
    runs-on: ubuntu-latest
    environment:
      name: production
      url: https://example.com
```

환경에 **승인 규칙**을 걸면 운영 배포 전 사람의 승인이 필요합니다.

## Secrets

- GitHub → Settings → Secrets and variables → Actions
- `${{ secrets.API_KEY }}`로 참조, 로그에 절대 노출 금지
- 환경별로 다른 시크릿 설정 가능

## 승인 흐름

`deployment_review` 이벤트가 발생하고, 승인 전까지 잡이 대기합니다.

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```

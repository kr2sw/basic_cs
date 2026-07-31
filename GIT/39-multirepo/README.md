# 39: 멀티레포 — GitHub ecosystem, 토큰, 자동화

여러 저장소에 걸친 작업을 GitHub 생태계와 자동화로 관리합니다.

## 멀티레포 시나리오

- 각 서비스/팀이 독립 저장소 보유
- 공통 코드는 npm 패키지로 게시
- **Repository Dispatch**: 한 저장소의 이벤트로 다른 저장소 워크플로우 트리거

```yaml
# repo A
- run: gh api repos/repoB/dispatches -f event_type=build
```

## 토큰 관리

- `GITHUB_TOKEN`: 각 저장소별 자동 발급
- `PAT`: 저장소를 넘어선 권한 필요 시 (범위 최소화)
- 저장소 시크릿/환경 시크릿에 등록

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```

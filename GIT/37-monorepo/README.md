# 37: 모노레포 — 전략, 툴, 경로 제한

모노레포는 여러 프로젝트를 하나의 저장소에서 관리합니다.

## 장단점

| 장점 | 단점 |
|------|------|
| 공통 코드 공유 용이 | 저장소 크기 증가 |
| 변경의 원자적 적용 | 권한 관리 복잡 |
| 의존성 버전 통일 | CI 시간 증가 |

## 전략

- **경로 제한 (Codeowners / path filters)**: `packages/auth/**` 등 팀별 소유권
- **영향 범위 분석**: 변경된 패키지의 다운스트림만 테스트
- **도구**: Turborepo, Nx, Lerna, pnpm workspaces

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```

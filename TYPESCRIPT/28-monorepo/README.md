# 28: 모노레포 — 프로젝트 레퍼런스, workspace 개념

모노레포(Monorepo)는 여러 프로젝트(패키지)를 하나의 저장소에서 관리하는 전략입니다.

## 프로젝트 레퍼런스 (TypeScript)

```json
// tsconfig.base.json
{
  "compilerOptions": {
    "composite": true,
    "declaration": true
  }
}
```

루트 `tsconfig.json`에서 `references`로 하위 프로젝트를 참조하고, `tsc -b`로 빌드합니다. 의존 관계에 따라 빌드 순서가 자동 결정됩니다.

## 워크스페이스 (npm/pnpm)

```json
{
  "workspaces": ["packages/*"]
}
```

여러 패키지가 공통으로 쓰는 의존성을 루트에 호이스팅하고, 패키지 간 로컬 참조(`workspace:*`)를 지원합니다.

`index.ts`에서 모노레포 빌드 스케줄러를 간단히 구현해 봅니다.

## 실행

```bash
cd TYPESCRIPT/28-monorepo
npx ts-node index.ts
```

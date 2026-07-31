# 29: 빌드 도구 — tsc vs esbuild/swc, tsconfig 고급

TypeScript 컴파일 타임이 느려질 때 `esbuild`/`swc` 같은 대체 컴파일러를 씁니다.

## tsc vs esbuild vs swc

| 도구 | 속도 | 타입 체크 | 설명 |
|------|------|-----------|------|
| tsc | 느림 | O | 공식 컴파일러, 타입 검사 포함 |
| esbuild | 매우 빠름 | X | 타입 지우기만 수행 (Go 기반) |
| swc | 매우 빠름 | X | Rust 기반 트랜스파일러 |

현대 개발에서는 **빠른 트랜스파일(esbuild/swc) + 별도 타입 체크(tsc --noEmit)** 를 결합합니다.

## tsconfig 고급 옵션

- `paths`: 경로 별칭
- `strict`: 모든 엄격 모드 활성화
- `noEmitOnError`: 에러 시 출력 금지
- `incremental`/`composite`: 증분 빌드
- `moduleResolution`: `bundler` / `node16`

`index.ts`에서 설정 검사기를 구현해 봅니다.

## 실행

```bash
cd TYPESCRIPT/29-build-tools
npx ts-node index.ts
```

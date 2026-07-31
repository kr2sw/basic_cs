# 38: 패키지 제작 — 라이브러리 작성, .d.ts 배포, 버전 관리

자신의 TypeScript 라이브러리를 만들어 npm에 배포하는 방법을 배웁니다.

## 라이브러리 구조

```
my-lib/
├── src/
│   └── index.ts        # 진입점
├── dist/               # 빌드 결과
│   ├── index.js        # JS
│   └── index.d.ts      # 타입 선언
└── package.json
```

## package.json의 types 필드

```json
{
  "name": "my-lib",
  "version": "1.0.0",
  "main": "dist/index.js",
  "types": "dist/index.d.ts",
  "exports": {
    ".": {
      "types": "./dist/index.d.ts",
      "import": "./dist/index.mjs",
      "require": "./dist/index.cjs"
    }
  }
}
```

## SemVer

- **MAJOR**: 하위 호환 없는 변경
- **MINOR**: 하위 호환되는 기능 추가
- **PATCH**: 버그 수정

`index.ts`에서 패키지 버전 검사기와 번들러를 구현합니다.

## 실행

```bash
cd TYPESCRIPT/38-package-authoring
npx ts-node index.ts
```

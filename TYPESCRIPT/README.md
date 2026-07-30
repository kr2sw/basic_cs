# TypeScript 기초 (20개 챕터)

TypeScript는 JavaScript에 정적 타입을 추가한 언어로, 대규모 애플리케이션 개발에 적합합니다.

## 실행 방법

```bash
# TypeScript 컴파일 후 실행
cd TYPESCRIPT/01-introduction
npx tsc index.ts   # JS로 컴파일
node index.js      # 실행

# 또는 ts-node로 직접 실행
npx ts-node index.ts
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | TypeScript 소개, tsc, tsconfig, 기본 타입 |
| 02 | Basic Types | number/string/boolean, tuple, enum, any/unknown/never |
| 03 | Interfaces | interface, optional/readonly, extends, index signature |
| 04 | Types | type alias, union, intersection, literal types |
| 05 | Functions | 매개변수/반환 타입, 오버로드, this |
| 06 | Classes | class, implements, abstract, parameter properties |
| 07 | Generics | 제네릭 함수/클래스/제약, infer |
| 08 | Enums & Type Guards | enum, typeof, instanceof, discriminated union |
| 09 | Utility Types | Partial, Required, Pick, Omit, Record, ReturnType |
| 10 | Modules | export/import, namespace, ambient 선언 |
| 11 | Type Manipulation | keyof, typeof, indexed access, conditional types |
| 12 | Template Literal Types | 템플릿 리터럴, intrinsic string types |
| 13 | Decorators | 클래스/메서드/프로퍼티/파라미터 데코레이터 |
| 14 | Declaration Files | .d.ts, declare, module augmentation |
| 15 | Advanced Types | recursive types, branded types, satisfies |
| 16 | Configuration | tsconfig.json, strict mode, paths, references |
| 17 | React with TS | FC, useState/useRef typing, 이벤트 핸들러 |
| 18 | Node.js with TS | Express, Request/Response, 미들웨어 |
| 19 | Testing | jest, ts-jest, typed mock |
| 20 | Real-world Project | Todo API (Express + TypeScript) |

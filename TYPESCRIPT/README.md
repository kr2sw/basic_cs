# TypeScript 강의 (기초 20개 + 중급 20개 챕터)

TypeScript는 JavaScript에 정적 타입을 추가한 언어로, 대규모 애플리케이션 개발에 적합합니다.

## 역사

TypeScript는 2012년 Microsoft의 Anders Hejlsberg(C# 창시자)가 개발하여 공개했습니다. JavaScript의 동적 타이핑으로 인한 대규모 프로젝트의 유지보수 어려움을 해결하기 위해 설계되었습니다. 2014년 Angular 2가 TypeScript를 채택하면서 본격적으로 대중화되기 시작했습니다. 2017년 TypeScript 2.3에서는 --strict 모드가 도입되었고, 2020년 TypeScript 4.0에서는 가변 인자 튜플 타입, 2023년 TypeScript 5.0에서는 데코레이터 표준화와 const 타입 파라미터가 추가되었습니다. 현재는 React, Vue, Next.js, NestJS 등 주요 프레임워크가 TypeScript를 공식 지원하며, JavaScript 생태계의 사실상 표준 타입 시스템으로 자리잡았습니다.

## 특징

- **정적 타입 시스템**: 컴파일 타임에 타입 오류 검출, 생산성 향상
- **JavaScript의 상위 집합**: 모든 JS 코드가 TS에서 동작, 점진적 도입 가능
- **강력한 타입 추론**: 명시적 타입 없이도 대부분의 타입을 자동 추론
- **구조적 타이핑**: 덕 타이핑(Duck Typing) 기반의 유연한 타입 호환성
- **고급 타입 기능**: 제네릭, 유니온/교차 타입, 조건부 타입, 매핑된 타입, 템플릿 리터럴 타입
- **뛰어난 도구 지원**: VS Code의 IntelliSense, 리팩토링, 자동 완성
- **점진적 타입 시스템**: `any` 타입으로 기존 JS 프로젝트에 단계적으로 도입 가능

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
| 21 | Advanced Generics | 타입 추론, 제약, infer 패턴 |
| 22 | Type System Deep | 구조적 타이핑, 공변성/반공변성 |
| 23 | Conditional & Mapped | 분배 법칙, 재귀 타입, 키 재매핑 |
| 24 | Template Literals Deep | 문자열 파싱, CamelCase 변환 |
| 25 | Utility Type Design | Partial, Pick, ReturnType 직접 구현 |
| 26 | Type-safe APIs | zod 스키마, tRPC 개념, 미니 검증기 |
| 27 | Decorators Deep | 메서드 데코레이터, DI 컨테이너, 싱글턴 |
| 28 | Monorepo | 프로젝트 레퍼런스, workspace, 위상 정렬 |
| 29 | Build Tools | tsc vs esbuild/swc, 증분 빌드, paths |
| 30 | FP & Pipeline | Option/Either 패턴, pipe, compose |
| 31 | Express + TS | 타입 안전 라우터, 제네릭 핸들러 |
| 32 | GraphQL + TS | 스키마, 리졸버, 미니 GraphQL 엔진 |
| 33 | Testing TS | 테스트 러너, 타입 테스팅(tsd) |
| 34 | React Generics | 다형성 컴포넌트, 제네릭 훅 |
| 35 | Node + TS Advanced | 워커, 스트림, 배압(backpressure) |
| 36 | Type-safe ORM | Prisma/Drizzle 개념, DTO 변환 |
| 37 | Module Systems | ESM/CJS 상호운용, createRequire |
| 38 | Package Authoring | .d.ts 배포, SemVer, 의존성 범위 |
| 39 | Events & State Machines | 이벤트 맵, 타입 안전 FSM |
| 40 | Final Project | 타입 안전 할일 관리 CLI |

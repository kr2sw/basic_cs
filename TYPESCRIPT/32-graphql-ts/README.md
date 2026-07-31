# 32: GraphQL + TS — 타입 안전 스키마

GraphQL은 클라이언트가 필요한 데이터를 정확히 요청하는 쿼리 언어입니다. 스키마(Schema)가 강타입이므로 TypeScript와 잘 어울립니다.

## 핵심 개념

- **스키마**: 타입 정의 (`type User { id: ID! name: String! }`)
- **쿼리**: 데이터 조회 (`query { user(id: 1) { name } }`)
- **리졸버**: 필드에 실제 데이터를 채우는 함수
- **뮤테이션**: 데이터 변경

## 타입 안전성

스키마 타입을 TypeScript 타입으로 추론하면 리졸버와 쿼리 결과가 컴파일 시점에 검증됩니다.

`index.ts`에서 미니 GraphQL 엔진을 구현해 봅니다.

## 실행

```bash
cd TYPESCRIPT/32-graphql-ts
npx ts-node index.ts
```

# 24: 템플릿 리터럴 심화 — 문자열 파싱, 대문자 유틸리티

템플릿 리터럴 타입(Template Literal Types)은 문자열 리터럴을 조합해 새 타입을 만듭니다.

## 기본 문법

```typescript
type Greeting = `Hello, ${string}!`;
type Endpoint = `/api/${string}`;
```

## 내장 문자열 유틸리티

`Uppercase`, `Lowercase`, `Capitalize`, `Uncapitalize` 타입이 제공됩니다.

## 문자열 파싱

`infer`와 결합해 문자열을 패턴 매칭으로 분해할 수 있습니다.

```typescript
type ParsePath<P extends string> =
  P extends `${infer Head}/${infer Tail}` ? [Head, ...ParsePath<Tail>] : [P];
```

라우트 경로, CSS 값, URL 등에 유용합니다. 자세한 예제는 `index.ts`를 참고하세요.

## 실행

```bash
cd TYPESCRIPT/24-template-literals-deep
npx ts-node index.ts
```

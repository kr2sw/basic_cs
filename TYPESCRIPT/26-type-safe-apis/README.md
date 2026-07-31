# 26: 타입 안전 API — zod 스키마, tRPC 개념

외부에서 들어오는 데이터(API 응답, 폼 입력)는 `any` 상태이므로 **런타임 검증**과 **타입 추론**을 함께 제공하는 라이브러리가 필요합니다.

## zod란?

zod는 스키마 정의를 통해 1) 런타임 검증 2) 타입 자동 추론을 동시에 제공합니다.

```typescript
import { z } from "zod";
const UserSchema = z.object({ id: z.number(), name: z.string() });
type User = z.infer<typeof UserSchema>;  // 타입 자동 생성
```

## tRPC란?

클라이언트와 서버가 **하나의 타입을 공유**해, 타입 안전하게 원격 함수를 호출하는 프레임워크입니다. 스키마 하나로 양쪽 타입이 동기화됩니다.

이 챕터에서는 zod의 동작을 흉내 낸 미니 스키마 검증기를 직접 구현합니다.

## 실행

```bash
cd TYPESCRIPT/26-type-safe-apis
npx ts-node index.ts
```

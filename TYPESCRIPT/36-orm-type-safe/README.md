# 36: 타입 안전 ORM — Prisma/Drizzle 개념, DTO 변환

ORM(Object-Relational Mapping)은 DB 테이블과 코드 객체를 매핑합니다. 타입 안전 ORM(Prisma, Drizzle)은 스키마로부터 타입을 자동 생성해 컴파일 시점에 DB 접근을 검증합니다.

## Prisma 스키마

```prisma
model User {
  id    Int     @id @default(autoincrement())
  name  String
  posts Post[]
}
```

## 자동 생성 타입

```typescript
import { PrismaClient } from "@prisma/client";
const user = await prisma.user.findFirst({ where: { name: "Alice" } });
// user의 타입은 스키마에서 자동 생성됨
```

`index.ts`에서 스키마 → 타입 → CRUD 변환 계층(DTO)을 직접 구현합니다.

## 실행

```bash
cd TYPESCRIPT/36-orm-type-safe
npx ts-node index.ts
```

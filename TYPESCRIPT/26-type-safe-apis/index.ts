// 26: 타입 안전 API — zod 스키마, tRPC 개념

// 미니 zod 유사 스키마 검증기 구현
type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

// === 1. 스키마 기본 타입 ===
type Schema<T> = { parse: (input: unknown) => T; _type: T };

function string(): Schema<string> {
  return {
    _type: "" as string,
    parse(input: unknown): string {
      if (typeof input !== "string") throw new Error(`문자열이 아님: ${input}`);
      return input;
    },
  };
}

function number(): Schema<number> {
  return {
    _type: 0 as number,
    parse(input: unknown): number {
      if (typeof input !== "number") throw new Error(`숫자가 아님: ${input}`);
      return input;
    },
  };
}

function boolean(): Schema<boolean> {
  return {
    _type: true as boolean,
    parse(input: unknown): boolean {
      if (typeof input !== "boolean") throw new Error(`불리언이 아님: ${input}`);
      return input;
    },
  };
}

function optional<T>(schema: Schema<T>): Schema<T | undefined> {
  return {
    _type: undefined as T | undefined,
    parse(input: unknown): T | undefined {
      if (input === undefined) return undefined;
      return schema.parse(input);
    },
  };
}

// === 2. object 스키마 ===
function object<S extends Record<string, Schema<unknown>>>(schemas: S): Schema<{
  [K in keyof S]: S[K] extends Schema<infer T> ? T : never;
}> {
  return {
    _type: undefined as never,
    parse(input: unknown) {
      if (typeof input !== "object" || input === null) throw new Error("객체가 아님");
      const record = input as Record<string, unknown>;
      const result = {} as Record<string, unknown>;
      for (const key of Object.keys(schemas)) {
        result[key] = schemas[key].parse(record[key]);
      }
      return result as never;
    },
  };
}

// === 3. array 스키마 ===
function array<T>(schema: Schema<T>): Schema<T[]> {
  return {
    _type: [] as T[],
    parse(input: unknown): T[] {
      if (!Array.isArray(input)) throw new Error("배열이 아님");
      return input.map((item) => schema.parse(item));
    },
  };
}

// === 4. 실제 사용 ===
const UserSchema = object({
  id: number(),
  name: string(),
  email: string(),
  age: optional(number()),
});
type User = { [K in keyof typeof UserSchema]: (typeof UserSchema)[K] extends Schema<infer T> ? T : never };

function fetchUser(raw: unknown): User {
  return UserSchema.parse(raw) as User;
}

// === 5. 클라이언트/서버 타입 공유 (tRPC 개념) ===
type Router<T extends Record<string, (args: unknown) => unknown>> = T;

function createRouter<T extends Record<string, (args: unknown) => unknown>>(routes: T): Router<T> {
  return routes;
}

const appRouter = createRouter({
  getUser: (input: unknown) => {
    const user = UserSchema.parse(input);
    return `Hello, ${user.name}!`;
  },
  listUsers: (_input: unknown) => ["a", "b"],
});
// appRouter의 타입이 그대로 클라이언트로 전달되는 것이 tRPC의 핵심

function clientCall<T>(router: Router<T>, name: keyof T, arg: Parameters<T[keyof T]>[0]) {
  return (router[name] as (a: unknown) => unknown)(arg);
}

// === 6. 실제 실행 ===
try {
  const valid = fetchUser({ id: 1, name: "Kim", email: "k@e.com" });
  console.log("검증 성공:", JSON.stringify(valid));

  const msg = clientCall(appRouter, "getUser", { id: 2, name: "Lee", email: "l@e.com", age: 30 });
  console.log("RPC 호출:", msg);
} catch (e) {
  console.error("에러:", e);
}

// 잘못된 입력은 검증에서 실패
try {
  fetchUser({ id: "wrong", name: "Kim" });
} catch (e) {
  console.log("잘못된 입력 차단:", (e as Error).message);
}

console.log("타입 안전 API 데모 완료!");

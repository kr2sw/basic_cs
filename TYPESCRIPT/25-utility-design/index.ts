// 25: 유틸리티 타입 설계 — Partial, Pick, ReturnType 직접 구현

type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

interface User {
  id: number;
  name: string;
  email: string;
  age?: number;
}

// === 1. Partial 직접 구현 ===
type MyPartial<T> = { [K in keyof T]?: T[K] };
type A = MyPartial<User>;

// === 2. Required 직접 구현 ===
type MyRequired<T> = { [K in keyof T]-?: T[K] };
type B = MyRequired<User>;

// === 3. Readonly 직접 구현 ===
type MyReadonly<T> = { readonly [K in keyof T]: T[K] };
type C = MyReadonly<User>;

// === 4. Pick 직접 구현 ===
type MyPick<T, K extends keyof T> = { [P in K]: T[P] };
type D = MyPick<User, "id" | "name">;

// === 5. Omit 직접 구현 (Exclude + Pick) ===
type MyExclude<T, U> = T extends U ? never : T;
type MyOmit<T, K extends keyof T> = MyPick<T, MyExclude<keyof T, K>>;
type E = MyOmit<User, "email">;

// === 6. Record 직접 구현 ===
type MyRecord<K extends keyof any, T> = { [P in K]: T };
type F = MyRecord<"a" | "b", number>;

// === 7. ReturnType 직접 구현 ===
type MyReturnType<T> = T extends (...args: never[]) => infer R ? R : never;
type G = MyReturnType<(a: number, b: number) => string>;  // string

// === 8. Parameters 직접 구현 ===
type MyParameters<T> = T extends (...args: infer P) => unknown ? P : never;
type H = MyParameters<(a: number, b: string) => void>;  // [number, string]

// === 9. Awaited 직접 구현 ===
type MyAwaited<T> = T extends Promise<infer U> ? MyAwaited<U> : T;
type I = MyAwaited<Promise<Promise<boolean>>>;  // boolean

// === 10. NonNullable 직접 구현 ===
type MyNonNullable<T> = T extends null | undefined ? never : T;
type J = MyNonNullable<string | null | undefined>;  // string

// === 실제 활용 ===
function patchUser(target: User, patch: MyPartial<User>): User {
  return { ...target, ...patch };
}
const updated = patchUser({ id: 1, name: "Kim", email: "k@e.com" }, { age: 20 });
console.log("부분 업데이트:", JSON.stringify(updated));

type Keys = MyOmit<keyof User, "email">;
console.log("Omit 결과 키:", Keys);

// === 타입 검증 ===
type Test1 = Expect<Equal<A, Partial<User>>>;
type Test2 = Expect<Equal<B, Required<User>>>;
type Test3 = Expect<Equal<D, Pick<User, "id" | "name">>>;
type Test4 = Expect<Equal<E, Omit<User, "email">>>;
type Test5 = Expect<Equal<G, string>>;
type Test6 = Expect<Equal<H, [number, string]>>;
type Test7 = Expect<Equal<I, boolean>>;
type Test8 = Expect<Equal<J, string>>;

console.log("모든 타입 검증 통과!");

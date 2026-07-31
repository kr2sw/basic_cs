// 23: 조건부/매핑 타입 — 분배 법칙, 재귀 타입

type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

// === 1. 기본 조건부 타입 ===
type IsString<T> = T extends string ? true : false;
type A = IsString<"hello">;  // true
type B = IsString<42>;       // false

// === 2. 분배 법칙 ===
// T가 유니온이면 각 멤버에 개별 적용
type ToArray<T> = T extends unknown ? T[] : never;
type C = ToArray<string | number>;  // string[] | number[]

// 분배를 막으려면 [] 로 감싼다
type NonDistributive<T> = [T] extends [unknown] ? T[] : never;
type D = NonDistributive<string | number>;  // (string | number)[]

// === 3. 분배를 활용한 필터 ===
type ExcludeType<T, U> = T extends U ? never : T;
type E = ExcludeType<"a" | "b" | "c", "a">;  // "b" | "c"

// === 4. 매핑 타입 ===
type MyReadonly<T> = { readonly [K in keyof T]: T[K] };
type MyPartial<T> = { [K in keyof T]?: T[K] };
type MyPick<T, K extends keyof T> = { [P in K]: T[P] };

interface User {
  id: number;
  name: string;
  email: string;
}

type ReadonlyUser = MyReadonly<User>;
type PartialUser = MyPartial<User>;
type PickedUser = MyPick<User, "id" | "name">;

// === 5. 키 재매핑 (as 절) ===
type Getters<T> = { [K in keyof T as `get${Capitalize<string & K>}`]: () => T[K] };
type UserGetters = Getters<User>;
// { getId: () => number; getName: () => string; getEmail: () => string }

// === 6. 재귀 타입 ===
type DeepReadonly<T> = {
  readonly [K in keyof T]: T[K] extends object ? DeepReadonly<T[K]> : T[K];
};

interface Nested {
  a: { b: { c: number } };
  arr: { x: number }[];
}

type DeepRO = DeepReadonly<Nested>;

// === 7. 재귀 조건부 타입 (튜플 유틸리티) ===
type Last<T extends unknown[]> = T extends [...infer _, infer L] ? L : never;
type F = Last<[1, 2, 3]>;  // 3

type WithoutFirst<T extends unknown[]> = T extends [unknown, ...infer Rest] ? Rest : [];
type G = WithoutFirst<[1, 2, 3]>;  // [2, 3]

// === 8. JSON 타입 표현 ===
type JsonPrimitive = string | number | boolean | null;
type Json = JsonPrimitive | Json[] | { [key: string]: Json };

const data: Json = { name: "kim", tags: ["a", "b"], meta: { count: 2 } };
console.log("JSON 타입:", JSON.stringify(data));

// === 타입 검증 ===
type Test1 = Expect<Equal<A, true>>;
type Test2 = Expect<Equal<B, false>>;
type Test3 = Expect<Equal<E, "b" | "c">>;
type Test4 = Expect<Equal<F, 3>>;
type Test5 = Expect<Equal<G, [2, 3]>>;

console.log("모든 타입 검증 통과!");

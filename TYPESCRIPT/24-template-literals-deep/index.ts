// 24: 템플릿 리터럴 심화 — 문자열 파싱, 대문자 유틸리티

type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

// === 1. 기본 템플릿 리터럴 타입 ===
type Greeting = `Hello, ${string}!`;
const g1: Greeting = "Hello, World!";

type HttpMethod = "GET" | "POST" | "PUT" | "DELETE";
type Endpoint = `${HttpMethod} /api/${string}`;
const e1: Endpoint = "GET /api/users";

// === 2. 문자열 유틸리티 ===
type Upper = Uppercase<"hello">;      // "HELLO"
type Lower = Lowercase<"HELLO">;      // "hello"
type Cap = Capitalize<"hello world">; // "Hello world"
type Uncap = Uncapitalize<"Hello">;   // "hello"

// === 3. infer로 문자열 파싱 ===
type SplitPath<P extends string> =
  P extends `${infer Head}/${infer Tail}` ? [Head, ...SplitPath<Tail>] : [P];

type A = SplitPath<"a/b/c">;  // ["a", "b", "c"]

// === 4. 라우트 경로 타입 ===
type RouteParams<R extends string> =
  R extends `${string}:${infer Param}/${infer Rest}`
    ? { [K in Param | keyof RouteParams<Rest>]: string }
    : R extends `${string}:${infer Param}`
      ? { [K in Param]: string }
      : {};

type Route = "/users/:id/posts/:postId";
type Params = RouteParams<Route>;
// { id: string; postId: string }

function buildPath<R extends string>(route: R, params: RouteParams<R>): string {
  return route.replace(/:(\w+)/g, (_, key) => params[key as keyof typeof params]);
}

const path = buildPath("/users/:id/posts/:postId", { id: "1", postId: "42" });
console.log("생성된 경로:", path);

// === 5. CSS 값 타입 ===
type Size = `${number}px` | `${number}%` | `${number}rem`;
const s1: Size = "12px";
const s2: Size = "50%";

// === 6. 이벤트 맵과 조합 ===
type Events = "click" | "focus" | "change";
type HandlerMap = { [K in `on${Capitalize<Events>}`]: (e: unknown) => void };
// { onClick: ...; onFocus: ...; onChange: ... }

// === 7. 문자열 반전 (재귀) ===
type Reverse<S extends string> = S extends `${infer Head}${infer Rest}`
  ? `${Reverse<Rest>}${Head}`
  : "";
type B = Reverse<"abc">;  // "cba"

// === 8. snake_case → camelCase ===
type CamelCase<S extends string> = S extends `${infer First}_${infer Rest}`
  ? `${First}${Capitalize<CamelCase<Rest>>}`
  : S;
type C = CamelCase<"user_profile_picture">;  // "userProfilePicture"

// === 타입 검증 ===
type Test1 = Expect<Equal<A, ["a", "b", "c"]>>;
type Test2 = Expect<Equal<B, "cba">>;
type Test3 = Expect<Equal<C, "userProfilePicture">>;
type Test4 = Expect<Equal<Params, { id: string; postId: string }>>;

console.log("모든 타입 검증 통과!");

// 22: 타입 시스템 심화 — 구조적 타이핑, 타입 동작 원리

type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

// === 1. 구조적 타이핑 (덕 타이핑) ===
interface Point { x: number; y: number; }
interface NamedPoint { x: number; y: number; name: string; }

function draw(p: Point): string {
  return `(${p.x}, ${p.y})`;
}

const named: NamedPoint = { x: 5, y: 7, name: "A" };
console.log("구조적 타이핑:", draw(named));  // NamedPoint는 Point로 호환

// === 2. 초과 속성 검사 (Excess Property Check) ===
// 객체 리터럴은 초과 속성이 있으면 에러
const ok: Point = { x: 1, y: 2 };        // OK
// const bad: Point = { x: 1, y: 2, z: 3 };  // Error: z는 없음

// === 3. 선택적 속성과 readonly ===
interface Config {
  name: string;
  timeout?: number;       // undefined 허용
  readonly id: number;    // 재할당 불가 (컴파일 타임)
}

const config: Config = { name: "app", id: 1 };
console.log("선택적 속성:", config.timeout ?? "기본값 1000");

// === 4. 함수 타입의 공변성/반공변성 ===
type StringPredicate = (value: string) => boolean;
type StringOrNumberPredicate = (value: string | number) => boolean;

const strOnly: StringPredicate = (s) => s.length > 0;
// 반환 타입: 더 좁은 타입 반환 가능 (공변)
const narrower: StringPredicate = (s) => s === "yes";
// 매개변수: supertype 파라미터 허용 (반공변, strictFunctionTypes)
const wider: StringOrNumberPredicate = (v) => typeof v === "string" && v.length > 0;
const asStr: StringPredicate = wider;

console.log("함수 호환성:", asStr("hello"), asStr(123 as unknown as string));

// === 5. 유니온과 인터섹션 ===
type Status = "idle" | "loading" | "success" | "error";
type Result = { data: string } | { error: string };

function handle(result: Result): string {
  return "data" in result ? `성공: ${result.data}` : `실패: ${result.error}`;
}

console.log(handle({ data: "ok" }));
console.log(handle({ error: "boom" }));

// === 6. 타입의 타입: 값과 타입 네임스페이스 ===
const value = 42;               // 값
type ValueType = typeof value;  // 타입 = number
type A = Expect<Equal<ValueType, number>>;

// === 7. typeof와 keyof 조합 ===
const obj = { a: 1, b: "two", c: true } as const;
type ObjKeys = keyof typeof obj;          // "a" | "b" | "c"
type ObjValues = (typeof obj)[ObjKeys];   // 1 | "two" | true

const keys: ObjKeys[] = ["a", "b", "c"];
console.log("keyof typeof:", keys);

console.log("모든 타입 검증 통과!");

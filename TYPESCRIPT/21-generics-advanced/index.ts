// 21: 고급 제네릭 — 타입 추론, 제약, infer 패턴

// === 타입 수준 테스트 유틸리티 ===
type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

// === 1. 기본 제네릭 함수와 추론 ===
function identity<T>(value: T): T {
  return value;
}

const num = identity(42);       // number
const str = identity("hello");  // string

// === 2. 제약 조건 (Constraints) ===
function getLength<T extends { length: number }>(value: T): number {
  return value.length;
}

console.log("문자열 길이:", getLength("TypeScript"));
console.log("배열 길이:", getLength([1, 2, 3]));

// === 3. 제네릭 인터페이스와 클래스 ===
interface Pair<K, V> {
  key: K;
  value: V;
}

class Stack<T> {
  private items: T[] = [];

  push(item: T): void {
    this.items.push(item);
  }

  pop(): T | undefined {
    return this.items.pop();
  }

  get size(): number {
    return this.items.length;
  }
}

const stack = new Stack<number>();
stack.push(10);
stack.push(20);
console.log("스택 pop:", stack.pop(), "| 남은 크기:", stack.size);

// === 4. keyof 제약 ===
function getProperty<T, K extends keyof T>(obj: T, key: K): T[K] {
  return obj[key];
}

const user = { name: "Alice", age: 30 };
console.log("name:", getProperty(user, "name"), "| age:", getProperty(user, "age"));

// === 5. infer 패턴 ===
type ElementType<T> = T extends (infer U)[] ? U : never;
type A = ElementType<string[]>;  // string
type B = ElementType<number[]>;  // number

type MyReturnType<T> = T extends (...args: never[]) => infer R ? R : never;
type C = MyReturnType<(x: number) => boolean>;  // boolean

type UnwrapPromise<T> = T extends Promise<infer U> ? U : T;
type D = UnwrapPromise<Promise<string>>;  // string

type MyAwaited<T> = T extends Promise<infer U> ? MyAwaited<U> : T;
type E = MyAwaited<Promise<Promise<number>>>;  // number

// === 6. infer를 활용한 튜플 뒤집기 ===
type ReverseTuple<T extends unknown[], R extends unknown[] = []> =
  T extends [infer Head, ...infer Tail] ? ReverseTuple<Tail, [Head, ...R]> : R;

type F = ReverseTuple<[1, 2, 3]>;  // [3, 2, 1]

// === 타입 검증 ===
type Test1 = Expect<Equal<A, string>>;
type Test2 = Expect<Equal<B, number>>;
type Test3 = Expect<Equal<C, boolean>>;
type Test4 = Expect<Equal<D, string>>;
type Test5 = Expect<Equal<E, number>>;
type Test6 = Expect<Equal<F, [3, 2, 1]>>;

console.log("모든 타입 검증 통과!");

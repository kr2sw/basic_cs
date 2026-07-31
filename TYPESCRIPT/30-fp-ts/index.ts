// 30: 함수형 프로그래밍 — Option/Either 패턴, 파이프라인

// === 1. Option 타입 구현 ===
type Option<T> = { kind: "some"; value: T } | { kind: "none" };

const some = <T>(value: T): Option<T> => ({ kind: "some", value });
const none = <T>(): Option<T> => ({ kind: "none" });

function mapOption<T, U>(opt: Option<T>, fn: (v: T) => U): Option<U> {
  return opt.kind === "some" ? some(fn(opt.value)) : none();
}

function flatMap<T, U>(opt: Option<T>, fn: (v: T) => Option<U>): Option<U> {
  return opt.kind === "some" ? fn(opt.value) : none();
}

function getOrElse<T>(opt: Option<T>, fallback: T): T {
  return opt.kind === "some" ? opt.value : fallback;
}

function fromNullable<T>(v: T | null | undefined): Option<T> {
  return v == null ? none() : some(v);
}

// === 2. 안전한 나눗셈 ===
function safeDivide(a: number, b: number): Option<number> {
  return b === 0 ? none() : some(a / b);
}

console.log("10/2:", getOrElse(safeDivide(10, 2), 0));
console.log("10/0:", getOrElse(safeDivide(10, 0), 0));

// === 3. Option 체이닝 ===
const getUserAge = (id: number): Option<number> =>
  fromNullable(users.get(id)).flatMap((name) => mapOption(fromNullable(ageByName(name)), (a) => a));

const users = new Map<number, string>([[1, "Alice"], [2, "Bob"]]);
const ageByName = (name: string): number | undefined => (name === "Alice" ? 30 : undefined);

console.log("Alice 나이:", getOrElse(getUserAge(1), -1));
console.log("Bob 나이:", getOrElse(getUserAge(2), -1));

// === 4. Either 타입 (성공/실패) ===
type Either<E, A> = { kind: "left"; error: E } | { kind: "right"; value: A };

const left = <E, A>(error: E): Either<E, A> => ({ kind: "left", error });
const right = <E, A>(value: A): Either<E, A> => ({ kind: "right", value });

function parseJson(input: string): Either<string, unknown> {
  try {
    return right(JSON.parse(input));
  } catch {
    return left("JSON 파싱 실패");
  }
}

function bind<E, A, B>(e: Either<E, A>, fn: (a: A) => Either<E, B>): Either<E, B> {
  return e.kind === "right" ? fn(e.value) : e;
}

const result = bind(
  bind(parseJson('{"name":"Kim","age":20}'), (obj) =>
    typeof (obj as { age?: unknown }).age === "number"
      ? right((obj as { age: number }).age * 2)
      : left("age 없음")
  ),
  (age) => right(`두 배 나이: ${age}`)
);
console.log("Either 체인:", result.kind === "right" ? result.value : result.error);

// === 5. 파이프라인 함수 ===
function pipe<A>(value: A): {
  through: <B>(fn: (a: A) => B) => ReturnType<typeof pipe<B>>;
  done: () => A;
};
function pipe<A>(value: A) {
  return {
    through<B>(fn: (a: A) => B) {
      return pipe(fn(value));
    },
    done: () => value,
  };
}

const finalValue = pipe(5)
  .through((n) => n * 2)
  .through((n) => n + 1)
  .through((n) => `결과: ${n}`)
  .done();

console.log("파이프라인:", finalValue);

// === 6. 배열 파이프라인 (compose) ===
const compose = <A>(...fns: Array<(a: any) => any>) => (initial: A) =>
  fns.reduce((acc, fn) => fn(acc), initial);

const double = (n: number) => n * 2;
const addTax = (n: number) => n * 1.1;
const toFixed2 = (n: number) => n.toFixed(2);

const calculate = compose(double, addTax, toFixed2);
console.log("compose:", calculate(100));  // "220.00"

console.log("\nFP 데모 완료!");

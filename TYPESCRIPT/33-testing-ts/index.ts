// 33: TS 테스팅 — vitest/jest, 타입 테스팅

// === 1. 테스트 대상 함수 ===
function add(a: number, b: number): number {
  return a + b;
}

function multiply(a: number, b: number): number {
  return a * b;
}

function safeParse(input: string): number | null {
  const n = Number(input);
  return Number.isNaN(n) ? null : n;
}

// === 2. 미니 테스트 러너 (vitest/jest 유사) ===
type TestCase = { name: string; fn: () => void };

function describe(label: string, fn: () => void) {
  console.log(`\n[${label}]`);
  fn();
}

let passed = 0;
let failed = 0;

function it(name: string, fn: () => void) {
  try {
    fn();
    passed++;
    console.log(`  ✓ ${name}`);
  } catch (e) {
    failed++;
    console.log(`  ✗ ${name} — ${(e as Error).message}`);
  }
}

function expect<T>(actual: T) {
  return {
    toBe(expected: T) {
      if (actual !== expected) throw new Error(`기대: ${expected}, 실제: ${actual}`);
    },
    toEqual(expected: T) {
      if (JSON.stringify(actual) !== JSON.stringify(expected)) {
        throw new Error(`기대: ${JSON.stringify(expected)}, 실제: ${JSON.stringify(actual)}`);
      }
    },
    toBeNull() {
      if (actual !== null) throw new Error(`기대: null, 실제: ${actual}`);
    },
  };
}

// === 3. 테스트 작성 ===
describe("add()", () => {
  it("두 양수를 더한다", () => {
    expect(add(1, 2)).toBe(3);
  });
  it("음수를 처리한다", () => {
    expect(add(-1, -2)).toBe(-3);
  });
});

describe("multiply()", () => {
  it("곱셈 결과", () => {
    expect(multiply(3, 4)).toBe(12);
  });
  it("0 곱셈", () => {
    expect(multiply(0, 5)).toBe(0);
  });
});

describe("safeParse()", () => {
  it("숫자 문자열 변환", () => {
    expect(safeParse("42")).toBe(42);
  });
  it("잘못된 입력은 null", () => {
    expect(safeParse("abc")).toBeNull();
  });
});

// === 4. 결과 요약 ===
console.log(`\n결과: ${passed} 통과, ${failed} 실패`);
if (failed > 0) process.exit(1);

// === 5. 타입 수준 테스트 (tsd 스타일) ===
type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

type T1 = Expect<Equal<ReturnType<typeof add>, number>>;
type T2 = Expect<Equal<Parameters<typeof add>, [number, number]>>;
type T3 = Expect<Equal<ReturnType<typeof safeParse>, number | null>>;

// === 6. vitest 사용 예시 (실제 프로젝트용, 주석) ===
/*
import { describe, it, expect, vi } from "vitest";

const mockFn = vi.fn();
mockFn.mockReturnValue(42);
expect(mockFn()).toBe(42);
*/

console.log("타입 검증 통과 + 테스트 러너 데모 완료!");

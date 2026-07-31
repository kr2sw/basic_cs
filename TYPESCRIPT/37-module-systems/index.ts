// 37: 모듈 시스템 심화 — ESM/CJS 상호운용, import 타입

// === 1. CJS 모듈 시뮬레이션 (require/module.exports) ===
type Exports = Record<string, unknown>;

const moduleCache = new Map<string, Exports>();

function createModule(loader: (exports: Exports) => void): Exports {
  const exports: Exports = {};
  loader(exports);
  return exports;
}

// math 모듈 (CJS 스타일)
const mathExports = createModule((exports) => {
  exports.add = (a: number, b: number) => a + b;
  exports.PI = 3.14159;
});

moduleCache.set("math", mathExports);

// === 2. require 구현 ===
function requireCjs(id: string): Exports {
  const mod = moduleCache.get(id);
  if (!mod) throw new Error(`모듈을 찾을 수 없음: ${id}`);
  return mod;
}

const math = requireCjs("math");
console.log("CJS require:", math.add(2, 3), "| PI:", math.PI);

// === 3. ESM 모듈 시뮬레이션 (import/export) ===
interface ESMNamespace {
  [key: string]: unknown;
}

function createESMModule(exports: ESMNamespace): ESMNamespace {
  return Object.freeze(exports);  // ESM은 읽기 전용 네임스페이스
}

// utils 모듈 (ESM 스타일)
const utils = createESMModule({
  toUpper: (s: string) => s.toUpperCase(),
  version: "1.0.0",
});

// === 4. ESM이 CJS를 import할 때 ===
// Node.js: import cjs from "pkg" → module.exports가 기본값이 됨
const cjsAsDefault = math;  // module.exports 전체가 default
console.log("ESM default ← CJS:", cjsAsDefault.add(10, 5));

// === 5. createRequire 개념 ===
function createRequire(_from: string) {
  return requireCjs;
}
const customRequire = createRequire(import.meta.url);
console.log("createRequire:", customRequire("math").PI);

// === 6. import type (런타임 제거) ===
interface MathTypes {
  add: (a: number, b: number) => number;
  PI: number;
}

// 실제 컴파일 후: import type { MathTypes }는 사라짐
const typedMath = requireCjs("math") as unknown as MathTypes;
console.log("타입 부여 후:", typedMath.add(1, 1));

// === 7. 순환 참조 (circular dependency) 경고 모델링 ===
const resolving = new Set<string>();
function requireWithCycleCheck(id: string): Exports {
  if (resolving.has(id)) {
    console.log(`경고: ${id}에서 순환 참조 발생 (부분적 로드)`);
  }
  resolving.add(id);
  const mod = requireCjs(id);
  resolving.delete(id);
  return mod;
}
requireWithCycleCheck("math");

// === 8. tree-shaking 개념 ===
// ESM은 정적 구조라 사용하지 않는 export를 제거할 수 있음
const usedExports = { ...utils };
console.log("ESM 네임스페이스:", Object.keys(usedExports).join(", "));

console.log("\n모듈 시스템 데모 완료!");

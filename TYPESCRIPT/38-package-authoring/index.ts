// 38: 패키지 제작 — 라이브러리 작성, .d.ts 배포, 버전 관리

// === 1. 라이브러리 API 정의 (배포할 타입 + 함수) ===
interface Money {
  amount: number;
  currency: string;
}

function createMoney(amount: number, currency = "KRW"): Money {
  return { amount, currency };
}

function add(a: Money, b: Money): Money {
  if (a.currency !== b.currency) throw new Error("통화 불일치");
  return { amount: a.amount + b.amount, currency: a.currency };
}

function format(m: Money): string {
  return `${m.amount.toLocaleString()} ${m.currency}`;
}

// === 2. .d.ts 생성 과정 시뮬레이션 ===
// tsc --declaration 으로 생성되는 파일 예시
const generatedDts = `
export interface Money { amount: number; currency: string; }
export declare function createMoney(amount: number, currency?: string): Money;
export declare function add(a: Money, b: Money): Money;
export declare function format(m: Money): string;
`;
console.log("=== 생성된 index.d.ts ===");
console.log(generatedDts);

// === 3. 사용자 관점 (타입 확인) ===
const m1 = createMoney(1000);
const m2 = createMoney(500, "KRW");
console.log("합계:", format(add(m1, m2)));

// === 4. SemVer 검사기 ===
type SemVer = { major: number; minor: number; patch: number };

function parseVersion(v: string): SemVer {
  const [major, minor, patch] = v.split(".").map(Number);
  return { major, minor, patch };
}

function isCompatible(v1: SemVer, v2: SemVer): boolean {
  return v1.major === v2.major;  // MAJOR가 같으면 하위 호환
}

function bump(current: SemVer, kind: "major" | "minor" | "patch"): SemVer {
  switch (kind) {
    case "major": return { major: current.major + 1, minor: 0, patch: 0 };
    case "minor": return { ...current, minor: current.minor + 1, patch: 0 };
    case "patch": return { ...current, patch: current.patch + 1 };
  }
}

const v1 = parseVersion("1.2.3");
console.log("\n현재 버전:", "1.2.3");
console.log("minor bump →", `${bump(v1, "minor").major}.${bump(v1, "minor").minor}.${bump(v1, "minor").patch}`);
console.log("major bump →", `${bump(v1, "major").major}.${bump(v1, "major").minor}.${bump(v1, "major").patch}`);
console.log("1.x.x 호환:", isCompatible(parseVersion("1.2.3"), parseVersion("1.9.0")));
console.log("1.x.x vs 2.x.x 호환:", isCompatible(parseVersion("1.2.3"), parseVersion("2.0.0")));

// === 5. 의존성 범위 해석 ===
function satisfies(range: string, version: SemVer): boolean {
  if (range.startsWith("^")) {
    const min = parseVersion(range.slice(1));
    return version.major === min.major && version >= min;
  }
  if (range.startsWith("~")) {
    const min = parseVersion(range.slice(1));
    return version.major === min.major && version.minor === min.minor;
  }
  return parseVersion(range).major === version.major;
}

console.log("\n^1.0.0 ← 1.5.0:", satisfies("^1.0.0", parseVersion("1.5.0")));
console.log("~1.2.0 ← 1.3.0:", satisfies("~1.2.0", parseVersion("1.3.0")));

// === 6. 번들 크기 추정 (tree-shaking 개념) ===
const exported = Object.keys({ createMoney, add, format });
console.log("\n내보내는 심볼:", exported.join(", "));
console.log("사용자에게 전달되는 코드: 사용한 export만 포함됨 (tree-shaking)");

console.log("\n패키지 제작 데모 완료!");

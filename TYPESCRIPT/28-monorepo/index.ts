// 28: 모노레포 — 프로젝트 레퍼런스, workspace 개념

// === 1. 모노레포 패키지 모델링 ===
interface Package {
  name: string;
  dependencies: string[];
  isLibrary?: boolean;
}

const monorepo: Record<string, Package> = {
  "packages/ui": { name: "packages/ui", dependencies: ["packages/core"] },
  "packages/core": { name: "packages/core", dependencies: [] },
  "apps/web": { name: "apps/web", dependencies: ["packages/ui", "packages/core"] },
  "apps/api": { name: "apps/api", dependencies: ["packages/core"] },
};

// === 2. 의존성 그래프에서 빌드 순서 계산 (위상 정렬) ===
function buildOrder(packages: Record<string, Package>): string[] {
  const order: string[] = [];
  const visited = new Set<string>();
  const visiting = new Set<string>();

  function visit(name: string) {
    if (visited.has(name)) return;
    if (visiting.has(name)) throw new Error(`순환 의존성: ${name}`);
    visiting.add(name);
    for (const dep of packages[name].dependencies) {
      visit(dep);
    }
    visiting.delete(name);
    visited.add(name);
    order.push(name);
  }

  for (const name of Object.keys(packages)) visit(name);
  return order;
}

const order = buildOrder(monorepo);
console.log("빌드 순서:", order);

// === 3. 변경 감지와 영향 분석 (모노레포 캐시 개념) ===
function affectedBy(changed: string): string[] {
  const affected = new Set<string>();

  function propagate(name: string) {
    for (const [pkgName, pkg] of Object.entries(monorepo)) {
      if (pkg.dependencies.includes(name) && !affected.has(pkgName)) {
        affected.add(pkgName);
        propagate(pkgName);
      }
    }
  }

  affected.add(changed);
  propagate(changed);
  return [...affected];
}

console.log("packages/ui 변경 시 영향:", affectedBy("packages/ui").join(", "));

// === 4. workspace 의존성 버전 (workspace:* 개념) ===
function resolveDependency(pkg: Package): string[] {
  return pkg.dependencies.map((dep) => {
    const depPkg = monorepo[dep];
    return `${depPkg.name}@${depPkg.isLibrary ? "workspace:*" : depPkg.name}`;
  });
}

// === 5. 패키지 관리자 스타일 의존성 트리 ===
function printTree(pkg: Package, depth = 0): string {
  const indent = "  ".repeat(depth);
  let out = `${indent}└─ ${pkg.name}\n`;
  for (const dep of pkg.dependencies) {
    out += printTree(monorepo[dep], depth + 1);
  }
  return out;
}

console.log("\n의존성 트리 (apps/web):");
console.log(printTree(monorepo["apps/web"]));

// === 6. 프로젝트 레퍼런스 빌드 계획 (tsc -b 개념) ===
function makeBuildPlan(entry: string): string[] {
  const orderList = buildOrder(monorepo);
  const plan: string[] = [];
  for (const name of orderList) {
    const pkg = monorepo[name];
    if (pkg.dependencies.includes(entry) || name === entry) plan.push(name);
  }
  return plan;
}

console.log("\napps/web 관련 빌드 계획:", makeBuildPlan("apps/web").join(" → "));
console.log("\n모노레포 데모 완료!");

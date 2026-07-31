// 29: 빌드 도구 — tsc vs esbuild/swc, tsconfig 고급

// === 1. 컴파일러 성능 비교 개념 ===
interface Compiler {
  name: string;
  typeCheck: boolean;
  speed: number; // 상대 속도
  output: (code: string) => string;
}

// tsc: 타입 체크 + 트랜스파일
const tsc: Compiler = {
  name: "tsc",
  typeCheck: true,
  speed: 1,
  output: (code) => `[tsc] ${code.replace(/:\s*\w+/, "")}`,  // 타입 주석 제거
};

// esbuild: 타입 지우기만
const esbuild: Compiler = {
  name: "esbuild",
  typeCheck: false,
  speed: 100,
  output: (code) => `[esbuild] ${code.replace(/:\s*\w+/g, "")}`,
};

// swc: 타입 지우기만 (Rust)
const swc: Compiler = {
  name: "swc",
  typeCheck: false,
  speed: 80,
  output: (code) => `[swc] ${code.replace(/:\s*\w+/g, "")}`,
};

const source = "const x: number = 42;";
console.log(tsc.output(source));
console.log(esbuild.output(source));

// === 2. 빌드 파이프라인 설계 ===
interface BuildConfig {
  compiler: Compiler;
  noEmitOnError: boolean;
  incremental: boolean;
  sourceMap: boolean;
}

function build(config: BuildConfig, files: string[]): string[] {
  if (config.compiler.typeCheck && config.noEmitOnError) {
    console.log(`[${config.compiler.name}] 타입 체크 중...`);
  }
  return files.map((f) => config.compiler.output(f));
}

const fastBuild = build(
  { compiler: esbuild, noEmitOnError: false, incremental: true, sourceMap: true },
  ["const a: string = 'x'", "let b: boolean = true"]
);
console.log("esbuild 빌드:", fastBuild);

// === 3. 증분 빌드 캐시 개념 ===
class IncrementalCache {
  private cache = new Map<string, string>();
  private dirty = new Set<string>();

  markDirty(file: string) {
    this.dirty.add(file);
  }

  get(file: string): string | undefined {
    return this.cache.get(file);
  }

  put(file: string, output: string) {
    this.cache.set(file, output);
    this.dirty.delete(file);
  }

  needsBuild(file: string): boolean {
    return this.dirty.has(file) || !this.cache.has(file);
  }
}

const cache = new IncrementalCache();
const project = ["a.ts", "b.ts", "c.ts"];

for (const file of project) {
  if (cache.needsBuild(file)) {
    cache.put(file, `compiled(${file})`);
  }
}
console.log("\n1차 빌드 완료 (모두 컴파일)");

cache.markDirty("b.ts");
for (const file of project) {
  if (cache.needsBuild(file)) {
    cache.put(file, `recompiled(${file})`);
  }
}
console.log("2차 빌드: 변경된 b.ts만 재컴파일");

// === 4. tsconfig paths 검증 ===
interface TsConfig {
  paths: Record<string, string[]>;
  baseUrl: string;
}

const tsconfig: TsConfig = {
  baseUrl: ".",
  paths: {
    "@core/*": ["src/core/*"],
    "@utils/*": ["src/utils/*"],
  },
};

function resolvePath(alias: string, config: TsConfig): string | null {
  for (const [key, targets] of Object.entries(config.paths)) {
    const pattern = key.replace("*", "(.*)");
    const match = alias.match(pattern);
    if (match) return targets[0].replace("*", match[1]);
  }
  return null;
}

console.log("\n경로 별칭:", resolvePath("@core/logger", tsconfig));
console.log("경로 별칭:", resolvePath("@utils/format", tsconfig));
console.log("\n빌드 도구 데모 완료!");

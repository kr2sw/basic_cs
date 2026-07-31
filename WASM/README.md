# WebAssembly 기초 (20개 챕터)

WebAssembly(WASM)는 브라우저와 서버에서 네이티브에 가까운 성능으로 실행되는 바이너리 명령어 형식입니다.

## 역사

WebAssembly는 2015년 W3C Community Group이 공식 출범하면서 본격적으로 개발이 시작되었습니다. Google, Microsoft, Mozilla, Apple 등 주요 브라우저 벤더가 합심하여, JavaScript만으로는 한계가 있었던 고성능 웹 애플리케이션(게임, 비디오 편집, VR/AR)을 위한 새로운 표준을 만들고자 했습니다. 2017년 3월, 최소 실행 가능 제품(MVP)이 완성되어 모든 주요 브라우저(Chrome 57, Firefox 52, Safari 11, Edge 16)에 탑재되었습니다. 2019년 W3C 표준 권고안이 되었고, 2022년에는 WASI(WebAssembly System Interface) Preview 2와 Component Model이 발표되면서 브라우저를 넘어 서버, 클라우드, 엣지 환경으로 확장되고 있습니다.

## 특징

- **네이티브 성능**: 브라우저에서 실행되는 바이너리 형식으로, JavaScript보다 10~50% 빠름
- **언어 독립성**: C/C++, Rust, Go, AssemblyScript 등 다양한 언어를 WASM으로 컴파일 가능
- **플랫폼 독립성**: 모든 주요 브라우저에서 동일하게 동작
- **보안**: 샌드박스 환경에서 실행되어 메모리 안전성 보장
- **컴팩트한 바이너리**: 효율적인 바이너리 인코딩으로 빠른 다운로드와 파싱
- **JavaScript와 상호 운용**: JS ↔ WASM 간 함수 호출, 메모리 공유 가능
- **점진적 도입**: 기존 웹 애플리케이션의 성능이 중요한 부분만 WASM으로 대체 가능

## 실행

```bash
cd WASM/01-introduction
wat2wasm add.wat -o add.wasm
npx http-server .
# http://localhost:8080 접속
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | WASM 소개, 모듈 구조, 첫 WASM 예제 |
| 02 | WAT Basics | S-Expression 문법, 섹션 구조 |
| 03 | Types & Operators | i32/i64/f32/f64, 산술/비트/비교 연산 |
| 04 | Variables | local 변수, global 변수, mut |
| 05 | Memory | 선형 메모리, load/store, data 세그먼트 |
| 06 | Functions | 함수 정의, 매개변수, 반환값, 재귀 |
| 07 | Control Flow | block/loop/if/else/br/br_if/br_table |
| 08 | Stack | 스택 머신 동작, drop/select/tee |
| 09 | Import & Export | import/export 섹션, 모듈 간 인터페이스 |
| 10 | JS Interop | JS ↔ WASM 함수 호출, 메모리 공유 |
| 11 | Call JS from WASM | import로 JS 함수 호출, 콜백 패턴 |
| 12 | Memory Management | memory.grow/copy/fill, 동적 메모리 |
| 13 | WABT Tooling | wat2wasm, wasm2wat, wasm-objdump, wasm-interp |
| 14 | Emscripten (C) | C → WASM, EMSCRIPTEN_KEEPALIVE |
| 15 | Emscripten (C++) | C++ → WASM, Embind, 클래스 바인딩 |
| 16 | Rust + WASM | wasm-pack, wasm-bindgen, cargo |
| 17 | AssemblyScript | TypeScript → WASM, AS 컴파일러 |
| 18 | Debugging | Chrome DevTools, wasm-objdump, 소스맵 |
| 19 | WASI | WebAssembly System Interface, Wasmtime |
| 20 | Real-world Project | 이미지 필터 (grayscale/invert/threshold) |
| 21 | Tables & Indirect | table, elem, call_indirect, 함수 포인터 |
| 22 | Bulk Memory | memory.copy/fill, 수동 데이터 세그먼트 |
| 23 | Reference Types | externref, funcref, ref.null |
| 24 | SIMD | v128, 128비트 벡터 연산, 정수/부동소수 |
| 25 | Multi-value | 다중값 반환, 다중 메모리 |
| 26 | Exception Handling | try/catch, throw, tag |
| 27 | Threads | shared memory, atomic 연산, worker |
| 28 | Advanced JS Interop | 공유 메모리, 객체 변환, 성능 패턴 |
| 29 | Emscripten FS | FS, IDBFS, MEMFS |
| 30 | Emscripten Advanced | pthreads, Optimize 플래그 |
| 31 | Rust + WASM Advanced | wasm-bindgen, 파서 성능 |
| 32 | AssemblyScript Advanced | 메모리 관리, 라이브러리 |
| 33 | Wasmtime/WASI Advanced | CLI, 리소스 제한 |
| 34 | Component Model | wit, 인터페이스, 합성 |
| 35 | Edge Runtime | Cloudflare Workers, 모듈 연동 |
| 36 | Plugins & Sandbox | Extism, Wasmer, 보안 격리 |
| 37 | Performance | 벤치마킹, 크기 최적화, 메모리 튜닝 |
| 38 | Advanced Debugging | DWARF, 소스맵, Chrome DevTools |
| 39 | Security | 검증, CSP, 메모리 안전 |
| 40 | Final Project | 이미지 필터/계산기 앱 |

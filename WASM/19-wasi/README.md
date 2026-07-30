# 19: WASI — WebAssembly System Interface

WASI(WebAssembly System Interface)는 WASM이 파일 시스템, 네트워크, 시계 등 시스템 리소스에 접근할 수 있게 해주는 표준 인터페이스입니다.

## WASI 런타임

```bash
# Wasmtime
curl https://wasmtime.dev/install.sh -sSf | bash

# Wasmer
curl https://get.wasmer.io -sSfL | sh

# Node.js (WASI 지원)
node --experimental-wasi-unstable-preview1 hello.wasm
```

## C → WASI 컴파일

```bash
# WASI SDK 사용
wasi-sdk/bin/clang hello.c -o hello.wasm
```

## 실행

```bash
wasmtime hello.wasm
wasmer hello.wasm
node --experimental-wasi-unstable-preview1 hello.wasm
```

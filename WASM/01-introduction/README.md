# 01: WebAssembly 소개 — 첫 WASM 모듈

WebAssembly(WASM)는 브라우저에서 네이티브에 가까운 성능으로 실행되는 바이너리 명령어 형식입니다. C, C++, Rust 등 다양한 언어를 컴파일하여 WASM으로 실행할 수 있습니다.

## WAT → WASM 컴파일

```bash
wat2wasm add.wat -o add.wasm
```

## 브라우저에서 실행

```bash
npx http-server .
# http://localhost:8080 으로 접속
```

## 주요 개념

- **모듈**: 컴파일된 WASM 바이너리 (.wasm)
- **인스턴스**: 모듈의 실행 인스턴스 (메모리 포함)
- **익스포트**: WASM이 JS에 노출하는 함수/메모리
- **임포트**: WASM이 JS로부터 받는 함수/메모리

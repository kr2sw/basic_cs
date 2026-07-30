# 14: Emscripten — C를 WASM으로 컴파일

Emscripten은 C/C++ 코드를 WebAssembly로 컴파일하는 도구 체인입니다.

## Emscripten 설치

```bash
git clone https://github.com/emscripten-core/emsdk.git
cd emsdk
emsdk install latest
emsdk activate latest
emsdk_env.bat  # Windows
source ./emsdk_env.sh  # macOS/Linux
```

## 컴파일

```bash
emcc hello.c -o hello.js -s WASM=1
emcc hello.c -o hello.html  # HTML 래퍼 포함
```

## 실행

```bash
emrun hello.html
# 또는
npx http-server .  # http://localhost:8080/hello.html
```

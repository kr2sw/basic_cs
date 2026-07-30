# 15: Emscripten — C++와 Embind

Emscripten의 Embind를 사용하면 C++ 클래스와 함수를 JavaScript에 직접 바인딩할 수 있습니다.

## 컴파일 (Embind 사용)

```bash
emcc hello.cpp -o hello.js \
  -s WASM=1 \
  -s EXPORTED_RUNTIME_METHODS='["ccall", "cwrap"]' \
  --bind
```

## 실행

```bash
npx http-server .
# http://localhost:8080/hello.html
```

## Embind 장점

- C++ 클래스를 JS 클래스로 자동 매핑
- 복잡한 객체와 배열도 자동 변환
- 메모리 관리 자동화

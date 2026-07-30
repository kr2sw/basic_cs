# 11: JS 함수를 WASM에서 호출하기

WASM 모듈은 `import`를 통해 JavaScript 함수를 임포트하여 호출할 수 있습니다. 이를 통해 WASM이 DOM 조작, 네트워크 요청, 파일 I/O 등 JS 생태계의 기능을 활용할 수 있습니다.

## 실행

```bash
wat2wasm call-js.wat -o call-js.wasm
```

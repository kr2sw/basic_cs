# 10: JS ↔ WASM 상호 운용

JavaScript와 WebAssembly 간의 함수 호출, 메모리 공유, 데이터 전달 방법을 다룹니다.

## JS에서 WASM 호출

```js
WebAssembly.instantiateStreaming(fetch('module.wasm'))
  .then(({ instance }) => {
    instance.exports.myFunction();
  });
```

## 실행

```bash
wat2wasm interop.wat -o interop.wasm
```

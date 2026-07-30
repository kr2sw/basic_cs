# 06: 함수 — 정의, 호출, 익스포트

함수는 WASM 모듈의 기본 실행 단위입니다. `(func ...)`로 정의하며, 매개변수와 반환값을 가질 수 있습니다.

## 함수 정의

```wat
(func $add (param $a i32) (param $b i32) (result i32)
  local.get $a
  local.get $b
  i32.add
)
```

## 실행

```bash
wat2wasm functions.wat -o functions.wasm
wasm-interp functions.wasm --run-all-exports
```

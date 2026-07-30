# 04: 변수 — 지역 변수와 전역 변수

## 지역 변수 (Local)

함수 내부에서 선언되며, 함수 실행 중에만 존재합니다. `local.get`/`local.set`/`local.tee`로 접근합니다.

```wat
(func $example (param $x i32) (local $y i32)
  local.get $x
  local.set $y
)
```

## 전역 변수 (Global)

모듈 전체에서 접근 가능하며, `(global ...)`로 선언합니다. `(mut)` 키워드로 변경 가능 여부를 지정합니다.

```wat
(global $count (mut i32) (i32.const 0))
(export "get_count" (func $get_count))
```

## 실행

```bash
wat2wasm vars.wat -o vars.wasm
wasm-interp vars.wasm --run-all-exports
```

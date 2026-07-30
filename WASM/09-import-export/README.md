# 09: 임포트와 익스포트

WASM 모듈은 `import`로 외부에서 함수/메모리/테이블을 받아오고, `export`로 내부 기능을 외부에 노출합니다.

## Import

```wat
(import "env" "log" (func $log (param i32)))
```

## Export

```wat
(export "my_func" (func $my_func))
```

## 실행

```bash
wat2wasm import-export.wat -o import-export.wasm
```

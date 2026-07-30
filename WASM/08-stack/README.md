# 08: 스택 — WASM 스택 머신 이해하기

WASM은 스택 기반 가상 머신입니다. 모든 연산은 스택에 값을 쌓고(push), 소비(pop)하는 방식으로 동작합니다.

## 스택 연산

```wat
i32.const 10    ;; 스택: [10]
i32.const 20    ;; 스택: [10, 20]
i32.add         ;; 스택: [30] (10+20을 계산하고 결과를 push)
```

## 실행

```bash
wat2wasm stack.wat -o stack.wasm
```

# 07: 제어 흐름 — block, loop, if/else, br

WASM의 제어 흐름은 `block`, `loop`, `if/else` 구조와 `br`, `br_if` 분기 명령어로 구성됩니다.

## 제어 구조

| 구조 | 설명 |
|------|------|
| `block $label` | 블록 (break로 탈출) |
| `loop $label` | 루프 (breack로 재시작) |
| `if ... else ... end` | 조건 분기 |
| `br $label` | 무조건 분기 |
| `br_if $label` | 조건부 분기 |

## 실행

```bash
wat2wasm control.wat -o control.wasm
wasm-interp control.wasm --run-all-exports
```

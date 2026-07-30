# 03: 타입과 연산자

## WASM 숫자 타입

| 타입 | 설명 |
|------|------|
| `i32` | 32비트 부호 있는 정수 |
| `i64` | 64비트 부호 있는 정수 |
| `f32` | 32비트 단정도 실수 (IEEE 754) |
| `f64` | 64비트 배정도 실수 (IEEE 754) |

## 산술 연산자

| 연산 | i32 | i64 | f32 | f64 |
|------|-----|-----|-----|-----|
| 덧셈 | `i32.add` | `i64.add` | `f32.add` | `f64.add` |
| 뺄셈 | `i32.sub` | `i64.sub` | `f32.sub` | `f64.sub` |
| 곱셈 | `i32.mul` | `i64.mul` | `f32.mul` | `f64.mul` |
| 나눗셈 | `i32.div_s/u` | `i64.div_s/u` | `f32.div` | `f64.div` |

## 실행

```bash
wat2wasm ops.wat -o ops.wasm
wasm-interp ops.wasm --run-all-exports
```

# 05: 메모리 — 선형 메모리와 로드/스토어

WASM의 메모리는 페이지(Page) 단위로 관리되는 선형 메모리(Linear Memory)입니다. 1페이지 = 64KB입니다.

## 메모리 연산

| 명령어 | 설명 |
|--------|------|
| `i32.load` | i32 값 읽기 |
| `i32.store` | i32 값 쓰기 |
| `i32.load8_s/u` | 8비트 부호/무부호 읽기 |
| `i32.store8` | 8비트 쓰기 |
| `memory.size` | 현재 페이지 수 반환 |
| `memory.grow` | 메모리 확장 |

## 실행

```bash
wat2wasm memory.wat -o memory.wasm
```

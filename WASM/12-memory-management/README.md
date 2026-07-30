# 12: 메모리 관리 — 확장과 데이터 세그먼트

WASM의 메모리는 페이지(Page = 64KB) 단위로 동적 확장이 가능합니다.

## 메모리 명령어

| 명령어 | 설명 |
|--------|------|
| `memory.size` | 현재 페이지 수 반환 |
| `memory.grow` | 메모리 N페이지 확장 |
| `memory.init` | 데이터 세그먼트를 메모리에 복사 |
| `memory.copy` | 메모리 영역 복사 |
| `memory.fill` | 메모리 영역 채우기 |

## 실행

```bash
wat2wasm memory-mgmt.wat -o memory-mgmt.wasm
wasm-interp memory-mgmt.wasm --run-all-exports
```

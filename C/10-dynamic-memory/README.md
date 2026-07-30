# 10: Dynamic Memory — 동적 메모리 할당

## 동적 할당 함수 (<stdlib.h>)

| 함수 | 설명 |
|------|------|
| `malloc(size)` | size 바이트 할당 (초기화 안 함) |
| `calloc(n, size)` | n개 × size 바이트 할당 (0으로 초기화) |
| `realloc(ptr, new_size)` | 크기 재조정 |
| `free(ptr)` | 메모리 해제 |

## 주의사항

- 할당 실패 시 `NULL` 반환
- 사용 후 반드시 `free()`로 해제 (메모리 누수 방지)
- 해제된 메모리 접근 금지 (댕글링 포인터)
- `free()` 후 포인터를 `NULL`로 설정 (double free 방지)

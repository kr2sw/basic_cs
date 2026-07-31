# 33: Wasmtime/WASI 고급 — CLI, 리소스 제한

WASM을 브라우저 밖에서 실행하는 대표 런타임 Wasmtime과 WASI(시스템 인터페이스)를 활용한 CLI 애플리케이션을 다룹니다.

## WASI Preview 1 syscall

WASM 모듈은 `wasi_snapshot_preview1` 네임스페이스의 함수를 import해 파일/시계/랜덤에 접근합니다.

```wat
(import "wasi_snapshot_preview1" "fd_write" (func $fd_write (param i32 i32 i32 i32) (result i32)))
(import "wasi_snapshot_preview1" "random_get" (func $random_get (param i32 i32) (result i32)))
```

## 실행

```bash
wat2wasm wasi.wat -o wasi.wasm

# 기본 실행 (진입점은 _start)
wasmtime wasi.wasm

# 특정 함수 호출
wasmtime run --invoke add wasi.wasm 3 4

# 특정 디렉터리 접근 허용 (샌드박스 기본: 파일 접근 불가)
wasmtime run --dir ./data wasi.wasm

# 환경 변수 전달
wasmtime run --env HOME=/tmp wasi.wasm
```

## 리소스 제한

| 옵션 | 설명 |
|------|------|
| `--wasm max-wasm-stack=N` | 호출 스택 상한 (재귀 깊이 제한) |
| `--wasm max-memory=N` | 메모리 상한 |
| `--dir <path>` | 허용된 디렉터리 (가상화) |
| `--env KEY=value` | 환경 변수 |
| `--max-wasm-modules` | 로드 가능한 모듈 수 |

```bash
# 512KB 스택 상한으로 재귀 폭주 방지
wasmtime run --wasm max-wasm-stack=512k wasi.wasm

# 시간 제한 (와치독 역할)
timeout 5 wasmtime run wasi.wasm
```

## AOT 컴파일

```bash
# .wasm → 네이티브 코드로 사전 컴파일 (시작 속도 향상)
wasmtime compile wasi.wasm

# 결과 cwasm 파일 실행
wasmtime run wasi.cwasm
```

## 실행

```bash
wat2wasm wasi.wat -o wasi.wasm
wasmtime wasi.wasm
```

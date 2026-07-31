# 22: 벌크 메모리 — memory.copy, memory.fill, 데이터 세그먼트

기본 WASM은 메모리를 바이트 단위로만 다루었지만, 벌크 메모리 프로포절로 메모리 블록을 통째로 채우고(`fill`) 복사하고(`copy`), 수동 데이터 세그먼트를 지연 초기화(`init`)할 수 있게 되었습니다.

## memory.fill

메모리 범위를 지정한 값으로 채웁니다. `(memory.fill $dst $value $len)` — C의 `memset`에 해당합니다.

```wat
;; addr부터 len 바이트를 0으로 채움
(func (export "zero") (param $addr i32) (param $len i32)
  (memory.fill (local.get $addr) (i32.const 0) (local.get $len)))
```

## memory.copy

한 메모리 영역을 다른 영역으로 복사합니다. 원본과 대상이 겹쳐도 안전하게 동작합니다(오버랩 보장).

```wat
;; src → dst로 len 바이트 복사
(func (export "copyBlock") (param $dst i32) (param $src i32) (param $len i32)
  (memory.copy (local.get $dst) (local.get $src) (local.get $len)))
```

## 수동 데이터 세그먼트

`(data "문자열")`처럼 오프셋 없이 선언하면 초기화 시 메모리에 올라가지 않습니다. `memory.init`으로 원하는 시점/위치에 복사할 수 있습니다.

```wat
(memory 1)
(data $msg "Hello, Bulk Memory!")   ;; 수동(패시브) 세그먼트

(func (export "loadMsg") (param $dst i32)
  (memory.init $msg (local.get $dst) (i32.const 0) (i32.const 20)))
```

## 기타 명령어

| 명령어 | 설명 |
|--------|------|
| `memory.init $seg dst src len` | 수동 세그먼트에서 메모리로 복사 |
| `data.drop $seg` | 수동 세그먼트 버림 (메모리 절약) |
| `data.size $seg` | 세그먼트 바이트 수 반환 |
| `memory.copy dst src len` | 메모리 블록 복사 (오버랩 안전) |
| `memory.fill dst val len` | 메모리 블록 채우기 |

## 실행

```bash
wat2wasm bulk.wat -o bulk.wasm
npx http-server .
```

브라우저에서 `fillZero` → `loadMsg` → `copyBlock` 순서로 호출하며 메모리 변화를 hex 뷰로 확인해보세요.

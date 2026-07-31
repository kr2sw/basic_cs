# 27: 스레드와 아토믹 — shared memory, atomic 연산, worker

WASM 스레드 프로포절은 `shared` 메모리(SharedArrayBuffer 기반)와 원자적(atomic) 메모리 연산을 제공합니다. 여러 워커가 같은 선형 메모리를 공유해 진짜 병렬 계산을 수행할 수 있습니다.

## 공유 메모리

```js
// JS에서 공유 메모리 생성 (initial/maximum + shared: true)
const memory = new WebAssembly.Memory({
  initial: 1,        // 1페이지 = 64KB
  maximum: 2,
  shared: true        // SharedArrayBuffer 사용
});
```

WAT에서는 `shared` 키워드로 선언하고 JS에서 import합니다.

```wat
(memory (export "memory") (import "env" "memory") 1 2 shared)
```

## 원자적 연산

| 명령어 | 설명 |
|--------|------|
| `i32.atomic.load` / `i32.atomic.store` | 원자적 읽기/쓰기 |
| `i32.atomic.rmw.add` | 읽고 더하기 (값 반환) |
| `i32.atomic.rmw.sub` | 읽고 빼기 |
| `i32.atomic.rmw.cmpxchg` | 비교 후 교환 (CAS) |
| `i32.atomic.wait` | 값이 일치할 때까지 대기 |
| `atomic.notify` | 대기 중인 스레드 깨우기 |

```wat
;; 카운터를 원자적으로 1 증가, 증가 전 값 반환
(func (export "increment") (param $addr i32) (result i32)
  (i32.atomic.rmw.add (local.get $addr) (i32.const 1)))
```

## wait / notify

```wat
;; $addr의 값이 $expected일 때까지 무기한 대기 (0: 대기 성공)
(func (export "waitOn") (param $addr i32) (param $expected i32) (result i32)
  (i32.atomic.wait (local.get $addr) (local.get $expected) (i64.const -1)))

;; 대기 중인 스레드 1개 깨우기
(func (export "notifyOne") (param $addr i32) (result i32)
  (atomic.notify (local.get $addr) (i32.const 1)))
```

## 워커와 공유 메모리 전달

```js
const worker = new Worker('worker.js');
worker.postMessage({ module, memory, iterations });   // 공유 Memory 전달
```

```js
// worker.js — 같은 Memory 객체로 인스턴스 생성
const { module, memory } = e.data;
const instance = new WebAssembly.Instance(module, { env: { memory } });
```

## 주의

- **보안 헤더 필수**: 브라우저에서 SharedArrayBuffer를 쓰려면 교차 출처 격리(COOP/COEP) 헤더가 필요합니다.
- **데이터 레이스**: 원자적 연산을 안 쓰면 레이스로 값이 손실됩니다(데모로 확인 가능).
- `wat2wasm`에서 `--enable-threads` 플래그가 필요합니다.

## 실행

```bash
wat2wasm atomic.wat -o atomic.wasm --enable-threads
npx http-server .
```

웹 서버가 COOP/COEP 헤더를 설정해야 워커 데모가 동작합니다(미설정 시 메인 스레드 데모만 실행).

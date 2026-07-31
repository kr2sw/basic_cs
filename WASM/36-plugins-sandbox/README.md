# 36: 플러그인/샌드박싱 — Extism, Wasmer, 보안 격리

WASM은 신뢰할 수 없는 코드(플러그인)를 안전하게 실행하는 이상적인 샌드박스입니다. Extism은 호스트 SDK를, Wasmer는 범용 런타임을 제공합니다.

## Extism: 확장 가능한 플러그인 시스템

Extism에서 플러그인은 WASM 모듈입니다. 입력은 메모리 0번부터, 출력도 메모리 0번부터 주고받는 관례(컨벤션)를 사용합니다.

```wat
(module
  ;; Extism 호스트가 제공하는 로그 함수
  (import "extism:host/env" "log" (func $log (param i64 i64)))

  (memory (export "memory") 1)

  ;; 입력: 메모리 0의 i32 → 2배로 만들어 같은 자리에 기록
  (func (export "double")
    (local $n i32)
    (local.set $n (i32.load (i32.const 0)))
    (local.set $n (i32.mul (local.get $n) (i32.const 2)))
    (i32.store (i32.const 0) (local.get $n)))
)
```

## 호스트에서 호출 (Node.js)

플러그인 관례상 입력/출력은 메모리 0번부터의 **바이트**입니다.

```js
const { createPlugin } = require('@extism/extism');

const plugin = await createPlugin({ wasm: [{ path: 'plugin.wasm' }] });

// i32 값 4를 리틀엔디언 바이트로 전달
const input = new Uint8Array([4, 0, 0, 0]);
const out = await plugin.call('double', input);
const value = new DataView(out.buffer).getInt32(0, true);
console.log('double(4) =', value);   // 8
```

## 보안 격리 원칙

| 원칙 | 설명 |
|------|------|
| **인터페이스 최소화** | 파일/네트워크 등 권한을 호스트가 제어 |
| **메모리 분리** | 플러그인마다 독립 메모리 (충돌/오염 차단) |
| **리소스 상한** | 메모리/시간/스택 제한으로 악성 루프 차단 |
| **무결성 검증** | 서명/해시로 플러그인 원본 확인 |

## Wasmer JS

```js
import { Wasmer } from '@wasmer/sdk';

const wasmer = await Wasmer.init();
const instance = await wasmer.instantiate('plugin.wasm');
const result = await instance.exports.double?.(4);
```

## 메모리 상한 예 (WAT)

```wat
(memory (export "memory") 1 1)   ;; 초기 1페이지, 최대 1페이지 — 확장 차단
```

## 실행

```bash
wat2wasm plugin.wat -o plugin.wasm
node host.js

# Wasmer CLI로 단독 실행
wasmer run plugin.wasm --invoke double
```

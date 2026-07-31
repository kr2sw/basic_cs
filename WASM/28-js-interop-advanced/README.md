# 28: 고급 JS 연동 — 공유 메모리, 객체 변환, 성능 패턴

WASM은 기본적으로 숫자와 메모리만 이해합니다. JS 객체나 문자열은 바이트로 인코딩해 메모리에 쓰고, 결과도 메모리에서 바이트로 읽는 "경계 변환"이 필요합니다. 이번 장에서는 실전에서 쓰는 변환 패턴과 성능 팁을 다룹니다.

## 메모리 공유 (zero-copy)

JS의 `TypedArray`와 WASM 메모리는 같은 바이트를 바라봅니다. 복사 없이 직접 읽고 쓸 수 있습니다.

```js
const wasmMemory = instance.exports.memory;
const view = new Int32Array(wasmMemory.buffer);

// WASM 함수가 메모리에서 직접 변환 → JS에서 그대로 읽기
instance.exports.transform(0, 100);   // view[0..99]가 즉시 반영됨
```

## 문자열 변환

WASM은 UTF-8 바이트만 알므로, JS에서 `TextEncoder`/`TextDecoder`로 변환합니다.

```js
// JS → WASM: 문자열을 메모리에 인코딩 후 포인터/길이 전달
const bytes = new TextEncoder().encode('안녕, WASM!');
new Uint8Array(memory.buffer).set(bytes, ptr);
wasm.exports.rememberString(ptr, bytes.length);

// WASM → JS: 포인터/길이를 받아 디코딩
const out = new TextDecoder().decode(
  new Uint8Array(memory.buffer, wasm.exports.helloPtr(), 16));
```

## 객체 변환

구조화된 객체는 그대로 넘길 수 없으므로 **직렬화(JSON/바이트)**를 거칩니다.

```js
const json = JSON.stringify({ type: 'user', id: 3 });
// → 인코딩 → 메모리에 기록 → WASM 파싱 함수 호출 → 결과 읽기
```

## 성능 패턴

| 패턴 | 설명 |
|------|------|
| zero-copy | `postMessage`/`JSON.stringify` 대신 메모리 직접 접근 |
| 배칭 | 함수 호출당 1바이트 대신 버퍼 단위로 처리 |
| 타입 안 맞는 뷰 금지 | `memory.buffer`는 grow 시 detach됨 → 재획득 필수 |
| 결과 재사용 | 매 호출마다 할당하지 말고 출력 버퍼 재사용 |
| JS 루프 대신 WASM 루프 | 10만 개 이상 루프는 WASM 쪽이 빠름 |

## 실행

```bash
wat2wasm interop.wat -o interop.wasm
npx http-server .
```

브라우저에서 문자열 왕복, zero-copy 변환, 벤치마크를 실행해보세요.

# 32: AssemblyScript 심화 — 메모리 관리, 라이브러리

AssemblyScript(AS)는 TypeScript 문법을 WASM으로 컴파일하므로, JS에 없는 **포인터 개념**을 직접 다뤄야 하는 경우가 많습니다. 이번 장에서는 `store`/`load`, `changetype`, 런타임 힙 관리를 배웁니다.

## 타입과 포인터

```ts
export function write(ptr: usize, value: i32): void {
  store<i32>(ptr, value);       // 메모리 주소 ptr에 i32 저장
}

export function read(ptr: usize): i32 {
  return load<i32>(ptr);        // 주소 ptr에서 i32 로드
}
```

| AS 타입 | WASM 타입 | 비고 |
|---------|-----------|------|
| `i32`/`u32` | `i32` | 32비트 정수 |
| `usize` | `i32`/`i64` | 주소 용도 (플랫폼별) |
| `f32`/`f64` | `f32`/`f64` | 부동소수 |
| `ArrayBuffer` | — | 힙 위의 연속 메모리 |
| `string` | — | UTF-16 내부 표현 |

## 힙 할당

```ts
// 힙에 n바이트 버퍼를 만들고 그 시작 주소 반환
export function alloc(n: i32): usize {
  return changetype<usize>(new ArrayBuffer(n));
}
```

`new ArrayBuffer()`는 WASM 힙(메모리)에 할당됩니다. `--exportRuntime`으로 빌드하면 `__new`/`__free`/`memory`가 런타임에 포함됩니다.

## 런타임 메모리 export

```bash
npx asc assembly/index.ts --outFile build/module.wasm --exportRuntime
```

이러면 JS에서 `__new`, `__free`, `memory.grow` 등을 그대로 쓸 수 있습니다.

## 객체를 포인터로

클래스 인스턴스도 `changetype<usize>`로 주소로 변환해 다룰 수 있습니다.

```ts
export function makeCounter(): usize {
  return changetype<usize>(new Counter());
}

export function bump(ptr: usize): i32 {
  return changetype<Counter>(ptr).increment();
}
```

## 문자열 ↔ UTF-8

`String.UTF8.encode/decode`로 JS와 주고받을 바이트를 만들 수 있습니다.

```ts
export function utf8Encode(s: string): ArrayBuffer {
  return String.UTF8.encode(s);
}
```

## 최적화 플래그

```bash
npx asc assembly/index.ts --outFile build/module.wasm \
  --optimize --shrinkLevel 1 --noAssert --runtime stub
```

- `--optimize`: 최적화 활성화
- `--shrinkLevel 1~3`: 크기 축소 수준
- `--runtime stub`: 경량 런타임 (GC 없음, 크기 최소화)
- `--noAssert`: 검사 코드 제거

## 실행

```bash
npm install
npm run build
npx http-server .
```

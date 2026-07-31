# 26: 예외 처리 — try/catch, throw, tag

Exception Handling 프로포절은 WASM에 구조적 예외를 추가합니다. `tag`로 예외 타입을 선언하고, `throw`로 던지고, `try/catch`로 잡습니다. WASM 경계를 넘어 JS에도 `WebAssembly.Exception`으로 전달됩니다.

## tag 선언

```wat
;; 매개변수 i32를 운반하는 예외 타입
(tag $divByZero (param i32))
```

## throw / try/catch

```wat
(try
  (do
    (if (i32.eqz (local.get $b))
      (then (throw $divByZero (local.get $b))))   ;; 예외 던지기
    (i32.div_s (local.get $a) (local.get $b)))     ;; 정상 경로
  (catch $divByZero
    (drop)                                         ;; 예외 payload 버림
    (i32.const -1))                                ;; 대체값
  (catch_all
    (i32.const -2)))                               ;; 모든 예외 처리
```

- `(catch $tag ...)`: 특정 태그만 처리, 스택에 payload가 먼저 놓입니다.
- `(catch_all ...)`: 태그와 무관하게 모든 예외 처리.
- `(delegate $label)`: 예외를 바깥 `try` 레이블로 전달.

## JS로 전달

WASM에서 잡지 않은 예외는 JS에서 `WebAssembly.Exception`으로 나타납니다.

```js
try {
  wasm.exports.panic();
} catch (e) {
  console.log(e instanceof WebAssembly.Exception); // true
}
```

## 주의

- 예외는 성능에 부담이 있으므로 **정상 흐름 제어**에는 사용하지 않는 것이 좋습니다.
- `wat2wasm`에서 `--enable-exceptions` 플래그가 필요하며, 브라우저 지원(Chrome은 기본 활성화)이 필요합니다.

## 실행

```bash
wat2wasm exceptions.wat -o exceptions.wasm --enable-exceptions
npx http-server .
```

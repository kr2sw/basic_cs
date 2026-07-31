# 39: 보안 — 검증, CSP, 메모리 안전

WASM은 샌드박스(격리된 메모리 + 명시적 임포트)라는 보안 토대를 제공하지만, 배포 환경에서 추가 보안 조치가 필요합니다.

## 검증 (Validation)

브라우저는 인스턴스화 전에 모듈을 검증합니다. 형식이 잘못되면 `WebAssembly.CompileError`로 거부됩니다.

```bash
wasm-validate module.wasm   # 성공 시 조용히, 실패 시 오류 출력
```

- 정적 타입 검사(스택 타입 일치), 경계 검사, 제어 흐름 규칙
- 악의적인 모듈도 검증을 통과해야만 실행됨

## 메모리 안전

WASM의 모든 `load`/`store`는 **경계 검사**를 거칩니다. 범위를 벗어나면 네이티브처럼 메모리 파괴 대신 **트랩**이 발생합니다.

```wat
;; 메모리(1페이지 = 65536바이트) 밖 접근 → i32.load8_u 트랩
(func (export "readByte") (param $i i32) (result i32)
  (i32.load8_u (local.get $i)))
```

```js
try {
  wasm.exports.readByte(99999);   // OOB
} catch (e) {
  // RuntimeError: memory access out of bounds
}
```

## CSP (Content Security Policy)

CSP로 wasm 로딩을 제한합니다.

```bash
# HTTP 헤더: 같은 출처의 wasm만 허용
Content-Security-Policy: default-src 'self'; script-src 'self' 'wasm-unsafe-eval'; worker-src 'self'
```

- `wasm-unsafe-eval` 키워드로 `WebAssembly.compile`/`instantiate` 제어
- 인라인 스크립트, 원격 스크립트 차단

## 교차 출처 격리 (Spectre 방어)

SharedArrayBuffer·고정밀 타이머는 교차 출처 격리에서만 활성화됩니다.

```bash
Cross-Origin-Opener-Policy: same-origin
Cross-Origin-Embedder-Policy: require-corp
```

## 기타 조치

| 항목 | 설명 |
|------|------|
| SRI | `integrity="sha384-..."`로 wasm 무결성 보장 |
| 최소 권한 임포트 | 필요한 함수만 import/export |
| 런타임 상한 | 메모리 max, 스택 제한 (와치독) |
| WASI 샌드박스 | `wasmtime --dir`로 접근 경로 제한 |
| 서명/감사 | 플러그인 무결성 + 서드파티 감사 |

## 실행

```bash
wat2wasm sandbox.wat -o sandbox.wasm
npx http-server .
```

범위 밖 접근이 트랩으로 차단되는 것을 확인해보세요.

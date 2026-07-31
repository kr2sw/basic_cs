# 35: 엣지 런타임 — Cloudflare Workers, 모듈 연동

엣지 런타임(Cloudflare Workers, Fastly Compute, Deno, Bun)은 전 세계 PoP에서 WASM을 실행합니다. 요청마다 콜드 스타트가 중요하므로 작고 빠른 WASM 모듈이 이상적입니다.

## Cloudflare Workers에서 WASM 사용

Workers는 `wrangler.toml`의 `[wasm_modules]`로 wasm 파일을 모듈로 등록합니다.

```toml
name = "edge-add"
main = "worker.js"
compatibility_date = "2024-01-01"

[wasm_modules]
add = "./add.wasm"
```

worker.js에서 import하면 `WebAssembly.Module` 객체를 받습니다.

```js
import addModule from './add.wasm';

export default {
  async fetch(request) {
    const url = new URL(request.url);
    const a = parseInt(url.searchParams.get('a') ?? '0', 10);
    const b = parseInt(url.searchParams.get('b') ?? '0', 10);

    // WebAssembly.Module이면 인스턴스화
    const wasm = addModule instanceof WebAssembly.Module
      ? new WebAssembly.Instance(addModule).exports
      : addModule;

    return Response.json({ a, b, sum: wasm.add(a, b) });
  }
};
```

## WAT 소스

```wat
(module
  (func (export "add") (param $a i32) (param $b i32) (result i32)
    (i32.add (local.get $a) (local.get $b)))
)
```

## 엣지 환경 제약

- **파일 시스템 없음**: 저장은 KV/DO 등 외부 스토리지
- **짧은 실행 시간**: Workers는 CPU 시간 상한(기본 10ms/10s)이 있음
- **작은 모듈 선호**: 다운로드 크기가 콜드 스타트에 영향
- **동기 임포트 제한**: `fetch`는 `await` 필요

## 실행

```bash
wat2wasm add.wat -o add.wasm
npx wrangler dev          # 로컬 실행
npx wrangler deploy       # 배포

curl "http://localhost:8787/?a=40&b=2"
# {"a":40,"b":2,"sum":42}
```

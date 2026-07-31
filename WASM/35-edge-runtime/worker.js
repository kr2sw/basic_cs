// Cloudflare Worker — WASM 모듈과 연동
import addModule from './add.wasm';

export default {
  async fetch(request, env, ctx) {
    const url = new URL(request.url);
    const path = url.pathname;

    if (path === '/compute') {
      const a = parseInt(url.searchParams.get('a') ?? '0', 10);
      const b = parseInt(url.searchParams.get('b') ?? '0', 10);

      // wrangler가 내려주는 WebAssembly.Module이면 인스턴스화
      const wasm = addModule instanceof WebAssembly.Module
        ? new WebAssembly.Instance(addModule).exports
        : addModule;

      return Response.json({
        a,
        b,
        sum: wasm.add(a, b),
        mul: wasm.mul(a, b),
      });
    }

    if (path === '/classify') {
      const code = parseInt(url.searchParams.get('code') ?? '200', 10);
      const wasm = addModule instanceof WebAssembly.Module
        ? new WebAssembly.Instance(addModule).exports
        : addModule;
      const kind = wasm.classify(code);
      const label = ['ok', 'client error', 'server error'][kind];
      return Response.json({ code, kind, label });
    }

    return Response.json({ hello: 'wasm at the edge' });
  },
};

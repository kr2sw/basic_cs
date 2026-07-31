# 37: 성능 최적화 — 벤치마킹, 크기 최적화, 메모리 튜닝

WASM 성능 최적화는 "측정 없이 최적화하지 말 것"이 원칙입니다. 반복 가능한 벤치마크를 만들고, 크기와 메모리를 함께 고려해야 합니다.

## 벤치마킹

```js
const t0 = performance.now();
const result = wasm.exports.fib(30);
const elapsed = performance.now() - t0;
```

- **워밍업**: JIT 최적화를 위해 여러 번 실행 후 측정
- **반복**: 짧은 호출은 오버헤드가 크므로 배치로 반복
- **조건 통제**: 같은 데이터, 같은 스레드, GC 영향 배제

## 크기 최적화

```bash
# Binaryen 최적화 + 불필요한 이름 제거
wasm-opt module.wasm -O3 -o module.opt.wasm
wasm-strip module.opt.wasm

# C/C++ (Emscripten) 크기 위주 플래그
emcc src.c -o src.wasm -Oz --strip-all -sALLOW_MEMORY_GROWTH=0

# 크기 확인
wasm-objdump -h module.wasm
```

| 기법 | 효과 |
|------|------|
| `-O3` (wasm-opt) | 코드 크기 + 속도 개선 |
| 이름 제거 | 섹션 크기 감소 |
| 함수 인라이닝 | 호출 오버헤드 감소 |
| 공통 코드 제거 | 중복 감소 |

## 메모리 튜닝

```wat
;; 초기 1페이지, 최대 8페이지 — 초기 크기를 실제 사용량에 맞춤
(memory (export "memory") 1 8)
```

- **초기 크기**: 너무 크면 시작 느림, 너무 작으면 `memory.grow` 오버헤드
- **재할당 피하기**: grow는 페이지(64KB) 단위라 커질수록 유리
- **버퍼 재사용**: JS에서 매번 `new Uint8Array` 대신 출력 버퍼 재사용

## 핫 루프 패턴

- SIMD(`v128`)로 데이터 병렬 작업 처리
- `memory.copy/fill`로 벌크 연산 사용
- 작은 함수는 인라인 (호출 오버헤드 절감)
- 조건부 분기보다 산술 연산 선호 (브랜치 예측)

## 실행

```bash
wat2wasm bench.wat -o bench.wasm
npx http-server .
```

브라우저에서 WASM/JS 벤치마크를 반복 실행하고, 최적화 여부에 따른 차이를 관찰해보세요.

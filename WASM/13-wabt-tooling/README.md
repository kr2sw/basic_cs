# 13: WABT 도구 — wat2wasm, wasm2wat, wasm-objdump

WABT(WebAssembly Binary Toolkit)는 WASM 바이너리와 텍스트 형식 간 변환 및 분석 도구 모음입니다.

## 주요 도구

### wat2wasm — WAT → WASM 변환
```bash
wat2wasm input.wat -o output.wasm
wat2wasm input.wat -o output.wasm --debug-names  # 디버그 이름 유지
```

### wasm2wat — WASM → WAT 디컴파일
```bash
wasm2wat input.wasm -o output.wat
wasm2wat input.wasm --generate-names  # 이름 자동 생성
```

### wasm-objdump — WASM 바이너리 분석
```bash
wasm-objdump -x module.wasm    # 상세 정보
wasm-objdump -d module.wasm    # 디스어셈블리
wasm-objdump -h module.wasm    # 헤더 정보
```

### wasm-validate — 유효성 검사
```bash
wasm-validate module.wasm
```

### wasm-interp — CLI에서 직접 실행
```bash
wasm-interp module.wasm --run-all-exports
```

## 실습

```bash
wat2wasm demo.wat -o demo.wasm
wasm-objdump -x demo.wasm
wasm2wat demo.wasm -o roundtrip.wat
```

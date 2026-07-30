# 18: 디버깅 — WASM 디버깅 기법

## 브라우저 DevTools

Chrome DevTools는 WASM 디버깅을 지원합니다:

1. **Sources 패널**에서 WASM 파일 열기
2. **WAT 형식으로 디스어셈블리 표시**
3. **브레이크포인트 설정** 가능
4. **스택 추적** 및 변수 검사

## wasm-objdump

바이너리 정보 확인:

```bash
wasm-objdump -x module.wasm    # 섹션 정보
wasm-objdump -d module.wasm    # 디스어셈블리
```

## wasm2wat

바이너리를 WAT으로 변환:

```bash
wasm2wat module.wasm --generate-names
```

## 실행

```bash
wat2wasm debug.wat -o debug.wasm --debug-names
```

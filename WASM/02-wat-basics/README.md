# 02: WAT 기초 — S-Expression과 모듈 구조

WAT(WebAssembly Text Format)는 WASM 바이너리를 사람이 읽을 수 있는 텍스트 형식으로 표현합니다. S-Expression(괄호로 감싼 트리 구조) 문법을 사용합니다.

## 모듈 구조

모든 WAT 파일은 `(module ... )`로 시작합니다. 모듈 내부에는 섹션들이 위치합니다:

- **func**: 함수 정의
- **memory**: 메모리 정의
- **table**: 테이블 정의
- **global**: 전역 변수 정의

## 실행

```bash
wat2wasm module.wat -o module.wasm
wasm-objdump -x module.wasm  # 구조 확인
```

## 연습

`module.wat`를 수정하여 새 함수를 추가하고 컴파일해보세요.

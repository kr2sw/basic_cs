# 38: 고급 디버깅 — DWARF, 소스맵, 크롬 DevTools

WASM 디버깅은 소스 언어(C/Rust)에 따라 도구가 다릅니다. 바이너리 레벨 디버깅부터 DWARF를 활용한 소스 레벨 디버깅까지 알아봅니다.

## 네임 섹션 (WAT)

WAT의 함수명은 `--name-section`으로 이진에 남아 DevTools에서 읽기 쉽습니다.

```bash
wat2wasm debug.wat -o debug.wasm --debug-names
wasm-objdump -x debug.wasm | grep -A 20 "names section"
```

## 크롬 DevTools

1. **Sources → Page** 에서 `.wasm` 열기 (소스맵 없으면 WAT 뷰)
2. **Breakpoint**: 함수 호출 라인 클릭
3. **Call Stack / Scope / Stepping**: 로컬 변수·스택 값 확인
4. **Console에서 직접 호출**: `wasm.exports.factorial(5)`

## DWARF 기반 소스 레벨 디버깅 (C)

`-g`로 DWARF 정보를 포함하면 원본 C 코드에서 단계 실행이 가능합니다.

```bash
# Emscripten: 디버그 정보 + 소스맵 포함
emcc debug.c -o debug.html -g -gsource-map

# Wasmtime CLI: DWARF 디버깅
wasmtime run --invoke main debug.wasm
```

DevTools에서 `-gsource-map`이 생성한 `.map` 파일을 로드해 C 코드로 디버깅합니다.

## wasm-objdump

```bash
wasm-objdump -d debug.wasm    # 디스어셈블리
wasm-objdump -x debug.wasm    # 섹션 구조/임포트/내보내기
wasm-objdump -s debug.wasm    # 데이터 세그먼트
```

## 기타 도구

| 도구 | 용도 |
|------|------|
| `wasm-decompile debug.wasm` | C-유사 의사코드 출력 |
| `wasm-validate debug.wasm` | 모듈 검증 (에러 위치 표시) |
| `wasm2wat debug.wasm` | 이진 → WAT |
| `wasm-strip debug.wasm` | 디버그 정보 제거 (배포용) |
| `wasm-opt -g` | 디버그 정보 유지한 채 최적화 |

## trap 원인 확인

트랩(비정상 종료) 시 DevTools 콜스택에서 실패한 명령과 스택 값을 확인할 수 있습니다.

## 실행

```bash
wat2wasm debug.wat -o debug.wasm --debug-names
npx http-server .
```

# 00 개발환경 설정

## 필수 도구

- **WABT** (WebAssembly Binary Toolkit) - WAT ↔ WASM 변환
- **Node.js** 18 이상 - 로컬 HTTP 서버 실행
- **최신 웹 브라우저** (Chrome, Firefox, Edge) - WASM 실행
- **Emscripten** (선택, C/C++ → WASM 컴파일)
- **Rust + wasm-pack** (선택, Rust → WASM)

## WABT 설치

### Windows (scoop)
```bash
scoop install wabt
```

### macOS
```bash
brew install wabt
```

### Linux / 직접 빌드
```bash
# GitHub Releases에서 바이너리 다운로드
# https://github.com/WebAssembly/wabt/releases
# 또는 소스 빌드
git clone https://github.com/WebAssembly/wabt.git
cd wabt && mkdir build && cd build
cmake .. && cmake --build .
```

### 주요 WABT 도구

| 명령어 | 설명 |
|--------|------|
| `wat2wasm` | WAT → WASM 변환 |
| `wasm2wat` | WASM → WAT 변환 |
| `wasm-objdump` | WASM 바이너리 정보 출력 |
| `wasm-validate` | WASM 유효성 검사 |
| `wasm-interp` | WASM 인터프리터 (CLI에서 직접 실행) |

## WAT 컴파일 및 실행

```bash
# WAT → WASM 컴파일
wat2wasm add.wat -o add.wasm

# WASM → WAT 디컴파일
wasm2wat add.wasm

# WASM 검증
wasm-validate add.wasm

# WASM 정보 확인
wasm-objdump -x add.wasm
```

## 로컬 HTTP 서버 (WASM 로딩 필수)

WASM은 `file://` 프로토콜에서 로드되지 않으므로 HTTP 서버가 필요합니다.

```bash
# Node.js (http-server)
npx http-server .

# Python
python -m http.server 8000

# VS Code 확장 (Live Server)
```
브라우저에서 `http://localhost:8000` 접속

## VS Code 확장

- **WebAssembly Toolkit** (watt) - WAT 문법 강조
- **Live Server** - 로컬 HTTP 서버

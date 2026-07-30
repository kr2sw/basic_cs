# 00 개발환경 설정

## 필수 도구

- **rustup** - Rust 설치 및 버전 관리 (https://rustup.rs)
- **rustc** - Rust 컴파일러
- **cargo** - 패키지 매니저 및 빌드 도구

## Rust 설치

### Windows
1. https://rustup.rs 방문
2. `rustup-init.exe` 다운로드 및 실행
3. 설치 과정에서 **Visual Studio Build Tools** 필요 시 함께 설치

### Windows (scoop)
```bash
scoop install rustup
rustup default stable
```

### macOS / Linux
```bash
curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | sh
```

### 설치 확인
```bash
rustc --version
cargo --version
rustup --version
```

## cargo 명령어

```bash
# 새 프로젝트 생성
cargo new hello_world
cd hello_world

# 빌드
cargo build

# 릴리스 빌드
cargo build --release

# 실행
cargo run

# 컴파일만 확인 (실행 없음)
cargo check

# 테스트
cargo test

# 문서 생성
cargo doc --open
```

## VS Code 확장

- **rust-analyzer** - IntelliSense, 타입 검사
- **CodeLLDB** - 디버깅
- **Even Better TOML** - Cargo.toml 파일 지원
- **crates** - 의존성 버전 관리

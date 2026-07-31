# 38: CLI 애플리케이션 — clap 개념, 인자 파싱 직접 구현

명령줄 인자를 파싱하는 CLI 앱을 만듭니다. 실제로는 `clap` 크레이트를 쓰지만, 여기서는 직접 구현해 원리를 이해합니다.

## clap 개념 (외부 크레이트)

```rust
// Cargo.toml
// clap = { version = "4", features = ["derive"] }

#[derive(Parser)]
struct Args {
    #[arg(short, long)]
    verbose: bool,
    input: String,
}
```

## 본 챕터 구현

`--flag`, `--key value`, `-k value`, positional 인자를 직접 파싱하는 미니 CLI 프레임워크와 간단한 `todo` 명령어를 만듭니다.

## 실행

```bash
cd RUST/38-cli-app
cargo run -- add "할 일"
cargo run -- list
cargo run -- done 1
cargo run -- --help
```

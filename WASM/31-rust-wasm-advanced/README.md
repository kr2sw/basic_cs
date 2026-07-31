# 31: Rust + WASM 심화 — wasm-bindgen, 파서 성능

기본 과정의 `16-rust-wasm`에서 `wasm-pack`으로 간단한 함수를 내보냈다면, 이번에는 `wasm-bindgen`의 타입 매핑과 실용적인 파서를 다룹니다.

## Cargo.toml

```toml
[package]
name = "parser-wasm"
version = "0.1.0"
edition = "2021"

[lib]
crate-type = ["cdylib", "rlib"]

[dependencies]
wasm-bindgen = "0.2"
js-sys = "0.3"
```

## 타입 매핑

`wasm-bindgen`은 인자/반환 타입을 자동으로 매핑합니다.

| Rust 타입 | JS 타입 |
|-----------|---------|
| `String`, `&str` | `string` |
| `Vec<u8>`, `&[u8]` | `Uint8Array` |
| `Vec<u32>` | `Uint32Array` |
| `js_sys::Object` | `object` |
| `bool`, `f64`, `u32` | `boolean`, `number` |

```rust
#[wasm_bindgen]
pub fn parse_ints(input: &str) -> Vec<u32> {
    input
        .split(|c: char| !c.is_ascii_digit())
        .filter(|s| !s.is_empty())
        .filter_map(|s| s.parse().ok())
        .collect()
}
```

```js
const arr = parse_ints("a1 b22 c333");   // Uint32Array [1, 22, 333]
```

## 객체 변환

`js_sys::Reflect`로 JS 객체를 만들 수 있습니다.

```rust
#[wasm_bindgen]
pub fn parse_key_value(input: &str) -> js_sys::Object {
    let obj = js_sys::Object::new();
    for line in input.lines() {
        let mut parts = line.splitn(2, ':');
        if let (Some(k), Some(v)) = (parts.next(), parts.next()) {
            js_sys::Reflect::set(&obj,
                &JsValue::from_str(k.trim()),
                &JsValue::from_str(v.trim())).ok();
        }
    }
    obj
}
```

## 파서 성능

- **문자열 경계 최소화**: `&str`은 복사 없이 전달됩니다. 매 호출 큰 문자열을 만드는 것보다 재사용이 빠릅니다.
- **할당 줄이기**: 결과 `Vec`은 한 번에 반환합니다.
- **비교**: 순수 JS `split/map`과 WASM 파서의 차이를 `performance.now()`로 측정해보세요. 긴 입력일수록 WASM이 유리합니다.

## 빌드 및 실행

```bash
wasm-pack build --target web
npx http-server .
```

`pkg/`의 JS와 wasm이 `www/index.html`에서 ES 모듈로 로드됩니다.

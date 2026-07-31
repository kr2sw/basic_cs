// wit-bindgen이 add.wit 기반의 Guest 트레이트를 생성
wit_bindgen::generate!({
    path: "add.wit",
    world: "calculator",
});

struct Calculator;

// wit 인터페이스 구현
impl Guest for Calculator {
    fn add(a: u32, b: u32) -> u32 {
        a + b
    }

    fn sub(a: u32, b: u32) -> u32 {
        a - b
    }

    fn greet(name: String) -> String {
        format!("Hello, {name}!")
    }
}

// 컴포넌트 진입점 export
export!(Calculator);

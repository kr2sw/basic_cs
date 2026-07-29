// math/mod.rs - math 서브모듈

// 비공개 함수 (모듈 내부에서만 접근)
fn add_private(x: i32, y: i32) -> i32 {
    x + y
}

// pub: 공개 함수
pub fn add(x: i32, y: i32) -> i32 {
    add_private(x, y) // 내부 함수 호출 가능
}

pub fn subtract(x: i32, y: i32) -> i32 {
    x - y
}

// pub(crate): 현재 크레이트 내에서만 접근 가능
pub(crate) fn multiply(x: i32, y: i32) -> i32 {
    x * y
}

// 서브 서브모듈 (advanced)
pub mod advanced {
    // pub(super): 부모 모듈(math)에서 접근 가능
    #[allow(dead_code)]
    pub(super) fn internal_helper() -> i32 {
        42
    }

    pub fn factorial(n: u64) -> u64 {
        if n <= 1 {
            1
        } else {
            n * factorial(n - 1)
        }
    }

    pub fn power(base: i32, exp: u32) -> i32 {
        base.pow(exp)
    }
}

// 재익스포트: advanced::factorial을 math::factorial로 사용 가능
pub use advanced::factorial as my_factorial;

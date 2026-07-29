// 17-unsafe-rust
// 안전하지 않은 Rust: raw 포인터, unsafe 함수, extern, 가변 정적 변수

// --- 가변 정적 변수 (mutable static) ---
static mut COUNTER: u32 = 0;

// --- unsafe 트레이트와 구현 ---
unsafe trait UnsafeTrait {
    fn unsafe_method(&self);
}

unsafe impl UnsafeTrait for i32 {
    fn unsafe_method(&self) {
        println!("UnsafeTrait 구현 for i32: {}", self);
    }
}

// --- extern "C" 함수 (C ABI 호출 규약) ---
unsafe extern "C" {
    // C 표준 라이브러리 abs 함수 선언
    fn abs(input: i32) -> i32;
    // C 표준 라이브러리 strlen 함수 선언
    fn strlen(s: *const u8) -> usize;
}

// --- FFI로 안전한 래퍼 만들기 ---
mod c_ffi {
    // C ABI로 호출 가능한 함수
    #[unsafe(no_mangle)]
    pub extern "C" fn rust_add(a: i32, b: i32) -> i32 {
        a + b
    }
}

// --- 안전한 추상화로 unsafe 감싸기 ---
fn split_at_mut_safe<T>(slice: &mut [T], mid: usize) -> (&mut [T], &mut [T]) {
    let len = slice.len();
    assert!(mid <= len);

    // 안전한 방법: split_at_mut 사용
    slice.split_at_mut(mid)
}

fn split_at_mut_unsafe<T>(slice: &mut [T], mid: usize) -> (&mut [T], &mut [T]) {
    let len = slice.len();
    let ptr = slice.as_mut_ptr();

    assert!(mid <= len);

    unsafe {
        (
            std::slice::from_raw_parts_mut(ptr, mid),
            std::slice::from_raw_parts_mut(ptr.add(mid), len - mid),
        )
    }
}

fn main() {
    // --- Raw 포인터 (1. 생성) ---
    let mut num = 42;

    // 불변 raw 포인터
    let r1 = &num as *const i32;
    // 가변 raw 포인터
    let r2 = &mut num as *mut i32;

    // raw 포인터는 unsafe 블록 밖에서 생성 가능
    // 하지만 역참조(dereference)는 unsafe 블록 내에서만 가능
    println!("Raw 포인터 생성 완료: {:?}, {:?}", r1, r2);

    // --- Raw 포인터 (2. 역참조) ---
    unsafe {
        println!("r1이 가리키는 값: {}", *r1);
        println!("r2가 가리키는 값: {}", *r2);

        // 가변 포인터로 값 변경
        *r2 = 100;
        println!("변경 후 r1: {}", *r1);
    }

    // --- 임의의 메모리 주소 (위험!) ---
    // 실제로 실행하면 segfault 발생 가능
    // let address = 0x012345usize;
    // let r = address as *const i32;
    // unsafe {
    //     println!("{}", *r); // 정의되지 않은 동작!
    // }

    // --- unsafe 함수 호출 ---
    unsafe {
        println!("abs(-5) = {}", abs(-5));

        let s = "Hello, Rust!\0";
        let len = strlen(s.as_ptr());
        println!("strlen = {}", len);
    }

    // --- FFI로 노출한 rust_add 호출 ---
    // (다른 C 프로그램에서 호출 가능)
    let result = c_ffi::rust_add(3, 7);
    println!("rust_add(3, 7) = {}", result);

    // --- 가변 정적 변수 ---
    unsafe {
        COUNTER += 1;
        // Rust 2024에서는 직접 참조 대신 raw 포인터로 접근
        let ptr = std::ptr::addr_of!(COUNTER);
        println!("COUNTER = {}", std::ptr::read(ptr));
    }

    // --- unsafe 트레이트 ---
    let value = 42i32;
    value.unsafe_method();

    // --- split_at_mut (안전 vs unsafe) ---
    let mut arr = [1, 2, 3, 4, 5, 6];

    // 안전한 버전
    let (left, right) = split_at_mut_safe(&mut arr, 3);
    println!("safe left: {:?}, right: {:?}", left, right);

    // unsafe 버전
    let (left, right) = split_at_mut_unsafe(&mut arr, 3);
    println!("unsafe left: {:?}, right: {:?}", left, right);

    // --- 인라인 어셈블리 (추가 고급) ---
    // x86_64에서만 동작
    #[cfg(target_arch = "x86_64")]
    unsafe {
        use std::arch::asm;
        let mut x: u64 = 5;
        asm!("add {0}, 10", inout(reg) x);
        println!("인라인 어셈블리 결과: {}", x);
    }

    println!("unsafe Rust 예제 완료");
}

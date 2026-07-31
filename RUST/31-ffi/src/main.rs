// 31: FFI — extern "C", unsafe, C 바인딩 개념
//
// 실제 FFI는 extern "C" + unsafe로 C 함수를 호출합니다.
// 여기서는 (libc 크레이트 없이) Windows API 호출의 원리를
// std로 흉내 내고, unsafe의 올바른 사용법을 학습합니다.

// === 1. C 표준 라이브러리 함수 시그니처 (예시) ===
// 실무 코드:
//   extern "C" { fn strlen(s: *const i8) -> usize; }
// 안전한 래퍼:
//   fn safe_strlen(s: &str) -> usize { unsafe { strlen(s.as_ptr() as *const i8) } }

// === 2. std를 이용한 C 유사 동작 구현 ===
#[cfg(windows)]
fn system_dependencies() -> Vec<String> {
    // 명령줄에서 필요한 DLL 존재 확인 (libc 개념 대신)
    let checks = ["kernel32.dll", "user32.dll", "ntdll.dll"];
    checks
        .iter()
        .filter(|dll| {
            std::path::Path::new("C:\\Windows\\System32").join(dll).exists()
        })
        .map(|dll| dll.to_string())
        .collect()
}

#[cfg(not(windows))]
fn system_dependencies() -> Vec<String> {
    vec!["libc.so".into()]
}

// === 3. FFI 콜백 흉내: 함수 포인터 ===
type Callback = fn(i32) -> i32;

fn apply(value: i32, cb: Callback) -> i32 {
    // C의 함수 포인터 콜백 패턴과 동일
    cb(value)
}

fn double_c(v: i32) -> i32 {
    v * 2
}

fn triple_c(v: i32) -> i32 {
    v * 3
}

// === 4. raw 포인터와 unsafe (개념 재현) ===
fn raw_pointer_demo() {
    let x = 42;
    // &i32 -> *const i32 (raw 포인터)
    let raw: *const i32 = &x;

    unsafe {
        // unsafe 블록에서만 역참조 가능
        println!("raw 포인터 역참조: {}", *raw);
    }

    // null 포인터 개념
    let null_ptr: *const i32 = std::ptr::null();
    println!("null 포인터는 null: {}", null_ptr.is_null());
}

// === 5. 전역 정적 변수 (FFI 데이터 개념) ===
static mut GLOBAL_COUNTER: i32 = 0;

fn increment_counter() {
    unsafe {
        GLOBAL_COUNTER += 1;
    }
}

fn read_counter() -> i32 {
    unsafe { GLOBAL_COUNTER }
}

fn main() {
    println!("=== 시스템 의존 라이브러리 (DLL 확인) ===");
    let deps = system_dependencies();
    println!("확인된 DLL: {:?}", deps);
    println!("(참고) 이는 libc가 로드하는 시스템 라이브러리와 같은 개념입니다.");

    println!("\n=== 함수 포인터 콜백 ===");
    println!("double(5) = {}", apply(5, double_c));
    println!("triple(5) = {}", apply(5, triple_c));

    println!("\n=== raw 포인터 / unsafe ===");
    raw_pointer_demo();

    println!("\n=== 전역 정적 변수 (unsafe) ===");
    increment_counter();
    increment_counter();
    increment_counter();
    println!("카운터: {}", read_counter());

    println!("\n=== 실제 FFI 사용 시 (주석 예시) ===");
    println!("extern \"C\" {{ fn strlen(s: *const i8) -> usize; }}");
    println!("-> unsafe 블록에서 호출, safe wrapper로 감싸 안전성 회복");
}

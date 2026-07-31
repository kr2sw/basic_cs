// 36: 임베디드 Rust — no_std, cortex-m 개념
//
// 실제 임베디드는 #![no_std] + cortex-m-rt 로 개발합니다.
// 데스크톱에서는 표준 라이브러리가 쓰이지만, MCU 동작 방식을
// "가상 MCU" 시뮬레이터로 재현해 이해합니다.

// === 1. 레지스터 매핑 개념 (MCU 메모리 맵) ===
// ARM Cortex-M에서는 특정 주소에 하드웨어 레지스터가 매핑되어 있습니다.
// 예: GPIO B 데이터 레지스터 @ 0x48000414

#[derive(Clone, Copy)]
struct RegAddr(u32);

// === 2. 메모리 맵 시뮬레이션 ===
struct McuMemory {
    // 0..=0xFFFF 영역을 시뮬레이션
    data: Vec<u8>,
}

impl McuMemory {
    fn new() -> Self {
        McuMemory { data: vec![0u8; 0x1_0000] }
    }

    // 임베디드에서 쓰는 raw 포인터 접근 흉내
    unsafe fn write_byte(&mut self, addr: u32, value: u8) {
        self.data[addr as usize] = value;
    }

    unsafe fn read_byte(&self, addr: u32) -> u8 {
        self.data[addr as usize]
    }
}

// === 3. GPIO 제어 (embedded-hal 개념) ===
struct Gpio {
    base: u32, // 레지스터 베이스
}

impl Gpio {
    const MODE_OFFSET: u32 = 0x00;
    const OUTPUT_OFFSET: u32 = 0x14;

    fn new(base: u32) -> Self {
        Gpio { base }
    }

    // pin 모드 설정 (0=입력, 1=출력)
    unsafe fn set_mode(&self, mem: &mut McuMemory, pin: u8, output: bool) {
        let mode_reg = self.base + Self::MODE_OFFSET;
        let current = mem.read_byte(mode_reg);
        let v = current & !(0b11 << (pin * 2));
        mem.write_byte(mode_reg, v | if output { 0b01 << (pin * 2) } else { 0 });
    }

    // LED 켜기/끄기
    unsafe fn write_pin(&self, mem: &mut McuMemory, pin: u8, on: bool) {
        let out_reg = self.base + Self::OUTPUT_OFFSET;
        let current = mem.read_byte(out_reg);
        let v = if on { current | (1 << pin) } else { current & !(1 << pin) };
        mem.write_byte(out_reg, v);
    }

    unsafe fn read_pin(&self, mem: &mut McuMemory, pin: u8) -> bool {
        let out_reg = self.base + Self::OUTPUT_OFFSET;
        (mem.read_byte(out_reg) & (1 << pin)) != 0
    }
}

// === 4. 인터럽트(타이머) 개념 ===
// 인터럽트 = 외부 이벤트가 CPU를 중단시키는 것. 메인 루프에서 시뮬레이션.

// === 5. no_std 제약 체험 ===
// no_std에서는 다음이 불가능:
// - Vec (힙 할당 불가, alloc 필요)
// - String
// - 표준 입출력
// 대신:
// - 정적 배열 (StaticArray)
// - 고정 크기 버퍼

const MAX_BUF: usize = 64;
struct RingBuffer {
    buf: [u8; MAX_BUF],
    head: usize,
    tail: usize,
    len: usize,
}

impl RingBuffer {
    fn new() -> Self {
        RingBuffer { buf: [0; MAX_BUF], head: 0, tail: 0, len: 0 }
    }

    fn push(&mut self, v: u8) -> bool {
        if self.len == MAX_BUF {
            return false; // 꽉 참 (no_std 환경의 흔한 상황)
        }
        self.buf[self.tail] = v;
        self.tail = (self.tail + 1) % MAX_BUF;
        self.len += 1;
        true
    }

    fn pop(&mut self) -> Option<u8> {
        if self.len == 0 {
            return None;
        }
        let v = self.buf[self.head];
        self.head = (self.head + 1) % MAX_BUF;
        self.len -= 1;
        Some(v)
    }
}

// === 6. 메인 (메인 루프 패턴) ===
fn main() {
    println!("=== 가상 MCU 시뮬레이션 ===");

    // 메모리 준비
    let mut mem = McuMemory::new();

    // GPIO LED 핀 5 (포트 A 기준)
    let gpio_a = Gpio::new(0x4800_0000);

    // 버튼 시뮬레이션: 시작 시 초기화
    unsafe {
        gpio_a.set_mode(&mut mem, 5, true); // LED 출력
        gpio_a.write_pin(&mut mem, 5, false); // LED 끔
    }

    // 메인 루프 (블링크)
    let mut blink_counter = 0u32;
    for _ in 0..10 {
        blink_counter += 1;
        let on = blink_counter % 2 == 0;
        unsafe {
            gpio_a.write_pin(&mut mem, 5, on);
            println!("LED: {}", if on { "ON" } else { "OFF" });
        }
    }

    // RingBuffer (no_std 느낌)
    let mut ring = RingBuffer::new();
    for b in [1, 2, 3, 4, 5] {
        ring.push(b);
    }
    while let Some(b) = ring.pop() {
        print!("{} ", b);
    }
    println!("<- 링버퍼 수신");

    println!("\n실제 환경: #![no_std] + cortex-m-rt + embedded-hal");
    println!("타깃: thumbv7em-none-eabihf (STM32 등)");
}

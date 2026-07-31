// AssemblyScript 메모리 관리 & 라이브러리 데모

// 문자열 인코딩 결과를 계속 유지하기 위한 전역 참조 (GC 대상 방지)
let _lastUtf8: ArrayBuffer | null = null;

// 힙에 n바이트 할당하고 시작 주소 반환
export function alloc(n: i32): usize {
  return changetype<usize>(new ArrayBuffer(n));
}

// 주소 ptr에 i32 저장
export function write(ptr: usize, value: i32): void {
  store<i32>(ptr, value);
}

// 주소 ptr에서 i32 읽기
export function read(ptr: usize): i32 {
  return load<i32>(ptr);
}

// 문자열을 UTF-8로 인코딩해 힙에 두고 시작 주소 반환
// (raw instantiation에서는 객체가 아니라 주소가 반환됩니다)
export function encodeUtf8(s: string): usize {
  _lastUtf8 = String.UTF8.encode(s);
  return changetype<usize>(_lastUtf8);
}

// UTF-8 바이트 길이 계산
export function utf8ByteLength(s: string): i32 {
  return String.UTF8.byteLength(s);
}

// 참조 타입을 포인터로 다루기 위한 클래스
class Counter {
  count: i32 = 0;

  increment(): i32 {
    this.count += 1;
    return this.count;
  }
}

// 힙에 Counter 인스턴스를 만들고 주소 반환
export function makeCounter(): usize {
  return changetype<usize>(new Counter());
}

// 주소를 Counter로 바꿔 메서드 호출 (포인터 역참조)
export function bump(ptr: usize): i32 {
  return changetype<Counter>(ptr).increment();
}

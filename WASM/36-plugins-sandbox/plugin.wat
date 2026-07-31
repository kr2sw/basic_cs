(module
  ;; Extism 호스트가 제공하는 로그 함수 (메시지 포인터/길이)
  (import "extism:host/env" "log" (func $log (param i64 i64)))

  (memory (export "memory") 1)

  ;; 입력 i32를 2배로 만들어 반환
  ;; Extism 관례: 입력이 메모리 0번부터, 출력도 0번부터, 함수 반환값은 0(성공)
  (func (export "double")
    (local $n i32)
    (local.set $n (i32.load (i32.const 0)))
    (local.set $n (i32.mul (local.get $n) (i32.const 2)))
    (i32.store (i32.const 0) (local.get $n)))

  ;; 두 수를 더하는 예제 (입력: a, b 두 개의 i32)
  (func (export "add")
    (local $a i32)
    (local $b i32)
    (local.set $a (i32.load (i32.const 0)))
    (local.set $b (i32.load (i32.const 4)))
    (i32.store (i32.const 0) (i32.add (local.get $a) (local.get $b))))
)

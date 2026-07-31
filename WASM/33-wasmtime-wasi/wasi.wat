(module
  ;; WASI Preview 1 syscall import
  (import "wasi_snapshot_preview1" "fd_write"
    (func $fd_write (param i32 i32 i32 i32) (result i32)))
  (import "wasi_snapshot_preview1" "random_get"
    (func $random_get (param i32 i32) (result i32)))
  (import "wasi_snapshot_preview1" "clock_time_get"
    (func $clock_time_get (param i32 i64 i32) (result i32)))

  (memory (export "memory") 1)

  ;; 메시지 버퍼들
  (data (i32.const 64) "Hello from WASI!\n")
  (data (i32.const 96) "random bytes: ")
  (data (i32.const 128) "monotonic ticks: ")

  ;; iovec 하나를 stdout(fd=1)에 출력
  ;; iovec 배열: [ptr, len] 두 개의 i32 (8바이트)
  (func $println (param $ptr i32) (param $len i32)
    (i32.store (i32.const 0) (local.get $ptr))
    (i32.store (i32.const 4) (local.get $len))
    (call $fd_write (i32.const 1) (i32.const 0) (i32.const 1) (i32.const 8))
    drop)

  ;; i32 숫자를 10진수 문자열로 변환해 $buf에 기록, 길이 반환
  (func $itoa (param $value i32) (param $buf i32) (result i32)
    (local $i i32)
    (local $len i32)
    (local $n i32)
    (if (i32.eqz (local.get $value))
      (then
        (i32.store8 (local.get $buf) (i32.const 48))  ;; '0'
        (return (i32.const 1))))
    (local.set $i (i32.const 10))       ;; 버퍼 끝에서부터
    (local.set $n (local.get $value))
    (loop $loop
      (i32.store8
        (i32.add (local.get $buf) (local.get $i))
        (i32.add (i32.const 48)
          (i32.rem_u (local.get $n) (i32.const 10))))
      (local.set $n (i32.div_u (local.get $n) (i32.const 10)))
      (local.set $i (i32.sub (local.get $i) (i32.const 1)))
      (br_if $loop (i32.gt_u (local.get $n) (i32.const 0))))
    (local.set $i (i32.add (local.get $i) (i32.const 1)))
    (local.set $len (i32.sub (i32.const 11) (local.get $i)))
    (local.set $n (i32.const 0))
    ;; 자리수만큼 앞으로 이동 (복사)
    (loop $copy
      (i32.store8
        (i32.add (local.get $buf) (local.get $n))
        (i32.load8_u (i32.add (local.get $buf) (local.get $i))))
      (local.set $n (i32.add (local.get $n) (i32.const 1)))
      (local.set $i (i32.add (local.get $i) (i32.const 1)))
      (br_if $copy (i32.lt_u (local.get $n) (local.get $len))))
    (local.get $len))

  (func (export "_start")
    ;; 1) 표준 출력에 인사말 ("Hello from WASI!\n" = 17바이트)
    (call $println (i32.const 64) (i32.const 17))

    ;; 2) 8바이트 난수 생성 후 출력
    (call $random_get (i32.const 160) (i32.const 8))
    drop
    (call $println (i32.const 96) (i32.const 14))
    (call $println (i32.const 160) (i32.const 8))

    ;; 3) 모노토닉 시계 읽기 (nanosecond, u64)
    (call $clock_time_get (i32.const 1) (i64.const 0) (i32.const 168))
    drop
    (call $println (i32.const 128) (i32.const 17))  ;; "monotonic ticks: " = 17자
    ;; i64 상위/하위 32비트 중 하위만 10진수로 출력 (간이 데모)
    (call $println
      (i32.const 200)
      (call $itoa (i32.load (i32.const 168)) (i32.const 200))))

  ;; WASI 없이도 순수 함수로 테스트 가능
  (func (export "add") (param $a i32) (param $b i32) (result i32)
    (i32.add (local.get $a) (local.get $b)))
)

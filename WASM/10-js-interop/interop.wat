(module
  (memory (export "memory") 1)

  ;; 문자열을 메모리에 쓰고 주소 반환
  (func $get_string (export "get_string") (result i32)
    i32.const 0
  )

  ;; 정수 배열 합계 계산
  (func $sum_array (export "sum_array") (param $ptr i32) (param $len i32) (result i32)
    (local $i i32)
    (local $sum i32)
    (block $done
      (loop $loop
        local.get $i
        local.get $len
        i32.ge_s
        br_if $done

        local.get $sum
        local.get $ptr
        local.get $i
        i32.const 2
        i32.shl
        i32.add
        i32.load
        i32.add
        local.set $sum

        local.get $i
        i32.const 1
        i32.add
        local.set $i
        br $loop
      )
    )
    local.get $sum
  )

  (data (i32.const 0) "Hello from WASM!")

  ;; 포인터 역참조 - 메모리 값을 읽음
  (func $read_memory (export "read_memory") (param $addr i32) (result i32)
    local.get $addr
    i32.load
  )

  ;; 메모리 값 쓰기
  (func $write_memory (export "write_memory") (param $addr i32) (param $value i32)
    local.get $addr
    local.get $value
    i32.store
  )
)

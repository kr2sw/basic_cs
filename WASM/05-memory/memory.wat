(module
  (memory (export "memory") 1)  ;; 1페이지 (64KB)

  ;; 메모리에 정수 쓰기/읽기
  (func $write_int (export "write_int") (param $addr i32) (param $value i32)
    local.get $addr
    local.get $value
    i32.store
  )

  (func $read_int (export "read_int") (param $addr i32) (result i32)
    local.get $addr
    i32.load
  )

  ;; 메모리에 바이트 쓰기
  (func $write_byte (export "write_byte") (param $addr i32) (param $value i32)
    local.get $addr
    local.get $value
    i32.store8
  )

  ;; 메모리 복사
  (func $memcpy (export "memcpy") (param $dest i32) (param $src i32) (param $len i32)
    (local $i i32)
    (block $done
      (loop $loop
        local.get $i
        local.get $len
        i32.ge_u
        br_if $done

        local.get $dest
        local.get $i
        i32.add
        local.get $src
        local.get $i
        i32.add
        i32.load8_u
        i32.store8

        local.get $i
        i32.const 1
        i32.add
        local.set $i
        br $loop
      )
    )
  )

  ;; 문자열 저장 (hello.wasm에 미리 저장)
  (data (i32.const 0) "Hello, WebAssembly Memory!")

  ;; 문자열 읽기
  (func $read_string (export "read_string") (param $addr i32) (param $len i32) (result i32)
    local.get $addr  ;; 시작 주소 반환 (JS에서 문자열로 읽음)
  )

  ;; 메모리 크기 확인
  (func $get_memory_size (export "get_memory_size") (result i32)
    memory.size
  )
)

(module
  (memory (export "memory") 1 10)  ;; 최소 1페이지, 최대 10페이지

  ;; 데이터 세그먼트 (여러 개)
  (data $hello (i32.const 0) "Hello")
  (data $world (i32.const 16) "World!")
  (data $nums (i32.const 32) "\01\02\03\04\05")

  ;; 현재 메모리 크기 확인
  (func $get_pages (export "get_pages") (result i32)
    memory.size
  )

  ;; 메모리 확장
  (func $grow_memory (export "grow_memory") (param $pages i32) (result i32)
    local.get $pages
    memory.grow
  )

  ;; 메모리 영역 복사 (memcpy)
  (func $memcpy (export "memcpy") (param $dest i32) (param $src i32) (param $len i32)
    memory.copy
  )

  ;; 메모리 영역 채우기 (memset)
  (func $memset (export "memset") (param $addr i32) (param $value i32) (param $len i32)
    (local $i i32)
    (block $done
      (loop $loop
        local.get $i
        local.get $len
        i32.ge_u
        br_if $done

        local.get $addr
        local.get $i
        i32.add
        local.get $value
        i32.store8

        local.get $i
        i32.const 1
        i32.add
        local.set $i
        br $loop
      )
    )
  )

  ;; 데이터 세그먼트 초기화 (data.drop 후 수동 복사)
  (func $get_hello_addr (export "get_hello_addr") (result i32)
    i32.const 0
  )

  (func $get_world_addr (export "get_world_addr") (result i32)
    i32.const 16
  )
)

(module
  (func $buggy_factorial (export "buggy_factorial") (param $n i32) (result i32)
    (local $result i32)
    (local $i i32)

    i32.const 1
    local.set $result

    (block $done
      (loop $loop
        local.get $i
        local.get $n
        i32.ge_s
        br_if $done

        ;; result *= i (잘못됨: i가 0부터 시작)
        local.get $result
        local.get $i
        i32.mul
        local.set $result

        local.get $i
        i32.const 1
        i32.add
        local.set $i
        br $loop
      )
    )

    local.get $result
  )

  (func $fixed_factorial (export "fixed_factorial") (param $n i32) (result i32)
    (local $result i32)
    (local $i i32)

    i32.const 1
    local.set $result
    i32.const 1
    local.set $i          ;; 1부터 시작

    (block $done
      (loop $loop
        local.get $i
        local.get $n
        i32.gt_s          ;; > n이면 종료
        br_if $done

        local.get $result
        local.get $i
        i32.mul
        local.set $result

        local.get $i
        i32.const 1
        i32.add
        local.set $i
        br $loop
      )
    )

    local.get $result
  )

  ;; 스택 상태 출력용
  (func $add_trace (export "add_trace") (param $a i32) (param $b i32) (result i32 i32 i32)
    local.get $a
    local.get $b
    local.get $a
    local.get $b
    i32.add
  )
)

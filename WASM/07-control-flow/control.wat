(module
  ;; if/else 조건문
  (func $max (export "max") (param $a i32) (param $b i32) (result i32)
    (if (result i32)
      (i32.gt_s (local.get $a) (local.get $b))
      (then (local.get $a))
      (else (local.get $b))
    )
  )

  ;; block으로 조기 반환
  (func $abs (export "abs") (param $x i32) (result i32)
    (block $done (result i32)
      (if (i32.ge_s (local.get $x) (i32.const 0))
        (then (br $done (local.get $x)))
      )
      (i32.sub (i32.const 0) (local.get $x))
    )
  )

  ;; loop로 합계 계산 (1부터 n까지)
  (func $sum_to_n (export "sum_to_n") (param $n i32) (result i32)
    (local $sum i32)
    (local $i i32)

    (block $done
      (loop $loop
        ;; i > n이면 종료
        local.get $i
        local.get $n
        i32.gt_s
        br_if $done

        ;; sum += i
        local.get $sum
        local.get $i
        i32.add
        local.set $sum

        ;; i++
        local.get $i
        i32.const 1
        i32.add
        local.set $i

        br $loop
      )
    )

    local.get $sum
  )

  ;; br_table - switch/case
  (func $day_name (export "day_name") (param $day i32) (result i32)
    (block $mon
      (block $tue
        (block $wed
          (block $thu
            (block $fri
              (block $sat
                (block $sun
                  local.get $day
                  br_table $sun $mon $tue $wed $thu $fri $sat
                )
                i32.const 0  ;; 일요일
                return
              )
              i32.const 6  ;; 토요일
              return
            )
            i32.const 5  ;; 금요일
            return
          )
          i32.const 4  ;; 목요일
          return
        )
        i32.const 3  ;; 수요일
        return
      )
      i32.const 2  ;; 화요일
      return
    )
    i32.const 1  ;; 월요일
  )

  ;; 중첩 block/loop
  (func $nested_loops (export "nested_loops") (param $n i32) (param $m i32) (result i32)
    (local $i i32)
    (local $j i32)
    (local $count i32)

    (block $outer_done
      (loop $outer_loop
        local.get $i
        local.get $n
        i32.ge_s
        br_if $outer_done

        (block $inner_done
          (loop $inner_loop
            local.get $j
            local.get $m
            i32.ge_s
            br_if $inner_done

            local.get $count
            i32.const 1
            i32.add
            local.set $count

            local.get $j
            i32.const 1
            i32.add
            local.set $j
            br $inner_loop
          )
        )

        i32.const 0
        local.set $j

        local.get $i
        i32.const 1
        i32.add
        local.set $i
        br $outer_loop
      )
    )

    local.get $count
  )
)

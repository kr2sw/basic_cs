(module
  ;; 불변 전역 변수
  (global $PI f64 (f64.const 3.1415926535))

  ;; 가변 전역 변수
  (global $counter (mut i32) (i32.const 0))

  ;; 전역 변수 내보내기
  (export "PI" (global $PI))

  ;; 지역 변수 사용 예제
  (func $sum_to_n (export "sum_to_n") (param $n i32) (result i32)
    (local $sum i32)
    (local $i i32)

    (block $done
      (loop $loop
        ;; if i > n then break
        (if (i32.gt_s (local.get $i) (local.get $n))
          (br $done)
        )

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

  ;; 카운터 증가
  (func $increment (export "increment") (result i32)
    global.get $counter
    i32.const 1
    i32.add
    global.set $counter
    global.get $counter
  )

  ;; local.tee 예제 (읽기+쓰기 동시에)
  (func $double (export "double") (param $x i32) (result i32)
    (local $tmp i32)
    local.get $x
    local.tee $tmp
    local.get $tmp
    i32.add
  )

  ;; 여러 타입의 지역 변수
  (func $mixed_locals (export "mixed_locals") (result i32 i64 f64)
    (local $a i32)
    (local $b i64)
    (local $c f64)

    i32.const 100
    local.set $a

    i64.const 200
    local.set $b

    f64.const 3.14
    local.set $c

    local.get $a
    local.get $b
    local.get $c
  )
)

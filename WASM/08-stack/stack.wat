(module
  ;; 스택 기본 동작: ((10 + 20) * (30 - 5)) = 750
  (func $calc (export "calc") (result i32)
    i32.const 10
    i32.const 20
    i32.add       ;; 10 + 20 = 30

    i32.const 30
    i32.const 5
    i32.sub       ;; 30 - 5 = 25

    i32.mul       ;; 30 * 25 = 750
  )

  ;; drop: 스택 값 버리기
  (func $drop_example (export "drop_example") (result i32)
    i32.const 10
    i32.const 20
    drop          ;; 20 버림
    ;; 스택: [10]
  )

  ;; select: 조건부 값 선택
  (func $select_example (export "select_example") (param $cond i32) (result i32)
    i32.const 100
    i32.const 200
    local.get $cond
    select        ;; cond != 0 → 200, cond == 0 → 100
  )

  ;; local.tee: 읽기 + 쓰기를 동시에
  (func $tee_example (export "tee_example") (param $x i32) (param $y i32) (result i32 i32)
    (local $tmp i32)
    local.get $x
    local.set $tmp
    local.get $tmp   ;; x
    local.get $tmp
    local.get $y
    i32.add
    local.set $tmp
    local.get $tmp   ;; x + y
  )

  ;; 스택 재배열 (swap/unreachable)
  (func $swap_and_reject (export "swap_and_reject") (param $a i32) (param $b i32) (result i32)
    local.get $a  ;; push a
    local.get $b  ;; push b
    return        ;; 기본: b, a 순서로 반환
  )

  ;; 중간 연산 과정 보기: (a + b) * (a - b)
  (func $ab_calc (export "ab_calc") (param $a i32) (param $b i32) (result i32)
    local.get $a   ;; a
    local.get $b   ;; a, b
    i32.add        ;; a+b

    local.get $a   ;; a+b, a
    local.get $b   ;; a+b, a, b
    i32.sub        ;; a+b, a-b

    i32.mul        ;; (a+b)*(a-b)
  )
)

(module
  ;; 기본 함수 - 두 수의 합
  (func $add (export "add") (param $a i32) (param $b i32) (result i32)
    local.get $a
    local.get $b
    i32.add
  )

  ;; 여러 반환값
  (func $divmod (export "divmod") (param $a i32) (param $b i32) (result i32 i32)
    local.get $a
    local.get $b
    i32.div_s

    local.get $a
    local.get $b
    i32.rem_s
  )

  ;; 내부 함수 (익스포트되지 않음)
  (func $private_helper (param $x i32) (result i32)
    local.get $x
    i32.const 2
    i32.mul
  )

  ;; 내부 함수를 호출하는 exported 함수
  (func $double (export "double") (param $x i32) (result i32)
    local.get $x
    call $private_helper
  )

  ;; 재귀 함수 - 피보나치
  (func $fib (export "fib") (param $n i32) (result i32)
    (if (result i32)
      (i32.le_s (local.get $n) (i32.const 1))
      (then (local.get $n))
      (else
        (i32.add
          (call $fib (i32.sub (local.get $n) (i32.const 1)))
          (call $fib (i32.sub (local.get $n) (i32.const 2)))
        )
      )
    )
  )

  ;; 고차 함수 패턴 - 함수 테이블 호출 (간접 호출)
  (func $apply_twice (export "apply_twice") (param $f i32) (param $x i32) (result i32)
    local.get $x
    call_indirect (type $i32_to_i32)
    local.get $x
    call_indirect (type $i32_to_i32)  ;; f(f(x)) 대신 f(x) + f(x)
  )
)

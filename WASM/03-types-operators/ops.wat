(module
  ;; i32 산술 연산
  (func $i32_ops (export "i32_ops") (param $a i32) (param $b i32) (result i32 i32 i32 i32)
    local.get $a
    local.get $b
    i32.add

    local.get $a
    local.get $b
    i32.sub

    local.get $a
    local.get $b
    i32.mul

    local.get $a
    local.get $b
    i32.div_s

    ;; 스택에 쌓인 4개 값을 모두 반환
  )

  ;; f64 산술 연산
  (func $f64_ops (export "f64_ops") (param $a f64) (param $b f64) (result f64 f64 f64 f64)
    local.get $a
    local.get $b
    f64.add

    local.get $a
    local.get $b
    f64.sub

    local.get $a
    local.get $b
    f64.mul

    local.get $a
    local.get $b
    f64.div
  )

  ;; 비트 연산
  (func $bit_ops (export "bit_ops") (param $a i32) (param $b i32) (result i32 i32 i32 i32)
    local.get $a
    local.get $b
    i32.and

    local.get $a
    local.get $b
    i32.or

    local.get $a
    local.get $b
    i32.xor

    local.get $a
    i32.const 1
    i32.shl
  )

  ;; 비교 연산
  (func $cmp_ops (export "cmp_ops") (param $a i32) (param $b i32) (result i32 i32 i32)
    local.get $a
    local.get $b
    i32.eq

    local.get $a
    local.get $b
    i32.lt_s

    local.get $a
    local.get $b
    i32.gt_s
  )

  ;; 타입 변환
  (func $convert (export "convert") (param $i i32) (result f64)
    local.get $i
    f64.convert_i32_s
  )
)

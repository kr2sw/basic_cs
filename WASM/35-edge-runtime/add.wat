(module
  ;; 엣지 런타임에서 쓰는 작은 계산 모듈
  (func (export "add") (param $a i32) (param $b i32) (result i32)
    (i32.add (local.get $a) (local.get $b)))

  (func (export "mul") (param $a i32) (param $b i32) (result i32)
    (i32.mul (local.get $a) (local.get $b)))

  ;; HTTP 상태 분류 (작은 비즈니스 로직 예)
  (func (export "classify") (param $code i32) (result i32)
    (if (i32.lt_u (local.get $code) (i32.const 400))
      (then (return (i32.const 0))))   ;; ok
    (if (i32.lt_u (local.get $code) (i32.const 500))
      (then (return (i32.const 1))))   ;; client error
    (i32.const 2))                     ;; server error
)

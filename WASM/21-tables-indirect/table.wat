(module
  ;; 함수 시그니처 정의: 두 정수를 받아 한 정수를 반환
  (type $binop (func (param i32 i32) (result i32)))

  ;; funcref 타입 테이블, 초기 크기 4
  (table $funcs 4 funcref)

  ;; 테이블 0~3번에 함수 채우기
  (elem (i32.const 0) $add $sub $mul $div)

  ;; --- 함수 정의 ---
  (func $add (type $binop)
    (param $a i32) (param $b i32) (result i32)
    (i32.add (local.get $a) (local.get $b)))

  (func $sub (type $binop)
    (param $a i32) (param $b i32) (result i32)
    (i32.sub (local.get $a) (local.get $b)))

  (func $mul (type $binop)
    (param $a i32) (param $b i32) (result i32)
    (i32.mul (local.get $a) (local.get $b)))

  (func $div (type $binop)
    (param $a i32) (param $b i32) (result i32)
    (if (i32.eqz (local.get $b))
      (then (unreachable))  ;; 0으로 나누면 트랩
    )
    (i32.div_s (local.get $a) (local.get $b)))

  ;; 테이블의 $fn 인덱스에 있는 함수를 간접 호출
  (func (export "compute")
    (param $a i32) (param $b i32) (param $fn i32) (result i32)
    (call_indirect (type $binop)
      (local.get $a) (local.get $b) (local.get $fn)))

  ;; 테이블에서 함수 참조 읽기 (직접 호출 패턴)
  (func (export "directCompute")
    (param $a i32) (param $b i32) (param $fn i32) (result i32)
    (local $f funcref)
    (local.set $f (table.get $funcs (local.get $fn)))
    ;; funcref는 ref.is_null로 널 여부 확인 후 call_ref로 호출 가능
    (if (ref.is_null (local.get $f))
      (then (unreachable)))
    (call_ref $binop (local.get $a) (local.get $b) (local.get $f)))

  ;; 테이블 크기 반환
  (func (export "tableSize") (result i32)
    (table.size $funcs))

  ;; 실행 중 테이블 교체 (예: $sub 자리에 $mul 넣기)
  (func (export "swapToMul") (param $i i32)
    (table.set $funcs (local.get $i) (ref.func $mul)))

  ;; 0번 인덱스가 뭔지 물어보는 헬퍼 (교체 확인용)
  (func (export "callZero")
    (param $a i32) (param $b i32) (result i32)
    (call_indirect (type $binop) (local.get $a) (local.get $b) (i32.const 0)))
)

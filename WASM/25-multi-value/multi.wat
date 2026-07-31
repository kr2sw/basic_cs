(module
  ;; 기본 메모리
  (memory (export "memory") 1)
  ;; 두 번째 메모리 (multi-memory 프로포절)
  (memory $aux (export "aux") 1)

  (data (i32.const 0) "main memory")        ;; 기본 메모리 초기 데이터
  (data $auxData (memory $aux) (i32.const 0) "aux memory")  ;; 보조 메모리 초기 데이터

  ;; 두 수의 min / max
  (func $min (param i32 i32) (result i32)
    (local.get 0) (local.get 1) i32.min_s)

  (func $max (param i32 i32) (result i32)
    (local.get 0) (local.get 1) i32.max_s)

  ;; 몫과 나머지 동시 반환
  (func (export "divmod") (param $a i32) (param $b i32) (result i32 i32)
    (local.get $a) (local.get $b) i32.div_s
    (local.get $a) (local.get $b) i32.rem_s)

  ;; min과 max를 하나의 호출로 합성해서 반환
  (func (export "minmax") (param $a i32) (param $b i32) (result i32 i32)
    (local.get $a) (local.get $b) (call $min)
    (local.get $a) (local.get $b) (call $max))

  ;; 두 좌표를 더해 (x, y) 한 쌍 반환
  (func (export "addVec") (param $ax i32) (param $ay i32) (param $bx i32) (param $by i32)
    (result i32 i32)
    (i32.add (local.get $ax) (local.get $bx))
    (i32.add (local.get $ay) (local.get $by)))

  ;; 블록이 다중값을 만들어 냄: n이 짝수면 (n, 0), 홀수면 (0, n)
  (func (export "splitByParity") (param $n i32) (result i32 i32)
    (block $done (result i32 i32)
      (if (i32.eqz (i32.rem_u (local.get $n) (i32.const 2)))
        (then (br $done (local.get $n) (i32.const 0)))
        (else (br $done (i32.const 0) (local.get $n))))
      (unreachable)))

  ;; 보조 메모리에 값 쓰고 다시 읽기
  (func (export "auxWrite") (param $addr i32) (param $value i32)
    (i32.store (memory $aux) (local.get $addr) (local.get $value)))

  (func (export "auxRead") (param $addr i32) (result i32)
    (i32.load (memory $aux) (local.get $addr)))

  ;; 두 메모리 크기 각각 반환 (두 결과)
  (func (export "sizes") (result i32 i32)
    (memory.size)
    (memory.size $aux))
)

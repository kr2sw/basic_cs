(module
  (memory (export "memory") 1)   ;; 1페이지 = 65536바이트

  ;; 범위 밖 읽기 → 트랩 (경계 검사)
  (func (export "readByte") (param $i i32) (result i32)
    (i32.load8_u (local.get $i)))

  ;; 범위 밖 쓰기 → 트랩
  (func (export "writeByte") (param $i i32) (param $v i32)
    (i32.store8 (local.get $i) (local.get $v)))

  ;; 현재 메모리 페이지 수
  (func (export "size") (result i32)
    (memory.size))

  ;; 메모리 확장 (최대 8페이지)
  (func (export "grow") (param $pages i32) (result i32)
    (memory.grow (local.get $pages)))

  ;; 데이터 세그먼트로 유효 영역에 값 쓰기 (안전 접근 예)
  (data (i32.const 0) "\00\01\02\03\04\05\06\07")

  (func (export "readSafe") (param $i i32) (result i32)
    (if (i32.ge_u (local.get $i) (i32.const 8))
      (then (return (i32.const -1))))   ;; 명시적 거부
    (i32.load8_u (local.get $i)))
)

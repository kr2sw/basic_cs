(module
  (memory (export "memory") 1)

  ;; 수동(패시브) 데이터 세그먼트 — 초기화 시 메모리에 자동으로 올라가지 않음
  (data $msg "Hello, Bulk Memory!")

  ;; 메모리 범위를 0으로 채움 (memset)
  (func (export "fillZero") (param $dst i32) (param $len i32)
    (memory.fill (local.get $dst) (i32.const 0) (local.get $len)))

  ;; 메모리 범위를 특정 값으로 채움
  (func (export "fillPattern") (param $dst i32) (param $value i32) (param $len i32)
    (memory.fill (local.get $dst) (local.get $value) (local.get $len)))

  ;; 수동 세그먼트 $msg를 메모리 $dst 위치로 복사 (지연 초기화)
  (func (export "loadMsg") (param $dst i32)
    (memory.init $msg (local.get $dst) (i32.const 0) (i32.const 20)))

  ;; 세그먼트 크기 반환
  (func (export "msgSize") (result i32)
    (data.size $msg))

  ;; 세그먼트를 메모리에서 해제 (메모리 절약)
  (func (export "dropMsg")
    (data.drop $msg))

  ;; src에서 dst로 len 바이트 복사 (원본/대상 겹침 허용)
  (func (export "copyBlock") (param $dst i32) (param $src i32) (param $len i32)
    (memory.copy (local.get $dst) (local.get $src) (local.get $len)))

  ;; hello world 문자열을 준비된 영역에 조립
  (func (export "buildMessage") (param $dst i32) (result i32)
    ;; "Hello " (6바이트)를 $dst에 기록
    (memory.init $msg (local.get $dst) (i32.const 0) (i32.const 6))
    ;; 그 뒤 6바이트를 0x21 ('!')로 채움
    (memory.fill
      (i32.add (local.get $dst) (i32.const 6))
      (i32.const 33)
      (i32.const 6))
    ;; 총 길이 반환
    (i32.const 12))
)

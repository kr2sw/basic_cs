(module
  ;; 예외 태그 선언 (payload: i32)
  (tag $divByZero (param i32))
  (tag $rangeError (param i32 i32))

  ;; 0으로 나누면 예외를 잡아 -1 반환
  (func (export "safeDiv") (param $a i32) (param $b i32) (result i32)
    (try
      (do
        (if (i32.eqz (local.get $b))
          (then (throw $divByZero (local.get $b))))
        (i32.div_s (local.get $a) (local.get $b)))
      (catch $divByZero
        (drop)                       ;; payload i32 버림
        (i32.const -1))
      (catch_all
        (i32.const -2))))

  ;; 배열 인덱스 검사 — 실패 시 예외를 잡아 0 반환
  (func (export "safeGet") (param $idx i32) (param $len i32) (result i32)
    (try
      (do
        (if (i32.ge_u (local.get $idx) (local.get $len))
          (then (throw $rangeError (local.get $idx) (local.get $len))))
        (i32.load8_u (local.get $idx)))
      (catch $rangeError
        (drop) (drop)                 ;; payload 2개(i32, i32) 소비
        (i32.const 0))))

  ;; delegate: 내부 try에서 잡지 않고 바깥 try로 전달
  (func (export "nestedDiv") (param $a i32) (param $b i32) (result i32)
    (local $x i32)
    (try $outer
      (do
        (try
          (do
            (if (i32.eqz (local.get $b))
              (then (throw $divByZero (local.get $b))))
            (local.set $x (i32.div_s (local.get $a) (local.get $b))))
          (delegate $outer)))        ;; 예외를 $outer의 catch_all로 위임
      (catch_all
        (local.set $x (i32.const -999))))
    (local.get $x))

  ;; 잡지 않고 그대로 JS로 던짐
  (func (export "panic") (result i32)
    (throw $rangeError (i32.const 10) (i32.const 20)))

  ;; payload를 꺼내 쓰기: 두 payload를 더한 값을 catch 블록에서 계산
  (func (export "describe") (param $idx i32) (param $len i32) (result i32)
    (try
      (do
        (if (i32.ge_u (local.get $idx) (local.get $len))
          (then (throw $rangeError (local.get $idx) (local.get $len))))
        (i32.const 1))               ;; 정상
      (catch $rangeError
        (drop) (drop)                 ;; payload 2개 소비
        (i32.add (local.get $idx) (local.get $len)))))  ;; payload 합
)

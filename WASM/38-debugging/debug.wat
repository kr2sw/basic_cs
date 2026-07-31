(module $debug_demo
  (memory (export "memory") 1)

  ;; 팩토리얼 (Breakpoint 데모용)
  (func $factorial (export "factorial") (param $n i32) (result i32)
    (local $acc i32)
    (local $i i32)
    (local.set $acc (i32.const 1))
    (local.set $i (local.get $n))
    (block $done
      (loop $loop
        ;; 여기에 중단점을 걸어 $i, $acc 변화를 확인하세요
        (br_if $done (i32.le_s (local.get $i) (i32.const 1)))
        (local.set $acc (i32.mul (local.get $acc) (local.get $i)))
        (local.set $i (i32.sub (local.get $i) (i32.const 1)))
        (br $loop)))
    (local.get $acc))

  ;; 0으로 나누면 트랩 — 실패 원인 분석 데모
  (func $unsafeDiv (export "unsafeDiv")
    (param $a i32) (param $b i32) (result i32)
    (i32.div_s (local.get $a) (local.get $b)))

  ;; 지역 변수가 여러 개일 때 스코프 확인
  (func $avgOfThree (export "avgOfThree")
    (param $a i32) (param $b i32) (param $c i32) (result i32)
    (i32.div_s
      (i32.add (i32.add (local.get $a) (local.get $b)) (local.get $c))
      (i32.const 3)))

  ;; 문자열을 메모리에 복사 (네임 섹션 확인용)
  (data (i32.const 0) "debug me!")

  (func $debugStringPtr (export "debugStringPtr") (result i32)
    (i32.const 0))
)

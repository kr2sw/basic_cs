(module
  (memory (export "memory") 32)   ;; 2MB (합계 벤치마크 1.6MB 수용)

  ;; 재귀 피보나치 (측정용 — 느린 의도)
  (func (export "fib") (param $n i32) (result i32)
    (if (i32.le_s (local.get $n) (i32.const 1))
      (then (return (local.get $n))))
    (i32.add
      (call $fib (i32.sub (local.get $n) (i32.const 1)))
      (call $fib (i32.sub (local.get $n) (i32.const 2)))))

  ;; limit 이하 소수 개수
  (func (export "countPrimes") (param $limit i32) (result i32)
    (local $n i32)
    (local $d i32)
    (local $count i32)
    (local $isPrime i32)
    (local.set $n (i32.const 2))
    (block $done
      (loop $loop
        (br_if $done (i32.gt_s (local.get $n) (local.get $limit)))
        (local.set $isPrime (i32.const 1))
        (local.set $d (i32.const 2))
        (block $break
          (loop $inner
            (br_if $break
              (i32.gt_s
                (i32.mul (local.get $d) (local.get $d))
                (local.get $n)))
            (if (i32.eqz (i32.rem_s (local.get $n) (local.get $d)))
              (then
                (local.set $isPrime (i32.const 0))
                (br $break)))
            (local.set $d (i32.add (local.get $d) (i32.const 1)))
            (br $inner)))
        (if (local.get $isPrime)
          (then (local.set $count (i32.add (local.get $count) (i32.const 1)))))
        (local.set $n (i32.add (local.get $n) (i32.const 1)))
        (br $loop)))
    (local.get $count))

  ;; 메모리 접근 벤치: 배열 요소를 모두 더함
  (func (export "sumArray") (param $ptr i32) (param $count i32) (result i32)
    (local $i i32)
    (local $acc i32)
    (block $done
      (loop $loop
        (br_if $done (i32.ge_u (local.get $i) (local.get $count)))
        (local.set $acc
          (i32.add (local.get $acc)
            (i32.load (i32.add (local.get $ptr) (local.get $i)))))
        (local.set $i (i32.add (local.get $i) (i32.const 4)))
        (br $loop)))
    (local.get $acc))
)

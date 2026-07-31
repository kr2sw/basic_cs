(module
  (memory (export "memory") 128)   ;; 8MB (벤치마크 버퍼 4.8MB 수용)

  ;; a[i] + b[i] → dst[i] (i32x4, 4개 동시 처리)
  (func (export "add_i32x4")
    (param $a i32) (param $b i32) (param $dst i32) (param $n i32)
    (local $i i32)
    (block $done
      (loop $loop
        (br_if $done (i32.ge_u (local.get $i) (local.get $n)))
        ;; 16바이트 = i32 4개 로드 후 한 번에 덧셈
        (v128.store
          (i32.add (local.get $dst) (local.get $i))
          (i32x4.add
            (v128.load (i32.add (local.get $a) (local.get $i)))
            (v128.load (i32.add (local.get $b) (local.get $i)))))
        (local.set $i (i32.add (local.get $i) (i32.const 16)))
        (br $loop)))
  )

  ;; a[i] * b[i] → dst[i] (f32x4)
  (func (export "mul_f32x4")
    (param $a i32) (param $b i32) (param $dst i32) (param $n i32)
    (local $i i32)
    (block $done
      (loop $loop
        (br_if $done (i32.ge_u (local.get $i) (local.get $n)))
        (v128.store
          (i32.add (local.get $dst) (local.get $i))
          (f32x4.mul
            (v128.load (i32.add (local.get $a) (local.get $i)))
            (v128.load (i32.add (local.get $b) (local.get $i)))))
        (local.set $i (i32.add (local.get $i) (i32.const 16)))
        (br $loop)))
  )

  ;; i32x4 벡터 합산 (count는 i32x4 그룹 수, 16바이트 단위)
  (func (export "sum_i32x4") (param $ptr i32) (param $count i32) (result i32)
    (local $acc v128)
    (local $i i32)
    (local.set $acc (i32x4.splat (i32.const 0)))
    (block $done
      (loop $loop
        (br_if $done (i32.ge_u (local.get $i) (local.get $count)))
        (local.set $acc
          (i32x4.add
            (local.get $acc)
            (v128.load (i32.add (local.get $ptr) (local.get $i)))))
        (local.set $i (i32.add (local.get $i) (i32.const 16)))
        (br $loop)))
    ;; 4개 래인을 스칼라로 합침
    (i32.add
      (i32.add
        (i32x4.extract_lane 0 (local.get $acc))
        (i32x4.extract_lane 1 (local.get $acc)))
      (i32.add
        (i32x4.extract_lane 2 (local.get $acc))
        (i32x4.extract_lane 3 (local.get $acc))))
  )

  ;; 바이트 단위 셔플 (i8x16.swizzle): mask[i] 인덱스의 바이트 선택
  (func (export "swizzle_i8x16")
    (param $src i32) (param $mask i32) (param $dst i32)
    (v128.store (local.get $dst)
      (i8x16.swizzle
        (v128.load (local.get $src))
        (v128.load (local.get $mask))))
  )

  ;; 스칼라 i32 덧셈 (비교용, SIMD 없이)
  (func (export "add_i32_scalar")
    (param $a i32) (param $b i32) (param $dst i32) (param $n i32)
    (local $i i32)
    (block $done
      (loop $loop
        (br_if $done (i32.ge_u (local.get $i) (local.get $n)))
        (i32.store
          (i32.add (local.get $dst) (local.get $i))
          (i32.add
            (i32.load (i32.add (local.get $a) (local.get $i)))
            (i32.load (i32.add (local.get $b) (local.get $i)))))
        (local.set $i (i32.add (local.get $i) (i32.const 4)))
        (br $loop)))
  )
)

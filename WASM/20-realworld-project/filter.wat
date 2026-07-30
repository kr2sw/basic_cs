(module
  (memory (export "memory") 1)

  ;; 픽셀 데이터를 그레이스케일로 변환
  ;; 각 픽셀: R, G, B, A (4바이트)
  ;; 공식: gray = 0.299*R + 0.587*G + 0.114*B
  (func $grayscale (export "grayscale")
    (param $ptr i32)     ;; 픽셀 데이터 시작 주소
    (param $count i32)   ;; 픽셀 개수
    (local $i i32)
    (local $r i32)
    (local $g i32)
    (local $b i32)
    (local $gray i32)

    (block $done
      (loop $loop
        local.get $i
        local.get $count
        i32.ge_s
        br_if $done

        ;; R 읽기
        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.load8_u
        local.set $r

        ;; G 읽기
        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 1
        i32.add
        i32.load8_u
        local.set $g

        ;; B 읽기
        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 2
        i32.add
        i32.load8_u
        local.set $b

        ;; gray = (R*299 + G*587 + B*114 + 500) / 1000
        local.get $r
        i32.const 299
        i32.mul

        local.get $g
        i32.const 587
        i32.mul
        i32.add

        local.get $b
        i32.const 114
        i32.mul
        i32.add

        i32.const 500
        i32.add
        i32.const 1000
        i32.div_s
        local.set $gray

        ;; R = G = B = gray (알파는 유지)
        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        local.get $gray
        i32.store8

        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 1
        i32.add
        local.get $gray
        i32.store8

        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 2
        i32.add
        local.get $gray
        i32.store8

        local.get $i
        i32.const 1
        i32.add
        local.set $i
        br $loop
      )
    )
  )

  ;; 밝기 조절
  (func $brightness (export "brightness")
    (param $ptr i32)
    (param $count i32)
    (param $delta i32)   ;; 밝기 조절값 (-255 ~ 255)
    (local $i i32)
    (local $val i32)

    (block $done
      (loop $loop
        local.get $i
        local.get $count
        i32.ge_s
        br_if $done

        ;; R
        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.load8_u
        local.get $delta
        i32.add
        local.tee $val
        i32.const 0
        i32.max_s
        i32.const 255
        i32.min_s
        local.set $val

        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        local.get $val
        i32.store8

        ;; G
        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 1
        i32.add
        i32.load8_u
        local.get $delta
        i32.add
        local.tee $val
        i32.const 0
        i32.max_s
        i32.const 255
        i32.min_s
        local.set $val

        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 1
        i32.add
        local.get $val
        i32.store8

        ;; B
        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 2
        i32.add
        i32.load8_u
        local.get $delta
        i32.add
        local.tee $val
        i32.const 0
        i32.max_s
        i32.const 255
        i32.min_s
        local.set $val

        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 2
        i32.add
        local.get $val
        i32.store8

        local.get $i
        i32.const 1
        i32.add
        local.set $i
        br $loop
      )
    )
  )

  ;; 색상 반전
  (func $invert (export "invert")
    (param $ptr i32)
    (param $count i32)
    (local $i i32)

    (block $done
      (loop $loop
        local.get $i
        local.get $count
        i32.ge_s
        br_if $done

        ;; R = 255 - R
        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        local.tee $ptr
        i32.const 255
        local.get $ptr
        i32.load8_u
        i32.sub
        i32.store8

        ;; G
        local.get $ptr
        i32.const 1
        i32.add
        i32.const 255
        local.get $ptr
        i32.const 1
        i32.add
        i32.load8_u
        i32.sub
        i32.store8

        ;; B
        local.get $ptr
        i32.const 2
        i32.add
        i32.const 255
        local.get $ptr
        i32.const 2
        i32.add
        i32.load8_u
        i32.sub
        i32.store8

        local.get $i
        i32.const 1
        i32.add
        local.set $i
        br $loop
      )
    )
  )

  ;; 이진화 (threshold)
  (func $threshold (export "threshold")
    (param $ptr i32)
    (param $count i32)
    (param $threshold i32)
    (local $i i32)
    (local $r i32)
    (local $g i32)
    (local $b i32)
    (local $avg i32)

    (block $done
      (loop $loop
        local.get $i
        local.get $count
        i32.ge_s
        br_if $done

        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.load8_u
        local.set $r

        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 1
        i32.add
        i32.load8_u
        local.set $g

        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 2
        i32.add
        i32.load8_u
        local.set $b

        local.get $r
        local.get $g
        i32.add
        local.get $b
        i32.add
        i32.const 3
        i32.div_u
        local.set $avg

        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        (if (i32.gt_u (local.get $avg) (local.get $threshold))
          (then (i32.const 255))
          (else (i32.const 0))
        )
        i32.store8

        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 1
        i32.add
        (if (i32.gt_u (local.get $avg) (local.get $threshold))
          (then (i32.const 255))
          (else (i32.const 0))
        )
        i32.store8

        local.get $ptr
        local.get $i
        i32.const 4
        i32.mul
        i32.add
        i32.const 2
        i32.add
        (if (i32.gt_u (local.get $avg) (local.get $threshold))
          (then (i32.const 255))
          (else (i32.const 0))
        )
        i32.store8

        local.get $i
        i32.const 1
        i32.add
        local.set $i
        br $loop
      )
    )
  )
)

(module
  (memory (export "memory") 40)   ;; 2.5MB (640x480 픽셀 + 블러 스크래치 수용)

  ;; 블러 중간 결과용 스크래치 버퍼 (600000부터)
  (global $scratch (mut i32) (i32.const 600000))

  ;; v를 lo..hi 범위로 클램프
  (func $clamp (param $v i32) (param $lo i32) (param $hi i32) (result i32)
    (i32.min_s (i32.max_s (local.get $v) (local.get $lo)) (local.get $hi)))

  ;; 0..255 클램프
  (func $clamp8 (param $v i32) (result i32)
    (call $clamp (local.get $v) (i32.const 0) (i32.const 255)))

  ;; 그레이스케일: gray = (299R + 587G + 114B + 500) / 1000
  (func (export "grayscale") (param $ptr i32) (param $count i32)
    (local $i i32)
    (local $r i32) (local $g i32) (local $b i32) (local $gray i32)
    (block $done
      (loop $loop
        (br_if $done (i32.ge_u (local.get $i) (local.get $count)))
        (local.set $r (i32.load8_u (i32.add (local.get $ptr) (local.get $i))))
        (local.set $g (i32.load8_u (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 1))))
        (local.set $b (i32.load8_u (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 2))))
        (local.set $gray
          (i32.div_s
            (i32.add
              (i32.add
                (i32.add
                  (i32.mul (local.get $r) (i32.const 299))
                  (i32.mul (local.get $g) (i32.const 587)))
                (i32.mul (local.get $b) (i32.const 114)))
              (i32.const 500))
            (i32.const 1000)))
        (i32.store8 (i32.add (local.get $ptr) (local.get $i)) (local.get $gray))
        (i32.store8 (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 1)) (local.get $gray))
        (i32.store8 (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 2)) (local.get $gray))
        (local.set $i (i32.add (local.get $i) (i32.const 4)))
        (br $loop))))

  ;; 세피아 변환
  (func (export "sepia") (param $ptr i32) (param $count i32)
    (local $i i32)
    (local $r i32) (local $g i32) (local $b i32)
    (block $done
      (loop $loop
        (br_if $done (i32.ge_u (local.get $i) (local.get $count)))
        (local.set $r (i32.load8_u (i32.add (local.get $ptr) (local.get $i))))
        (local.set $g (i32.load8_u (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 1))))
        (local.set $b (i32.load8_u (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 2))))
        (i32.store8 (i32.add (local.get $ptr) (local.get $i))
          (call $clamp8
            (i32.div_s
              (i32.add
                (i32.add (i32.mul (local.get $r) (i32.const 393)) (i32.mul (local.get $g) (i32.const 769)))
                (i32.mul (local.get $b) (i32.const 189)))
              (i32.const 1000))))
        (i32.store8 (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 1))
          (call $clamp8
            (i32.div_s
              (i32.add
                (i32.add (i32.mul (local.get $r) (i32.const 349)) (i32.mul (local.get $g) (i32.const 686)))
                (i32.mul (local.get $b) (i32.const 168)))
              (i32.const 1000))))
        (i32.store8 (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 2))
          (call $clamp8
            (i32.div_s
              (i32.add
                (i32.add (i32.mul (local.get $r) (i32.const 272)) (i32.mul (local.get $g) (i32.const 534)))
                (i32.mul (local.get $b) (i32.const 131)))
              (i32.const 1000))))
        (local.set $i (i32.add (local.get $i) (i32.const 4)))
        (br $loop))))

  ;; 밝기 조절 (delta: -255 ~ 255)
  (func (export "brightness") (param $ptr i32) (param $count i32) (param $delta i32)
    (local $i i32)
    (block $done
      (loop $loop
        (br_if $done (i32.ge_u (local.get $i) (local.get $count)))
        (i32.store8 (i32.add (local.get $ptr) (local.get $i))
          (call $clamp8 (i32.add (i32.load8_u (i32.add (local.get $ptr) (local.get $i))) (local.get $delta))))
        (i32.store8 (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 1))
          (call $clamp8 (i32.add (i32.load8_u (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 1))) (local.get $delta))))
        (i32.store8 (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 2))
          (call $clamp8 (i32.add (i32.load8_u (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 2))) (local.get $delta))))
        (local.set $i (i32.add (local.get $i) (i32.const 4)))
        (br $loop))))

  ;; 대비 조절 (factor: 백분율, 50=0.5배 ~ 300=3배)
  (func (export "contrast") (param $ptr i32) (param $count i32) (param $factor i32)
    (local $i i32)
    (local $v i32)
    (block $done
      (loop $loop
        (br_if $done (i32.ge_u (local.get $i) (local.get $count)))
        (local.set $v (i32.load8_u (i32.add (local.get $ptr) (local.get $i))))
        (i32.store8 (i32.add (local.get $ptr) (local.get $i))
          (call $clamp8
            (i32.add (i32.const 128)
              (i32.div_s
                (i32.mul (i32.sub (local.get $v) (i32.const 128)) (local.get $factor))
                (i32.const 100)))))
        (local.set $v (i32.load8_u (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 1))))
        (i32.store8 (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 1))
          (call $clamp8
            (i32.add (i32.const 128)
              (i32.div_s
                (i32.mul (i32.sub (local.get $v) (i32.const 128)) (local.get $factor))
                (i32.const 100)))))
        (local.set $v (i32.load8_u (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 2))))
        (i32.store8 (i32.add (i32.add (local.get $ptr) (local.get $i)) (i32.const 2))
          (call $clamp8
            (i32.add (i32.const 128)
              (i32.div_s
                (i32.mul (i32.sub (local.get $v) (i32.const 128)) (local.get $factor))
                (i32.const 100)))))
        (local.set $i (i32.add (local.get $i) (i32.const 4)))
        (br $loop))))

  ;; 가로 방향 박스 블러 (scratch에 기록)
  (func $hblur
    (param $src i32) (param $dst i32) (param $w i32) (param $h i32) (param $r i32)
    (local $y i32) (local $x i32) (local $c i32) (local $k i32)
    (local $xx i32) (local $sum i32) (local $n i32)
    (block $doneY
      (loop $loopY
        (br_if $doneY (i32.ge_u (local.get $y) (local.get $h)))
        (local.set $x (i32.const 0))
        (block $doneX
          (loop $loopX
            (br_if $doneX (i32.ge_u (local.get $x) (local.get $w)))
            (local.set $c (i32.const 0))
            (block $doneC
              (loop $loopC
                (br_if $doneC (i32.ge_u (local.get $c) (i32.const 4)))
                (local.set $sum (i32.const 0))
                (local.set $n (i32.const 0))
                (local.set $k (i32.const 0))
                (block $doneK
                  (loop $loopK
                    (br_if $doneK (i32.gt_u (local.get $k) (i32.mul (local.get $r) (i32.const 2))))
                    (local.set $xx
                      (call $clamp
                        (i32.add (local.get $x) (i32.sub (local.get $k) (local.get $r)))
                        (i32.const 0)
                        (i32.sub (local.get $w) (i32.const 1))))
                    (local.set $sum
                      (i32.add (local.get $sum)
                        (i32.load8_u
                          (i32.add
                            (i32.add (local.get $src)
                              (i32.mul
                                (i32.add (i32.mul (local.get $y) (local.get $w)) (local.get $xx))
                                (i32.const 4)))
                            (local.get $c)))))
                    (local.set $n (i32.add (local.get $n) (i32.const 1)))
                    (local.set $k (i32.add (local.get $k) (i32.const 1)))
                    (br $loopK)))
                (i32.store8
                  (i32.add
                    (i32.add (local.get $dst)
                      (i32.mul
                        (i32.add (i32.mul (local.get $y) (local.get $w)) (local.get $x))
                        (i32.const 4)))
                    (local.get $c))
                  (i32.div_u (local.get $sum) (local.get $n)))
                (local.set $c (i32.add (local.get $c) (i32.const 1)))
                (br $loopC)))
            (local.set $x (i32.add (local.get $x) (i32.const 1)))
            (br $loopX)))
        (local.set $y (i32.add (local.get $y) (i32.const 1)))
        (br $loopY))))

  ;; 세로 방향 박스 블러 (src에서 읽어 dst에 기록)
  (func $vblur
    (param $src i32) (param $dst i32) (param $w i32) (param $h i32) (param $r i32)
    (local $y i32) (local $x i32) (local $c i32) (local $k i32)
    (local $yy i32) (local $sum i32) (local $n i32)
    (block $doneY
      (loop $loopY
        (br_if $doneY (i32.ge_u (local.get $y) (local.get $h)))
        (local.set $x (i32.const 0))
        (block $doneX
          (loop $loopX
            (br_if $doneX (i32.ge_u (local.get $x) (local.get $w)))
            (local.set $c (i32.const 0))
            (block $doneC
              (loop $loopC
                (br_if $doneC (i32.ge_u (local.get $c) (i32.const 4)))
                (local.set $sum (i32.const 0))
                (local.set $n (i32.const 0))
                (local.set $k (i32.const 0))
                (block $doneK
                  (loop $loopK
                    (br_if $doneK (i32.gt_u (local.get $k) (i32.mul (local.get $r) (i32.const 2))))
                    (local.set $yy
                      (call $clamp
                        (i32.add (local.get $y) (i32.sub (local.get $k) (local.get $r)))
                        (i32.const 0)
                        (i32.sub (local.get $h) (i32.const 1))))
                    (local.set $sum
                      (i32.add (local.get $sum)
                        (i32.load8_u
                          (i32.add
                            (i32.add (local.get $src)
                              (i32.mul
                                (i32.add (i32.mul (local.get $yy) (local.get $w)) (local.get $x))
                                (i32.const 4)))
                            (local.get $c)))))
                    (local.set $n (i32.add (local.get $n) (i32.const 1)))
                    (local.set $k (i32.add (local.get $k) (i32.const 1)))
                    (br $loopK)))
                (i32.store8
                  (i32.add
                    (i32.add (local.get $dst)
                      (i32.mul
                        (i32.add (i32.mul (local.get $y) (local.get $w)) (local.get $x))
                        (i32.const 4)))
                    (local.get $c))
                  (i32.div_u (local.get $sum) (local.get $n)))
                (local.set $c (i32.add (local.get $c) (i32.const 1)))
                (br $loopC)))
            (local.set $x (i32.add (local.get $x) (i32.const 1)))
            (br $loopX)))
        (local.set $y (i32.add (local.get $y) (i32.const 1)))
        (br $loopY))))

  ;; 박스 블러 (가로+세로 2패스), 원본과 중간 버퍼 교환
  (func (export "boxBlur")
    (param $ptr i32) (param $w i32) (param $h i32) (param $r i32)
    (call $hblur (local.get $ptr) (global.get $scratch) (local.get $w) (local.get $h) (local.get $r))
    (call $vblur (global.get $scratch) (local.get $ptr) (local.get $w) (local.get $h) (local.get $r)))

  ;; 엣지 검출: 위/왼쪽 픽셀과의 차분 합이 임계값을 넘으면 흰색, 아니면 검정
  (func (export "edgeDetect")
    (param $ptr i32) (param $w i32) (param $h i32) (param $threshold i32)
    (local $y i32) (local $x i32) (local $c i32)
    (local $idx i32) (local $gray i32) (local $dx i32) (local $dy i32)
    (block $doneY
      (loop $loopY
        (br_if $doneY (i32.ge_u (local.get $y) (local.get $h)))
        (local.set $x (i32.const 0))
        (block $doneX
          (loop $loopX
            (br_if $doneX (i32.ge_u (local.get $x) (local.get $w)))
            (local.set $idx
              (i32.mul (i32.add (i32.mul (local.get $y) (local.get $w)) (local.get $x)) (i32.const 4)))
            (local.set $gray (i32.load8_u (i32.add (local.get $ptr) (local.get $idx))))
            ;; dx: 왼쪽 픽셀과의 차이
            (local.set $dx
              (i32.sub (local.get $gray)
                (i32.load8_u
                  (i32.add (local.get $ptr)
                    (i32.mul (i32.add (i32.mul (local.get $y) (local.get $w))
                      (call $clamp (i32.sub (local.get $x) (i32.const 1)) (i32.const 0) (i32.sub (local.get $w) (i32.const 1))))
                      (i32.const 4))))))
            (local.set $dx (call $clamp (local.get $dx) (i32.const 0) (i32.const 255)))
            ;; dy: 위 픽셀과의 차이
            (local.set $dy
              (i32.sub (local.get $gray)
                (i32.load8_u
                  (i32.add (local.get $ptr)
                    (i32.mul (i32.add (i32.mul
                      (call $clamp (i32.sub (local.get $y) (i32.const 1)) (i32.const 0) (i32.sub (local.get $h) (i32.const 1)))
                      (local.get $w)) (local.get $x))
                      (i32.const 4))))))
            (local.set $dy (call $clamp (local.get $dy) (i32.const 0) (i32.const 255)))
            (local.set $c (i32.const 0))
            (block $doneC
              (loop $loopC
                (br_if $doneC (i32.ge_u (local.get $c) (i32.const 3)))
                (if (i32.gt_u (i32.add (local.get $dx) (local.get $dy)) (local.get $threshold))
                  (then (i32.store8 (i32.add (local.get $ptr) (i32.add (local.get $idx) (local.get $c))) (i32.const 255)))
                  (else (i32.store8 (i32.add (local.get $ptr) (i32.add (local.get $idx) (local.get $c))) (i32.const 0))))
                (local.set $c (i32.add (local.get $c) (i32.const 1)))
                (br $loopC)))
            (local.set $x (i32.add (local.get $x) (i32.const 1)))
            (br $loopX)))
        (local.set $y (i32.add (local.get $y) (i32.const 1)))
        (br $loopY))))
)

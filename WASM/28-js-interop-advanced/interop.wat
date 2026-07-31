(module
  (memory (export "memory") 32)   ;; 2MB (40만 개 정수 벤치마크 수용)

  ;; WASM 내부 정적 문자열 (주소 256부터)
  (data (i32.const 256) "Hello from WASM!\n")

  ;; JS가 인코딩한 문자열의 위치/길이를 기억
  (global $strPtr (mut i32) (i32.const 0))
  (global $strLen (mut i32) (i32.const 0))

  (func (export "rememberString") (param $ptr i32) (param $len i32)
    (global.set $strPtr (local.get $ptr))
    (global.set $strLen (local.get $len)))

  (func (export "stringPtr") (result i32) (global.get $strPtr))
  (func (export "stringLen") (result i32) (global.get $strLen))

  ;; 정적 문자열 포인터
  (func (export "helloPtr") (result i32) (i32.const 256))

  ;; zero-copy 변환: ptr부터 len 바이트(정수 배열)의 모든 요소를 2배로
  (func (export "transform") (param $ptr i32) (param $len i32)
    (local $i i32)
    (block $done
      (loop $loop
        (br_if $done (i32.ge_u (local.get $i) (local.get $len)))
        (i32.store
          (i32.add (local.get $ptr) (local.get $i))
          (i32.mul
            (i32.load (i32.add (local.get $ptr) (local.get $i)))
            (i32.const 2)))
        (local.set $i (i32.add (local.get $i) (i32.const 4)))
        (br $loop))))

  ;; 합계 계산 (WASM 루프 vs JS 루프 벤치마크용)
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

  ;; 문자열 길이를 계산해 반환 (WASM 쪽 반복 처리 예)
  (func (export "strLenAt") (param $ptr i32) (result i32)
    (local $i i32)
    (block $done
      (loop $loop
        (br_if $done
          (i32.eqz (i32.load8_u (i32.add (local.get $ptr) (local.get $i)))))
        (local.set $i (i32.add (local.get $i) (i32.const 1)))
        (br $loop)))
    (local.get $i))
)

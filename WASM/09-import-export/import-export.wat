(module
  ;; JS 콘솔 로그 함수 임포트
  (import "env" "log" (func $log (param i32)))

  ;; JS 메모리 임포트
  (import "env" "memory" (memory 1))

  ;; 숫자를 받아 1을 더해 반환 (export)
  (func $add_one (export "add_one") (param $x i32) (result i32)
    local.get $x
    i32.const 1
    i32.add
  )

  ;; 여러 값을 export
  (func $mul (export "mul") (param $a i32) (param $b i32) (result i32)
    local.get $a
    local.get $b
    i32.mul
  )

  ;; 내부 함수 (export되지 않음)
  (func $internal_add (param $a i32) (param $b i32) (result i32)
    local.get $a
    local.get $b
    i32.add
  )

  ;; export + import 함께 사용
  (func $process (export "process") (param $value i32) (result i32)
    local.get $value
    call $internal_add     ;; 내부 함수 호출
    local.tee $value       ;; 결과 저장
    call $log              ;; JS 로그 호출 (import)
    local.get $value       ;; 최종 결과 반환
  )

  ;; 전역 변수 export
  (global $version (export "version") i32 (i32.const 1))

  ;; 메모리 export
  (export "memory" (memory 0))
)

(module
  ;; 모듈 섹션의 기본 구조를 보여주는 예제

  ;; 함수 선언
  (func $greet (export "greet") (param $name i32) (param $len i32) (result i32)
    ;; 단순히 길이를 반환
    local.get $len
  )

  ;; 메모리 선언 (최소 1페이지 = 64KB, 최대 1페이지)
  (memory (export "memory") 1 1)

  ;; 전역 변수 선언
  (global $counter (export "counter") (mut i32) (i32.const 0))

  ;; 전역 변수 증가 함수
  (func $increment (export "increment") (result i32)
    local.get $counter
    i32.const 1
    i32.add
    global.set $counter
    local.get $counter
  )

  ;; 데이터 섹션 (메모리에 초기 데이터 저장)
  (data (i32.const 0) "Hello, WASM!")
)

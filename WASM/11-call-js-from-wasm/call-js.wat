(module
  ;; JS 함수 임포트
  (import "js" "print" (func $js_print (param i32)))
  (import "js" "double_and_print" (func $js_double_and_print (param i32) (result i32)))
  (import "js" "get_value" (func $js_get_value (result i32)))
  (import "js" "generate_id" (func $js_generate_id (result i32)))

  ;; WASM → JS 호출
  (func $call_js (export "call_js") (param $x i32) (result i32)
    local.get $x
    call $js_print         ;; JS print 호출
    local.get $x
    call $js_double_and_print
    local.set $x
    local.get $x
  )

  ;; JS에서 값 가져와서 처리
  (func $get_and_process (export "get_and_process") (result i32)
    call $js_get_value
    i32.const 10
    i32.add
  )

  ;; JS 함수를 여러 번 호출
  (func $generate_ids (export "generate_ids") (param $count i32) (result i32)
    (local $i i32)
    (local $sum i32)
    (block $done
      (loop $loop
        local.get $i
        local.get $count
        i32.ge_s
        br_if $done

        local.get $sum
        call $js_generate_id
        i32.add
        local.set $sum

        local.get $i
        i32.const 1
        i32.add
        local.set $i
        br $loop
      )
    )
    local.get $sum
  )
)

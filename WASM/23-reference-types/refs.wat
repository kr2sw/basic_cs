(module
  ;; JS에서 주입받는 호스트 함수: 받은 externref를 콘솔에 로그
  (import "env" "logObject" (func $logObject (param externref)))

  ;; 일변 함수 시그니처 (콜백용)
  (type $unary (func (param i32) (result i32)))

  ;; 저장용 externref 전역 변수 (JS에서 직접 get/set 가능하게 export)
  (global $saved (export "saved") (mut externref) (ref.null extern))

  ;; funcref 테이블: 콜백 함수 보관소
  (table $callbacks 3 funcref)
  (elem (i32.const 0) $cbA $cbB $cbNull)

  ;; --- 콜백 함수들 ---
  (func $cbA (type $unary) (param $x i32) (result i32)
    (i32.add (local.get $x) (i32.const 100)))

  (func $cbB (type $unary) (param $x i32) (result i32)
    (i32.mul (local.get $x) (i32.const 2)))

  (func $cbNull (type $unary) (param $x i32) (result i32)
    (unreachable))  ;; 호출되면 트랩

  ;; externref 저장/조회
  (func (export "saveRef") (param $obj externref)
    (global.set $saved (local.get $obj)))

  (func (export "getSaved") (result externref)
    (global.get $saved))

  ;; 저장된 참조가 널인지 검사
  (func (export "isSavedNull") (result i32)
    (ref.is_null (global.get $saved)))

  ;; 받은 externref를 JS 함수에 다시 넘겨 로그
  (func (export "logRef") (param $obj externref)
    (call $logObject (local.get $obj)))

  ;; ref.null 반환
  (func (export "getNull") (result externref)
    (ref.null extern))

  ;; 테이블 콜백 간접 호출 (널/범위 밖은 트랩)
  (func (export "invokeCallback") (param $idx i32) (param $x i32) (result i32)
    (call_indirect $unary (local.get $x) (local.get $idx)))

  ;; call_ref: funcref를 직접 호출
  (func (export "applyRef") (param $f funcref) (param $x i32) (result i32)
    (call_ref $unary (local.get $x) (local.get $f)))

  ;; 테이블 항목이 널인지 확인
  (func (export "isNullFunc") (param $idx i32) (result i32)
    (ref.is_null (table.get $callbacks (local.get $idx))))
)

# 23: 참조 타입 — externref, funcref, ref.null

참조 타입(Reference Types) 프로포절은 WASM 값 타입에 `externref`(호스트 객체 참조)와 `funcref`(함수 참조)를 추가합니다. 기존에는 메모리 주소(정수)로 우회했던 객체/함수 참조를 타입 시스템에서 직접 다룰 수 있게 되었습니다.

## 참조 타입 종류

| 타입 | 의미 | 예 |
|------|------|-----|
| `externref` | 호스트(JS) 객체 참조 | DOM 요소, Map, class 인스턴스 |
| `funcref` | WASM 함수 참조 | 테이블에 저장되는 함수 |
| `ref.null extern` | 널 외부 참조 | null |
| `ref.null func` | 널 함수 참조 | null |

## externref

`externref`는 숫자로 변환할 수 없는 "객체 그대로"의 참조입니다. JS에서 객체를 넘기면 WASM은 내부에서 다시 빼낼 수 있습니다.

```wat
(global $saved (mut externref) (ref.null extern))

;; JS에서 받은 객체 저장
(func (export "saveRef") (param $obj externref)
  (global.set $saved (local.get $obj)))

;; 저장된 객체를 그대로 반환
(func (export "getSaved") (result externref)
  (global.get $saved))

;; 널 여부 검사
(func (export "isNull") (result i32)
  (ref.is_null (global.get $saved)))
```

```js
// JS 측 — 일반 객체 그대로 전달/반환
const obj = { name: "example" };
wasm.exports.saveRef(obj);
const back = wasm.exports.getSaved();   // back === obj (동일 참조)
```

## funcref와 call_ref

`funcref`는 함수를 가리키는 참조입니다. `call_ref`로 직접 호출하거나, 테이블에 보관했다가 `call_indirect`로 호출할 수 있습니다.

```wat
(func $double (param i32) (result i32)
  (i32.mul (local.get 0) (i32.const 2)))

;; 함수 참조를 받아 호출
(func (export "apply") (param $f funcref) (param $x i32) (result i32)
  (call_ref (param i32) (result i32) (local.get $x) (local.get $f)))
```

## 실행

```bash
# call_ref(함수 참조 호출)는 function-references 프로포절이므로
# 최신 wabt에서는 기본 활성화, 구버전에서는 플래그가 필요할 수 있습니다
wat2wasm refs.wat -o refs.wasm --enable-function-references
npx http-server .
```

브라우저에서 JS 객체를 WASM에 넘겼다가 다시 받아 동일 참조인지 확인해보세요. `ref.null`과 `ref.is_null`로 빈 참조 처리를 연습해보세요.

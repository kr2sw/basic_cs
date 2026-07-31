# 25: 다중값 — multi-value 반환, 다중 메모리

Multi-value 프로포절은 함수가 2개 이상의 값을 반환하고, 블록/루프가 여러 값을 주고받을 수 있게 합니다. 여러 개의 함수 호출을 합성(composition)할 때 임시 구조체나 전역 변수 없이 간결하게 표현할 수 있습니다.

## 다중값 반환

```wat
;; 몫과 나머지를 함께 반환
(func (export "divmod") (param $a i32) (param $b i32) (result i32 i32)
  (local.get $a) (local.get $b) i32.div_s
  (local.get $a) (local.get $b) i32.rem_s)
```

JS에서는 배열로 받습니다.

```js
const [quotient, remainder] = wasm.exports.divmod(17, 5); // [3, 2]
```

## 값 합성

다중값은 스택에 쌓인 결과를 그대로 다음 호출의 인자로 넘기는 합성 패턴을 가능하게 합니다.

```wat
;; 두 수의 min과 max를 함께 반환
(func (export "minmax") (param $a i32) (param $b i32) (result i32 i32)
  (local.get $a) (local.get $b) (call $min)
  (local.get $a) (local.get $b) (call $max))
```

## 블록의 다중 결과

블록도 여러 값을 결과로 가질 수 있어, 조건부로 서로 다른 값 묶음을 만들 수 있습니다.

```wat
(block $both (result i32 i32)
  ...
)
```

## 다중 메모리

Multi-memory 프로포절은 모듈이 여러 개의 독립 메모리를 가질 수 있게 합니다. 분리된 도메인(예: 입력 버퍼와 출력 버퍼)을 하나의 주소 공간에서 관리하지 않고 격리할 수 있습니다.

```wat
(memory (export "memory") 1)         ;; 기본 메모리
(memory $aux (export "aux") 1)       ;; 두 번째 메모리

;; 두 번째 메모리에도 load/store 가능
(i32.store (memory $aux) (i32.const 0) (i32.const 42))
```

`wat2wasm`에서 다중 메모리는 `--enable-multi-memory` 플래그가 필요할 수 있습니다.

## 실행

```bash
wat2wasm multi.wat -o multi.wasm --enable-multi-memory
npx http-server .
```

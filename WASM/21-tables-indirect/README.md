# 21: 테이블과 간접 호출 — table, elem, call_indirect, 함수 포인터

WASM의 `table`은 함수 참조(또는 참조 타입)를 저장하는 배열입니다. `call_indirect`로 테이블의 특정 인덱스에 있는 함수를 간접적으로 호출할 수 있어, C의 함수 포인터와 같은 동적 디스패치를 구현할 수 있습니다.

## 테이블 선언

```wat
;; funcref 테이블, 초기 크기 4
(table $funcs 4 funcref)

;; 0번부터 $add, $sub, $mul, $div를 채움
(elem (i32.const 0) $add $sub $mul $div)
```

## call_indirect

`call_indirect`는 스택에서 인덱스를 꺼내 해당 함수를 호출합니다. 인덱스가 범위를 벗어나거나 타입이 맞지 않으면 `unreachable` 트랩이 발생합니다.

```wat
(func (export "compute") (param $a i32) (param $b i32) (param $fn i32) (result i32)
  (call_indirect (type $binop) (local.get $a) (local.get $b) (local.get $fn))
)
```

- **동적 디스패치**: 런타임에 함수 선택이 가능해 콜백/이벤트 시스템, 가상 함수 테이블(vtable)을 구현할 수 있습니다.
- **타입 검사**: 테이블 항목의 시그니처가 호출부의 `type`과 다르면 트랩 → 타입 안전성 보장.

## table.get / table.set

테이블은 실행 중에도 읽고 쓸 수 있습니다.

```wat
(func (export "getFunc") (param $i i32) (result funcref)
  (table.get $funcs (local.get $i)))

(func (export "setFunc") (param $i i32) (param $f funcref)
  (table.set $funcs (local.get $i) (local.get $f)))
```

## 직접 호출 vs 간접 호출

| 방식 | 명령어 | 특성 |
|------|--------|------|
| 직접 호출 | `call $func` | 컴파일 타임에 함수 확정, 가장 빠름 |
| 간접 호출 | `call_indirect (type $t)` | 런타임 인덱스로 함수 선택, 타입 검사 추가 |
| 참조 호출 | `call_ref $t` | `funcref` 값으로 호출, 널 검사 필요 |

간접 호출은 테이블 조회 + 시그니처 타입 검사가 추가되어 직접 호출보다 조금 느리지만, **다형성(가상 함수)**과 **콜백 시스템**을 만들 수 있습니다.

## 실행

```bash
# table/call_indirect는 기본 지원. call_ref는 function-references 프로포절 플래그 필요
wat2wasm table.wat -o table.wasm --enable-function-references
npx http-server .
```

브라우저에서 연산자 선택 버튼으로 `compute`의 인덱스를 바꿔가며 호출해보세요.

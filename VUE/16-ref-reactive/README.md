# 16: ref & reactive — 반응형 데이터

## ref

기본형(Primitive) 값을 반응형으로 만듭니다.
- `.value`로 접근/수정
- 템플릿에서는 자동 언래핑 (`.value` 불필요)

## reactive

객체(Object)를 반응형으로 만듭니다.
- 직접 속성 접근/수정 가능
- ref와 달리 `.value` 불필요
- 재할당하면 반응성 사라짐

## toRefs / toRef

reactive 객체의 각 속성을 ref로 변환합니다.

## shallowRef / shallowReactive

깊은 반응성을 만들지 않습니다. (성능 최적화)

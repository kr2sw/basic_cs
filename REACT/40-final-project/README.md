# 40: 종합 프로젝트 — 할일 관리 앱 (Full Integration)

지금까지 배운 모든 것을 하나의 할일 관리 앱에 통합합니다.

## 통합된 개념

- **Context + useReducer** — 상태 구조 관리 (22장)
- **localStorage** — 영속화 (재방문 시 복원)
- **useMemo / useCallback** — 파생 통계와 콜백 안정화 (21장)
- **커스텀 훅** — `useLocalStorage`로 로직 분리 (13·21장)
- **접근성** — label, 키보드, `aria-live` (32장)
- **성능** — `React.memo`로 불필요한 리렌더 방지 (27장)

## 아키텍처 (데이터 흐름)

```
localStorage ──초기값──▶ useReducer (todoReducer)
      ▲                        │ dispatch
      │ 저장                   ▼
      └───── useEffect ◀── 액션(add/toggle/remove/clear)
```

컴포넌트는 `dispatch`를 통해서만 상태를 바꾸고, 파생값(필터·통계)은 `useMemo`로 계산합니다. 읽기 전용 `TodoItem`은 `memo`로 감싸 리렌더를 줄입니다.

## 동작 요약

1. 초기 로딩: `localStorage`에 저장된 할일을 리듀서 초기 상태로 복원
2. 추가/토글/삭제/완료-지우기: `dispatch` 액션으로 변경
3. 변경될 때마다 `useEffect`가 `localStorage`에 자동 저장
4. 필터(전체/진행/완료)와 통계는 `useMemo`로 최소 계산

## 할일 데이터 구조

```js
{
  id: 1720000000000,        // Date.now()로 생성
  text: '리액트 복습하기',    // 사용자 입력
  done: false,               // 완료 여부
  priority: 'normal' | 'high'
}
```

리듀서가 이 구조를 지키는 액션만 받으므로 상태 변형이 예측 가능합니다. 저장 키는 `final-todos`이며, 초기화하고 싶다면 DevTools에서 `localStorage.removeItem('final-todos')`를 실행하면 됩니다.

## 확장 아이디어

- 편집 기능 추가: 액션 `update`를 리듀서에 추가
- 우선순위 필드 추가: `context`와 스키마 확장
- TanStack Query로 서버 동기화 (24장)
- 테스트 작성: MSW + user-event (29장)
- PWA로 오프라인 지원 (39장)

## 실행

```bash
npm install && npm run dev
```

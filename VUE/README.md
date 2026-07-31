# Vue.js 기초 강의 (20개 챕터)

Vue.js 프레임워크의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 역사

Vue.js는 2014년 Google의 전 직원이었던 Evan You가 개인 프로젝트로 시작했습니다. AngularJS(1.x)의 양방향 데이터 바인딩과 React의 가상 DOM의 장점을 결합하면서도 더 가볍고 배우기 쉬운 프레임워크를 목표로 했습니다. 2015년 Vue 1.0, 2016년 Vue 2.0이 출시되었고, 특히 2016년 Alibaba 그룹이 Vue를 채택하면서 중국 시장에서 급성장했습니다. 2020년 Vue 3.0(Composition API 도입)이 출시되었고, 2023년에는 Vapor Mode와 Vite 기반의 공식 빌드 도구로 전환되었습니다. 현재는 React, Angular와 함께 "Big 3" 프론트엔드 프레임워크 중 하나입니다.

## 특징

- **점진적 도입 가능**: CDN 스크립트 하나로 시작하여 점진적으로 규모 확장 가능
- **반응형 시스템**: Proxy 기반의 세밀한 반응성, 자동 의존성 추적
- **싱글 파일 컴포넌트(SFC)**: 템플릿, 스크립트, 스타일을 하나의 `.vue` 파일에 통합
- **Composition API**: Options API의 한계를 극복한 로직 구성 방식
- **가상 DOM**: 효율적인 UI 업데이트를 위한 가상 DOM 엔진
- **직관적인 문법**: HTML 기반 템플릿 문법으로 낮은 학습 곡선
- **공식 라우터/상태 관리**: Vue Router와 Pinia를 공식 지원하여 일관된 생태계

## 실행

```bash
npm create vue@latest  # 프로젝트 생성
cd 프로젝트 && npm install && npm run dev
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | Vue.js 소개, CDN 설치, create-vue, 프로젝트 구조 |
| 02 | Template Syntax | 템플릿 문법, 보간법, directives, raw HTML |
| 03 | Data & Event Binding | v-bind, v-on, data 속성, 메서드 |
| 04 | Computed & Watch | computed 속성, watch, getter/setter |
| 05 | Class & Style Binding | v-bind:class, v-bind:style, 배열/객체 문법 |
| 06 | Conditional Rendering | v-if, v-else-if, v-else, v-show, template |
| 07 | List Rendering | v-for, key, 배열/객체 순회, 필터링 |
| 08 | Event Handling | v-on, 이벤트 수식어, 키 수식어, 마우스 수식어 |
| 09 | Form Input Binding | v-model, 양방향 바인딩, 수식어, 다양한 입력 타입 |
| 10 | Components | 컴포넌트 정의, 등록, 컴포넌트 간 통신 기초 |
| 11 | Component Props | props 선언, 타입 검증, props drilling |
| 12 | Component Emits | emits, emit 이벤트, v-model 커스텀 |
| 13 | Slots | slot, named slot, scoped slot, fallback |
| 14 | Lifecycle Hooks | onMounted, onUpdated, onUnmounted, onBeforeMount |
| 15 | Composition API | setup(), ref, reactive, Composition API vs Options API |
| 16 | ref & reactive | ref, reactive, toRefs, shallowRef, triggerRef |
| 17 | Computed & Watch (Composition) | computed, watch, watchEffect, watchPostEffect |
| 18 | Provide & Inject | provide, inject, 전역 상태 공유, reactivity |
| 19 | Vue Router | router 설치, routes, router-link, router-view, 네비게이션 |
| 20 | Composition Patterns | composables, 커스텀 훅, 재사용성 패턴 |

## 목차 (중급 21-40)

| # | 주제 | 설명 |
|---|------|------|
| 21 | Composition Advanced | Composition API 심화, 라이프사이클, ref vs reactive, 함수 분리 |
| 22 | Pinia | 상태 관리, store, actions, getters |
| 23 | Router Advanced | 라우터 심화, 가드, lazy loading, 메타 필드 |
| 24 | Forms Validation | 폼 검증, VeeValidate, 커스텀 규칙 |
| 25 | HTTP Axios | HTTP 통신, Axios 인터셉터, API 레이어 패턴 |
| 26 | TypeScript + Vue | TypeScript, script setup 타입, props 타입 |
| 27 | Reusable Components | 재사용 컴포넌트, v-model 패턴, composable props |
| 28 | Teleport & Suspense | Teleport, Suspense, 모달, 비동기 컴포넌트 |
| 29 | Render Functions | 렌더 함수, h(), VNode, JSX |
| 30 | Custom Directives | 커스텀 디렉티브, v-focus, v-click-outside |
| 31 | Plugins | 플러그인 개발, app.use, provide/inject |
| 32 | Testing | 테스팅, Vitest, Vue Test Utils, e2e 개념 |
| 33 | Performance | 성능 최적화, defineAsyncComponent, memoization, v-memo |
| 34 | Transitions & Animations | 전환과 애니메이션, Transition, TransitionGroup |
| 35 | SSR & Nuxt | SSR 개념, hydration, Nuxt 시작 |
| 36 | Nuxt Advanced | Nuxt 심화, data fetching, middleware, layouts |
| 37 | Composables Deep | 컴포저블 심화, useMouse, useFetch 구현, 패턴 |
| 38 | Accessibility | 접근성, ARIA, 포커스 관리, 키보드 |
| 39 | Security | 보안, XSS, v-html 위험, CSP, 인증 가드 |
| 40 | Final Project | 종합 프로젝트, 대시보드 앱 (전체 통합) |

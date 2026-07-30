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

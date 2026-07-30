# React 기초 (20개 챕터)

React 라이브러리의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 역사

React는 2011년 Facebook(현 Meta)의 Jordan Walke가 뉴스피드에 광고를 효율적으로 표시하기 위해 내부 프로젝트로 개발했습니다. 2013년 JSConf US에서 오픈소스로 공개되었고, 가상 DOM(Virtual DOM)과 단방향 데이터 흐름이 큰 주목을 받았습니다. 2015년 React Native가 발표되어 모바일 앱 개발로 확장되었고, 2016년에는 React의 설계 철학을 계승한 Vue.js와 Angular와 함께 "Big 3" 프론트엔드 프레임워크로 자리잡았습니다. 2019년 Hooks(React 16.8)가 도입되면서 함수형 컴포넌트와 상태 관리의 패러다임이 혁신적으로 변화했습니다. 2023년 React Server Components와 App Router가 포함된 Next.js 13이 발표되었습니다.

## 특징

- **가상 DOM(Virtual DOM)**: 실제 DOM 조작을 최소화하여 성능 최적화
- **컴포넌트 기반**: 독립적이고 재사용 가능한 UI 컴포넌트로 구성
- **단방향 데이터 흐름**: props를 통한 명시적 데이터 전달 (디버깅 용이)
- **JSX**: JavaScript 내에서 XML/HTML 같은 선언적 UI 작성
- **Hooks**: useState, useEffect 등 함수형 컴포넌트에서 상태와 생명주기 관리
- **방대한 생태계**: React Router, Redux, Next.js, Gatsby 등 수많은 도구와 라이브러리
- **선언적 UI**: 상태에 따라 UI가 자동으로 업데이트되는 선언적 패러다임

## 실행 방법

```bash
# 각 챕터 디렉토리에서 vite로 실행
cd REACT/01-introduction
npm install
npm run dev
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | React 소개, createRoot, JSX 기본 |
| 02 | JSX | JSX 표현식, 조건부 렌더링, Fragments |
| 03 | Components & Props | 함수 컴포넌트, props 전달, 기본값 |
| 04 | State & useState | useState, 상태 업데이트, 배열/객체 상태 |
| 05 | Event Handling | onClick/onChange/onSubmit, 합성 이벤트 |
| 06 | Conditional Rendering | &&, 삼항연산자, if/else, 조건부 className |
| 07 | Lists & Keys | map, filter, key prop, 리스트 재정렬 |
| 08 | Forms | 제어 컴포넌트, input/select/checkbox, 검증 |
| 09 | useEffect | useEffect, 의존성 배열, cleanup, 데이터 패칭 |
| 10 | useRef & DOM | useRef, forwardRef, DOM 조작 |
| 11 | Context API | createContext, useContext, Provider 패턴 |
| 12 | useReducer | useReducer, dispatch, 복잡한 상태 로직 |
| 13 | Custom Hooks | 커스텀 Hook, useLocalStorage, useFetch |
| 14 | React Router | BrowserRouter, Routes, Route, Link, useParams |
| 15 | Styling | CSS Modules, inline styles, Styled Components |
| 16 | Error Handling | ErrorBoundary, try/catch, fallback UI |
| 17 | Performance | React.memo, useMemo, useCallback, Suspense |
| 18 | Portals & Fragments | createPortal, Fragment, 모달 예제 |
| 19 | Testing | React Testing Library, jest, screen, fireEvent |
| 20 | Deployment | 빌드, 환경변수, Netlify/Vercel 배포 |

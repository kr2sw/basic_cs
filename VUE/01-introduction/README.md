# 01: Introduction — Vue.js 소개

## Vue.js란?

Vue.js는 Evan You가 만든 **점진적인 Progressive JavaScript 프레임워크**입니다.

### 주요 특징

- **반응형 (Reactivity)**: 데이터 변경이 자동으로 화면에 반영
- **컴포넌트 기반**: UI를 재사용 가능한 컴포넌트로 분할
- **SFC (Single File Component)**: `.vue` 파일에 template/script/style 통합

## 설치 방법

### CDN
```html
<script src="https://unpkg.com/vue@3/dist/vue.global.prod.js"></script>
```

### create-vue (권장)
```bash
npm create vue@latest
cd my-project
npm install
npm run dev
```

## 프로젝트 구조
```
src/
├── assets/         # 정적 파일
├── components/     # 컴포넌트
├── App.vue         # 루트 컴포넌트
└── main.js         # 앱 진입점
```

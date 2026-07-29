# 09. 템플릿 엔진 (EJS)

EJS(Embedded JavaScript)는 HTML 안에 JavaScript를 삽입할 수 있는 템플릿 엔진입니다.

## 설치

```bash
npm install ejs
```

## Express에 EJS 설정

```javascript
app.set('view engine', 'ejs');       // 템플릿 엔진 지정
app.set('views', './views');         // 템플릿 파일 폴더 (기본값)
```

## EJS 문법

```ejs
<%= 변수 %>        <!-- HTML 이스케이프해서 출력 -->
<%- 변수 %>        <!-- HTML 이스케이프 없이 출력 (주의) -->
<% 자바스크립트 %>  <!-- 제어문 실행 -->
<%%                <!-- 리터럴 <% 출력 -->
<%# 주석 %>        <!-- 주석 -->
```

### 예시
```ejs
<h1><%= title %></h1>
<ul>
  <% items.forEach(item => { %>
    <li><%= item %></li>
  <% }) %>
</ul>
```

## 데이터 전달

```javascript
app.get('/', (req, res) => {
  res.render('index', {
    title: '홈페이지',
    users: [{ name: '홍길동' }, { name: '김철수' }]
  });
});
```

## Partials (부분 템플릿)

중복되는 코드를 분리해서 재사용합니다.

```ejs
<!-- views/partials/header.ejs -->
<header>
  <h1><%= siteName %></h1>
  <nav>...</nav>
</header>

<!-- views/index.ejs -->
<%- include('partials/header') %>
<p>본문 내용</p>
<%- include('partials/footer') %>
```

## Layouts (레이아웃)

EJS는 기본적으로 레이아웃을 지원하지 않지만, `express-ejs-layouts` 패키지나 include로 구현할 수 있습니다.

```ejs
<!-- views/layout.ejs -->
<!DOCTYPE html>
<html>
<head><title><%= title %></title></head>
<body>
  <%- include('partials/header') %>
  <%- body %>
  <%- include('partials/footer') %>
</body>
</html>
```

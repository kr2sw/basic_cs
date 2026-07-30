# 05: Class & Style Binding — 클래스와 스타일 바인딩

## 클래스 바인딩

### 객체 문법
```html
<div :class="{ active: isActive, 'text-danger': hasError }">
```

### 배열 문법
```html
<div :class="[activeClass, errorClass]">
```

### 삼항 연산자
```html
<div :class="[isActive ? activeClass : '', errorClass]">
```

## 인라인 스타일 바인딩

### 객체 문법
```html
<div :style="{ color: activeColor, fontSize: fontSize + 'px' }">
```

### 배열 문법 (여러 객체)
```html
<div :style="[baseStyles, overrides]">
```

### 접두사 자동 완성
Vue가 자동으로 vendor prefix를 추가합니다.

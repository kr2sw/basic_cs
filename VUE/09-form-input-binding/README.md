# 09: Form Input Binding — 폼 입력 바인딩

## v-model

양방향 데이터 바인딩을 제공합니다.

```html
<input v-model="message">
<textarea v-model="text"></textarea>
<input type="checkbox" v-model="checked">
<input type="radio" v-model="picked" value="A">
<select v-model="selected">
  <option value="A">A</option>
</select>
```

## v-model 수식어

| 수식어 | 설명 |
|--------|------|
| `.lazy` | change 이벤트 후 동기화 (input 대신) |
| `.number` | 자동 숫자 변환 |
| `.trim` | 앞뒤 공백 제거 |

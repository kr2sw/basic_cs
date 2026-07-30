# 04: Computed & Watch — 계산된 속성과 감시자

## Computed (계산된 속성)

템플릿 내에서 복잡한 표현식을 대체합니다. **캐싱**되므로 의존 값이 변경될 때만 재계산됩니다.

```js
computed: {
  reversedMessage() {
    return this.message.split('').reverse().join('')
  }
}
```

### getter/setter
```js
computed: {
  fullName: {
    get() { return `${this.first} ${this.last}` },
    set(value) { [this.first, this.last] = value.split(' ') }
  }
}
```

## Watch (감시자)

데이터 변경을 감지하여 비동기/무거운 작업을 수행합니다.

```js
watch: {
  question(newVal, oldVal) {
    if (newVal.includes('?')) this.getAnswer()
  }
}
```

## computed vs watch

| computed | watch |
|----------|-------|
| 선언적 계산 | 명령형 사이드 이펙트 |
| 캐싱됨 | 캐싱 안 됨 |
| 동기 | 비동기 가능 |
| 여러 값 조합 | 단일 값 감시 |

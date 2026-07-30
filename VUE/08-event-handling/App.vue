<template>
  <div class="app">
    <h1>이벤트 핸들링</h1>

    <h2>기본 이벤트</h2>
    <button @click="count++">+1 (인라인)</button>
    <button @click="increment">+1 (메서드)</button>
    <button @click="incrementBy(5)">+5 (파라미터)</button>
    <p>count: {{ count }}</p>

    <h2>이벤트 객체</h2>
    <button @click="handleEvent($event, 'data')">이벤트 정보</button>

    <h2>이벤트 수식어</h2>
    <div class="box" @click="outerClick">
      <div class="inner" @click.stop="innerClick">.stop: 전파 중단</div>
    </div>
    <a href="https://vuejs.org" @click.prevent="linkClick">.prevent: 링크 차단</a>
    <button @click.once="onceClick">.once: 한 번만 실행 ({{ onceCount }})</button>

    <h2>키 수식어</h2>
    <p>Enter 키: <input @keyup.enter="onEnter" placeholder="Enter 입력"></p>
    <p>Esc 키: <input @keyup.esc="onEsc" placeholder="Esc 입력"></p>
    <p>Ctrl+Enter: <input @keyup.ctrl.enter="onCtrlEnter" placeholder="Ctrl+Enter"></p>

    <h2>마우스 수식어</h2>
    <button @click.left="left">좌클릭</button>
    <button @click.middle="middle">휠클릭</button>
    <button @click.right.prevent="right">우클릭</button>

    <h2>마우스 이벤트</h2>
    <div
      class="track"
      @mousemove="onMouseMove"
      @mouseenter="onMouseEnter"
      @mouseleave="onMouseLeave"
    >
      마우스를 움직여보세요
      <p>X: {{ mouseX }}, Y: {{ mouseY }}</p>
    </div>

    <h2>다중 이벤트</h2>
    <button @click="a($event); b($event)">두 개 메서드</button>

    <h2>이벤트 버블링</h2>
    <div class="bubble" @click="bubble('outer')">
      outer
      <div class="bubble inner" @click="bubble('inner')">
        inner
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      count: 0,
      onceCount: 0,
      mouseX: 0,
      mouseY: 0,
      log: ''
    }
  },
  methods: {
    increment() { this.count++ },
    incrementBy(n) { this.count += n },
    handleEvent(event, msg) {
      alert(`type: ${event.type}, msg: ${msg}, target: ${event.target.tagName}`)
    },
    outerClick() { console.log('outer') },
    innerClick() { console.log('inner (stop)') },
    linkClick() { alert('링크 차단됨') },
    onceClick() { this.onceCount++ },
    onEnter(e) { alert(`Enter: ${e.target.value}`) },
    onEsc(e) { alert('Esc 눌림') },
    onCtrlEnter(e) { alert(`Ctrl+Enter: ${e.target.value}`) },
    left() { alert('좌클릭') },
    middle() { alert('휠클릭') },
    right() { alert('우클릭') },
    onMouseMove(e) {
      this.mouseX = e.offsetX
      this.mouseY = e.offsetY
    },
    onMouseEnter() { console.log('mouse enter') },
    onMouseLeave() { console.log('mouse leave') },
    a() { console.log('A') },
    b() { console.log('B') },
    bubble(name) { console.log(`bubble: ${name}`) }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
button { margin: 4px; padding: 6px 16px; cursor: pointer; }
input { padding: 4px 8px; }
.box { padding: 12px; border: 1px solid #ccc; border-radius: 4px; }
.inner { padding: 12px; margin: 8px 0; background: #f0f0f0; border-radius: 4px; }
.track { padding: 20px; background: #f9f9f9; border: 1px solid #ddd; border-radius: 4px; cursor: crosshair; min-height: 80px; }
.bubble { padding: 12px; margin: 4px 0; border: 1px solid #999; border-radius: 4px; cursor: pointer; }
.bubble.inner { background: #e9ecef; margin-left: 20px; }
</style>

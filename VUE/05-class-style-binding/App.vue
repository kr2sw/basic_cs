<template>
  <div class="app">
    <h1>Class & Style Binding</h1>

    <h2>Class 객체 문법</h2>
    <div
      class="box"
      :class="{ active: isActive, highlight: isHighlighted }"
      @click="isActive = !isActive"
    >
      클릭하여 active 토글 ({{ isActive }})
    </div>
    <label><input type="checkbox" v-model="isHighlighted"> highlight</label>

    <h2>Class 배열 문법</h2>
    <div :class="[baseClass, isActive ? 'active' : '', 'extra']">
      배열 문법 예제
    </div>

    <h2>Computed 클래스</h2>
    <div :class="computedClass" @click="toggleStatus">
      status: {{ status }}
    </div>

    <h2>인라인 스타일 (객체)</h2>
    <div
      class="box"
      :style="{
        backgroundColor: bgColor,
        color: textColor,
        fontSize: fontSize + 'px',
        fontWeight: isBold ? 'bold' : 'normal'
      }"
    >
      스타일 바인딩
    </div>
    <label><input type="checkbox" v-model="isBold"> bold</label>

    <h2>스타일 배열 문법</h2>
    <div :style="[baseStyle, overrides]">배열 스타일</div>

    <h2>실시간 스타일 변경</h2>
    <div class="box" :style="{ backgroundColor: `hsl(${hue}, 70%, 80%)` }">
      hue: {{ hue }}
    </div>
    <input type="range" v-model.number="hue" min="0" max="360">
  </div>
</template>

<script>
export default {
  data() {
    return {
      isActive: false,
      isHighlighted: false,
      baseClass: 'box',
      status: 'pending',
      bgColor: '#42b883',
      textColor: 'white',
      fontSize: 18,
      isBold: false,
      baseStyle: { padding: '12px', borderRadius: '4px' },
      overrides: { border: '2px solid #35495e' },
      hue: 150
    }
  },
  computed: {
    computedClass() {
      return {
        box: true,
        active: this.status === 'active',
        success: this.status === 'done',
        warning: this.status === 'pending'
      }
    }
  },
  methods: {
    toggleStatus() {
      const states = ['pending', 'active', 'done']
      const idx = states.indexOf(this.status)
      this.status = states[(idx + 1) % states.length]
    }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.box { padding: 16px; margin: 8px 0; border: 1px solid #ddd; border-radius: 4px; cursor: pointer; transition: all 0.3s; }
.box.active { background: #42b883; color: white; border-color: #42b883; }
.box.highlight { box-shadow: 0 0 8px rgba(66, 184, 131, 0.5); }
.box.extra { font-style: italic; }
.box.success { background: #28a745; color: white; }
.box.warning { background: #ffc107; color: #333; }
input[type="range"] { width: 100%; }
</style>

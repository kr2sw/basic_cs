<template>
  <div class="app">
    <h1>조건부 렌더링</h1>

    <h2>v-if / v-else-if / v-else</h2>
    <div>
      <button @click="score = Math.floor(Math.random() * 100)">점수 랜덤</button>
      <span> 점수: {{ score }}</span>
    </div>
    <div class="result" v-if="score >= 90">A - 우수</div>
    <div class="result" v-else-if="score >= 80">B - 좋음</div>
    <div class="result" v-else-if="score >= 70">C - 보통</div>
    <div class="result" v-else-if="score >= 60">D - 미흡</div>
    <div class="result fail" v-else>F - 불합격</div>

    <h2>v-show</h2>
    <label><input type="checkbox" v-model="isVisible"> 표시</label>
    <div class="box" v-show="isVisible">v-show: {{ isVisible }}</div>

    <h2>v-if vs v-show</h2>
    <div style="display: flex; gap: 20px;">
      <div>
        <p><strong>v-if</strong> (DOM 제거됨)</p>
        <button @click="toggleIf">토글</button>
        <div v-if="showIf" class="box green">v-if: {{ showIf }}</div>
      </div>
      <div>
        <p><strong>v-show</strong> (display: none)</p>
        <button @click="toggleShow">토글</button>
        <div v-show="showShow" class="box blue">v-show: {{ showShow }}</div>
      </div>
    </div>

    <h2>template v-if</h2>
    <template v-if="showGroup">
      <p>이 텍스트는</p>
      <p>template 태그로</p>
      <p>그룹화되어 있습니다.</p>
    </template>
    <button @click="showGroup = !showGroup">{{ showGroup ? '숨기기' : '보이기' }}</button>

    <h2>v-if with v-for</h2>
    <div v-for="item in items" :key="item.id">
      <span v-if="item.stock > 0">✅ {{ item.name }} (재고: {{ item.stock }})</span>
      <span v-else class="soldout">❌ {{ item.name }} (품절)</span>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      score: 85,
      isVisible: true,
      showIf: true,
      showShow: true,
      showGroup: true,
      items: [
        { id: 1, name: '노트북', stock: 5 },
        { id: 2, name: '마우스', stock: 0 },
        { id: 3, name: '키보드', stock: 3 },
        { id: 4, name: '모니터', stock: 0 }
      ]
    }
  },
  methods: {
    toggleIf() { this.showIf = !this.showIf },
    toggleShow() { this.showShow = !this.showShow }
  }
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 600px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.result { padding: 8px 16px; margin: 8px 0; background: #28a745; color: white; border-radius: 4px; }
.result.fail { background: #dc3545; }
.box { padding: 12px; margin: 8px 0; border-radius: 4px; transition: all 0.3s; }
.box.green { background: #42b883; color: white; }
.box.blue { background: #007bff; color: white; }
.soldout { color: #999; text-decoration: line-through; }
button { margin: 4px; padding: 6px 16px; cursor: pointer; }
</style>

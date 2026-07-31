<template>
  <div class="app">
    <h1>보안 (XSS, v-html, CSP)</h1>

    <h2>보간법은 기본적으로 안전</h2>
    <p>입력 내용 ({{ type === 'danger' ? '악성' : '안전' }}):</p>
    <div class="box">
      {{ attackerInput }}
    </div>
    <p class="hint">`{{ }}` 보간은 HTML을 이스케이프하므로 스크립트가 실행되지 않습니다.</p>

    <h2>v-html 위험 데모</h2>
    <input v-model="attackerInput" placeholder="&lt;img src=x onerror=alert(1)&gt; 입력" class="wide">
    <button @click="renderRaw">raw 그대로 렌더링</button>
    <button @click="renderSanitized">이스케이프 후 렌더링</button>

    <div class="box" v-html="rendered"></div>
    <p v-if="rendered" class="hint">
      {{ renderMode === 'sanitized' ? '✔ 이스케이프 처리되어 안전합니다.' : '⚠ raw 그대로면 img/script가 실행될 수 있습니다.' }}
    </p>

    <h2>XSS 공격 페이로드 예시</h2>
    <pre class="payload">아래 페이로드를 입력해 보세요:
&lt;img src=x onerror="alert('XSS')"&gt;
&lt;script&gt;alert('XSS')&lt;/script&gt;</pre>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const attackerInput = ref('<img src=x onerror="alert(\'XSS\')">')
const rendered = ref('')
const renderMode = ref('')

// 안전한 렌더링: 사용자 입력을 이스케이프
function escapeHTML(str) {
  return str
    .replaceAll('&', '&amp;')
    .replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;')
    .replaceAll('"', '&quot;')
    .replaceAll("'", '&#39;')
}

function renderRaw() {
  // ❌ 위험: 신뢰할 수 없는 입력을 그대로 v-html에 넣음
  rendered.value = attackerInput.value
  renderMode.value = 'raw'
}

function renderSanitized() {
  // ✔ 안전: 이스케이프하면 화면에 텍스트로만 표시
  rendered.value = escapeHTML(attackerInput.value)
  renderMode.value = 'sanitized'
}
</script>

<style scoped>
.app { font-family: Arial; max-width: 640px; margin: 40px auto; padding: 20px; }
h1 { color: #42b883; }
h2 { color: #333; margin-top: 20px; border-bottom: 1px solid #eee; }
.hint { color: #999; font-size: 12px; }
.box { margin: 10px 0; padding: 12px; background: #f5f5f5; border-radius: 6px; min-height: 32px; }
.wide { width: 100%; padding: 8px; box-sizing: border-box; margin-bottom: 8px; }
button { margin: 4px; padding: 6px 14px; cursor: pointer; }
.payload { background: #fff3cd; padding: 12px; border-radius: 6px; font-size: 12px; }
</style>

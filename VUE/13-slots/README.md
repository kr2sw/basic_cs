# 13: Slots — 슬롯

## 기본 Slot

부모 컴포넌트에서 자식 컴포넌트로 HTML을 전달합니다.

```html
<!-- 자식: MyComponent.vue -->
<div class="card">
  <slot></slot>
</div>

<!-- 부모 -->
<MyComponent>
  <p>이 내용이 slot 위치에 렌더링됩니다.</p>
</MyComponent>
```

## Named Slot (이름 있는 슬롯)

```html
<!-- 자식 -->
<div class="layout">
  <header><slot name="header"></slot></header>
  <main><slot></slot></main>
  <footer><slot name="footer"></slot></footer>
</div>

<!-- 부모 (Vue 3) -->
<Layout>
  <template #header>Header</template>
  <p>Main content</p>
  <template #footer>Footer</template>
</Layout>
```

## Scoped Slot (범위 슬롯)

자식의 데이터를 부모 템플릿에서 사용할 수 있게 합니다.

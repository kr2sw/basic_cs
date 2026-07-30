<template>
  <div class="field">
    <label v-if="label">{{ label }}</label>
    <input :type="type" :placeholder="placeholder" :required="required">
  </div>
</template>

<script>
export default {
  props: {
    label: { type: String, default: '' },
    type: {
      type: String,
      default: 'text',
      validator(val) {
        return ['text', 'email', 'password', 'number', 'tel'].includes(val)
      }
    },
    placeholder: { type: String, default: '' },
    required: { type: Boolean, default: false }
  }
}
</script>

<style scoped>
.field { margin: 8px 0; }
label { display: block; margin-bottom: 4px; font-weight: bold; font-size: 14px; }
input { padding: 6px 12px; border: 1px solid #ddd; border-radius: 4px; width: 200px; }
</style>

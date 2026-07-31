import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import Counter from '../components/Counter.vue'

describe('Counter.vue', () => {
  it('초기 카운트는 0이다', () => {
    const wrapper = mount(Counter)
    expect(wrapper.text()).toContain('카운트: 0')
  })

  it('props로 초기값을 지정할 수 있다', () => {
    const wrapper = mount(Counter, { props: { initial: 5 } })
    expect(wrapper.text()).toContain('카운트: 5')
  })

  it('+1 버튼을 클릭하면 카운트가 증가한다', async () => {
    const wrapper = mount(Counter)
    await wrapper.findAll('button')[0].trigger('click')
    expect(wrapper.text()).toContain('카운트: 1')
  })

  it('-1 버튼을 클릭하면 카운트가 감소한다', async () => {
    const wrapper = mount(Counter)
    await wrapper.findAll('button')[1].trigger('click')
    expect(wrapper.text()).toContain('카운트: -1')
  })

  it('카운트가 음수가 되지 않도록 제한한다', async () => {
    const wrapper = mount(Counter, { props: { initial: 0 } })
    const minusButton = wrapper.findAll('button')[1]
    await minusButton.trigger('click')
    await minusButton.trigger('click')
    expect(wrapper.text()).toContain('카운트: 0')
  })
})

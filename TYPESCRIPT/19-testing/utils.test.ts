import { add, greet, parseJSON } from './utils'

test('adds two numbers', () => {
  expect(add(2, 3)).toBe(5)
})

test('greets with default prefix', () => {
  expect(greet('Alice')).toBe('Hello, Alice!')
})

test('greets with custom prefix', () => {
  expect(greet('Bob', 'Hi')).toBe('Hi, Bob!')
})

test('parses valid JSON', () => {
  const result = parseJSON<{ name: string }>('{"name":"Alice"}')
  expect(result).toEqual({ name: 'Alice' })
})

test('returns null for invalid JSON', () => {
  expect(parseJSON('invalid')).toBeNull()
})

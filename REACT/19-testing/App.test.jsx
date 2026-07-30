import { render, screen, fireEvent } from '@testing-library/react'
import { Counter, Greeting } from './App'

test('renders initial count', () => {
  render(<Counter initial={5} />)
  expect(screen.getByTestId('count')).toHaveTextContent('Count: 5')
})

test('increments count', () => {
  render(<Counter />)
  fireEvent.click(screen.getByText('+1'))
  expect(screen.getByTestId('count')).toHaveTextContent('Count: 1')
})

test('decrements count', () => {
  render(<Counter initial={5} />)
  fireEvent.click(screen.getByText('-1'))
  expect(screen.getByTestId('count')).toHaveTextContent('Count: 4')
})

test('resets to initial', () => {
  render(<Counter initial={10} />)
  fireEvent.click(screen.getByText('+1'))
  fireEvent.click(screen.getByText('Reset'))
  expect(screen.getByTestId('count')).toHaveTextContent('Count: 10')
})

test('greeting renders name', () => {
  render(<Greeting name="Alice" />)
  expect(screen.getByText('Hello, Alice!')).toBeInTheDocument()
})

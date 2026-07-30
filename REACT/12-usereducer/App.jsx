import { useReducer } from 'react'

const initialState = { count: 0, step: 1 }

function reducer(state, action) {
  switch (action.type) {
    case 'increment':
      return { ...state, count: state.count + state.step }
    case 'decrement':
      return { ...state, count: state.count - state.step }
    case 'setStep':
      return { ...state, step: action.payload }
    case 'reset':
      return initialState
    default:
      return state
  }
}

function App() {
  const [state, dispatch] = useReducer(reducer, initialState)

  return (
    <div>
      <h1>useReducer</h1>
      <p>Count: {state.count}</p>
      <p>Step: {state.step}</p>
      <div>
        <button onClick={() => dispatch({ type: 'decrement' })}>-</button>
        <button onClick={() => dispatch({ type: 'increment' })}>+</button>
      </div>
      <div>
        <label>Step:
          <input type="number" value={state.step}
            onChange={e => dispatch({ type: 'setStep', payload: Number(e.target.value) })} />
        </label>
      </div>
      <button onClick={() => dispatch({ type: 'reset' })}>Reset</button>
    </div>
  )
}

export default App

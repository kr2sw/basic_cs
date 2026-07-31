import { QueryClient, QueryClientProvider, useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import axios from 'axios'
import { useState } from 'react'

const api = axios.create({ baseURL: 'https://jsonplaceholder.typicode.com' })
const queryClient = new QueryClient()

// 읽기: useQuery
function useTodos() {
  return useQuery({
    queryKey: ['todos'],
    queryFn: async () => (await api.get('/todos')).data,
  })
}

// 쓰기: useMutation -> 성공 시 캐시 무효화로 자동 재조회
function useAddTodo() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: title => api.post('/todos', { title, completed: false }),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['todos'] }),
  })
}

function Todos() {
  const { data, isLoading, isError, error, refetch } = useTodos()
  const addTodo = useAddTodo()
  const [title, setTitle] = useState('')

  if (isLoading) return <p>로딩 중...</p>
  if (isError) return <p style={{ color: 'red' }}>에러: {error.message}</p>

  function onSubmit(e) {
    e.preventDefault()
    if (!title.trim()) return
    addTodo.mutate(title)   // post 후 onSuccess로 캐시 갱신
    setTitle('')
  }

  return (
    <div>
      <h2>할일 목록</h2>
      <button onClick={() => refetch()}>수동 재조회</button>
      <form onSubmit={onSubmit}>
        <input value={title} onChange={e => setTitle(e.target.value)} placeholder="새 할일" />
        <button type="submit" disabled={addTodo.isPending}>
          {addTodo.isPending ? '추가 중...' : '추가'}
        </button>
      </form>
      <ul>
        {data.map(t => (
          <li key={t.id}>{t.title} {t.completed ? '✓' : ''}</li>
        ))}
      </ul>
    </div>
  )
}

function App() {
  return (
    // QueryClientProvider로 앱 전체에 쿼리 클라이언트 주입
    <QueryClientProvider client={queryClient}>
      <div>
        <h1>TanStack Query 기초</h1>
        <Todos />
      </div>
    </QueryClientProvider>
  )
}

export default App

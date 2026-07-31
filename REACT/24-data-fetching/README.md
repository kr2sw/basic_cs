# 24: 데이터 패칭 — Data Fetching with TanStack Query

서버 상태(server state)를 선언적으로 관리하는 TanStack Query(React Query)의 기초를 배웁니다.

## 왜 TanStack Query인가?

`useEffect + fetch`는 로딩/에러/캐시/재시도를 일일이 구현해야 합니다. TanStack Query는 이 모든 것을 제공합니다:

- 캐싱 & 같은 키 중복 요청 방지
- 백그라운드 재조회(revalidate)
- 로딩/에러 상태 자동 관리
- 낙관적 업데이트(optimistic update)

```jsx
import { QueryClient, QueryClientProvider, useQuery } from '@tanstack/react-query'

const queryClient = new QueryClient()

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <Todos />
    </QueryClientProvider>
  )
}
```

## useQuery — 읽기

`queryKey`가 캐시의 기준입니다. 같은 키의 요청은 서로 공유됩니다.

```jsx
const { data, isLoading, isError, error, refetch } = useQuery({
  queryKey: ['todos'],
  queryFn: async () => (await axios.get('/todos')).data,
  staleTime: 5 * 60 * 1000, // 5분 동안 재조회 안 함
})
```

## useMutation — 쓰기

쓰기 작업은 `useMutation`입니다. 성공 시 `invalidateQueries`로 관련 캐시를 무효화하면 자동 재조회가 일어납니다.

```jsx
const addTodo = useMutation({
  mutationFn: title => axios.post('/todos', { title }),
  onSuccess: () => queryClient.invalidateQueries({ queryKey: ['todos'] }),
})
```

## 실행

```bash
npm install @tanstack/react-query axios && npm run dev
```

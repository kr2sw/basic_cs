# 25: HTTP 통신 — Axios 인터셉터, API 레이어 패턴

## 설치

```bash
npm install axios
```

## Axios 인스턴스 + 인터셉터

모든 요청에 공통 설정(baseURL, timeout)과 인터셉터를 적용합니다.

```js
// api/client.js
import axios from 'axios'

const client = axios.create({
  baseURL: 'https://jsonplaceholder.typicode.com',
  timeout: 10000
})

// 요청 인터셉터: 토큰 첨부, 로깅
client.interceptors.request.use((config) => {
  config.headers.Authorization = `Bearer ${localStorage.getItem('token') || ''}`
  return config
})

// 응답 인터셉터: 공통 에러 처리
client.interceptors.response.use(
  (res) => res,
  (error) => {
    if (error.response?.status === 401) {
      alert('인증이 필요합니다')
    }
    return Promise.reject(error)
  }
)

export default client
```

## API 레이어 패턴

컴포넌트에서 URL을 직접 쓰지 않고 API 모듈을 거쳐 호출합니다.
데이터 변환과 재사용 로직을 한곳에 모아 유지보수가 쉬워집니다.

```js
// api/todos.js
export const todoApi = {
  async fetchAll() {
    const { data } = await client.get('/todos?_limit=5')
    return data
  },
  async create(text) {
    const { data } = await client.post('/todos', { title: text, completed: false })
    return data
  }
}
```

## 레이어 흐름

```
컴포넌트 → api 모듈 → axios client(인터셉터) → 서버
```

## 실행

```bash
npm install && npx vite serve .
```

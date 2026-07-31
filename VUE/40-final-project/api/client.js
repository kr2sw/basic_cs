import axios from 'axios'

// 공통 axios 인스턴스
const client = axios.create({
  baseURL: 'https://jsonplaceholder.typicode.com',
  timeout: 10000
})

// 요청 인터셉터: 공통 헤더
client.interceptors.request.use((config) => {
  config.headers.Authorization = `Bearer ${localStorage.getItem('token') || ''}`
  return config
})

// 응답 인터셉터: 공통 에러 처리
client.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      console.warn('인증 만료. 다시 로그인하세요.')
    }
    return Promise.reject(error)
  }
)

export default client

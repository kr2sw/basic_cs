import axios from 'axios'

// 공통 설정이 담긴 axios 인스턴스
const client = axios.create({
  baseURL: 'https://jsonplaceholder.typicode.com',
  timeout: 10000
})

// 요청 인터셉터: 요청 전에 공통 처리를 수행
client.interceptors.request.use((config) => {
  // 실제 서비스에서는 토큰을 헤더에 첨부
  config.headers.Authorization = `Bearer ${localStorage.getItem('token') || ''}`
  console.log('[요청]', config.method?.toUpperCase(), config.url)
  return config
})

// 응답 인터셉터: 응답/에러를 가로채 공통 처리
client.interceptors.response.use(
  (response) => response,
  (error) => {
    // 401(인증 실패) 등 공통 에러 처리
    if (error.response?.status === 401) {
      console.warn('인증이 필요합니다. 로그인 페이지로 이동하세요.')
    }
    return Promise.reject(error)
  }
)

export default client

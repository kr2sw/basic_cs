// Service Worker: 오프라인 우선(cache-first) 캐싱
const CACHE = 'pwa-demo-v1'
const CORE_ASSETS = ['/', '/index.html', '/main.jsx']

// 설치: 핵심 자산을 미리 캐시에 담는다
self.addEventListener('install', event => {
  event.waitUntil(
    caches.open(CACHE).then(cache => cache.addAll(CORE_ASSETS))
  )
  self.skipWaiting()
})

// 활성화: 이전 버전 캐시 정리
self.addEventListener('activate', event => {
  event.waitUntil(
    caches.keys().then(keys =>
      Promise.all(keys.filter(key => key !== CACHE).map(key => caches.delete(key)))
    )
  )
  self.clients.claim()
})

// 요청 처리: 캐시에 있으면 캐시 우선, 없으면 네트워크 후 캐시에 저장
self.addEventListener('fetch', event => {
  if (event.request.method !== 'GET') return

  event.respondWith(
    caches.match(event.request).then(cached => {
      if (cached) return cached
      return fetch(event.request).then(response => {
        const copy = response.clone()
        caches.open(CACHE).then(cache => cache.put(event.request, copy))
        return response
      }).catch(() => new Response('오프라인 상태입니다', { status: 200 }))
    })
  )
})

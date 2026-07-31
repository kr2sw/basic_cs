# 39: PWA — Progressive Web App

Service Worker, Web App Manifest, 오프라인 캐싱으로 웹 앱을 "설치 가능한 네이티브 앱처럼" 만듭니다.

## PWA의 세 가지 기둥

1. **HTTPS** (로컬 개발에서는 localhost 허용)
2. **Service Worker**: 네트워크 요청을 가로채 캐시에서 응답 → 오프라인 동작
3. **Manifest**: 앱 이름, 아이콘, 설치 가능성 제공

## Service Worker 기본

설치 → 활성화 → 요청 처리(fetch) 라이프사이클을 가집니다.

```js
// sw.js
const CACHE = 'my-app-v1'

self.addEventListener('install', e => {
  e.waitUntil(caches.open(CACHE).then(c => c.addAll(['/', '/index.html'])))
})

self.addEventListener('activate', e => {
  e.waitUntil(caches.keys().then(keys => Promise.all(
    keys.filter(k => k !== CACHE).map(k => caches.delete(k))
  )))
})

// 오프라인 우선(cache-first) 전략
self.addEventListener('fetch', e => {
  e.respondWith(
    caches.match(e.request)
      .then(hit => hit || fetch(e.request).then(res => {
        const copy = res.clone()
        caches.open(CACHE).then(c => c.put(e.request, copy))
        return res
      }))
  )
})
```

## React에서 등록

앱 시작 시 Service Worker를 등록합니다. 처음엔 스크롤 위 네이티브 이벤트, `onload` 후에 등록하면 초기 로딩을 방해하지 않습니다.

```jsx
useEffect(() => {
  if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('/sw.js')
  }
}, [])
```

## Manifest 예시

```json
{
  "name": "할일 앱",
  "short_name": "할일",
  "start_url": "/",
  "display": "standalone",
  "background_color": "#ffffff",
  "theme_color": "#3b82f6",
  "icons": [{ "src": "/icon.svg", "sizes": "any", "type": "image/svg+xml" }]
}
```

`index.html`에서 `<link rel="manifest" href="/manifest.webmanifest">`로 연결합니다.

## 실행

```bash
npm install && npm run dev
# DevTools → Application 탭에서 Service Worker 상태 확인
```

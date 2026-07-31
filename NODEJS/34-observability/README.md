# 34: 관찰 가능성 — Logging, Request Tracing, Metrics

프로덕션에서 애플리케이션 상태를 파악하는 방법을 학습합니다.

## 구조화된 로깅

문자열보다 JSON 형식으로 로그를 남기면 검색/집계가 쉬워집니다.

```js
const entry = {
  level: 'info',
  message: '요청 처리 완료',
  durationMs: 42,
  timestamp: new Date().toISOString(),
};
console.log(JSON.stringify(entry));
```

로깅 레벨: `debug < info < warn < error`

## 요청 추적 (Request Tracing)

요청마다 고유한 `requestId`를 부여하면 분산 환경에서 한 요청의 모든 로그를 연결할 수 있습니다.

```js
req.id = crypto.randomUUID();
logger.info('요청 시작', { requestId: req.id, method, path });
logger.info('요청 종료', { requestId: req.id, status });
```

## 성능 메트릭

- **응답 시간**: 요청 시작~종료 시간 측정, 히스토그램으로 저장
- **카운터**: 요청 수, 오류 수, 5xx 수
- **활용**: `/metrics` 엔드포인트로 노출 → Prometheus 수집

## 3가지 관찰 신호 (3 Pillars)

| 신호 | 의미 |
|------|------|
| **Logs** | 상세 이벤트 기록 (무엇이 일어났는가) |
| **Metrics** | 수치 집계 (얼마나 많은가) |
| **Traces** | 요청 흐름 추적 (어디서 시간을 보내는가) |

## 예제 실행

```bash
node index.js
```

```bash
curl http://localhost:3000/
curl http://localhost:3000/slow
curl http://localhost:3000/error
curl http://localhost:3000/metrics
```

# 36: 마이크로서비스 — Service Separation, HTTP Communication, Health Check

서비스를 독립적으로 분리하고 통신하는 MSA 구조를 학습합니다.

## 모놀리식 vs 마이크로서비스

| 구분 | 모놀리식 | 마이크로서비스 |
|------|----------|----------------|
| 구조 | 하나의 앱에 모든 기능 | 기능별 독립 서비스 |
| 배포 | 전체 재배포 | 서비스별 개별 배포 |
| 확장 | 전체 확장 | 필요한 서비스만 확장 |
| 장애 | 전체 영향 | 해당 서비스만 영향 |

## 서비스 분리

예시: `user-service`, `order-service`, `payment-service`처럼 도메인 단위로 나눕니다.

## 서비스 간 통신

다른 서비스의 API를 HTTP로 호출합니다.

```js
// order-service가 user-service의 사용자 정보를 조회
const res = await fetch('http://user-service:3001/users/1');
const user = await res.json();
```

실제 배포에서는 내부 DNS/서비스 디스커버리(레지스트리)로 주소를 찾습니다.

## 서비스 레지스트리

각 서비스가 자신의 주소를 등록하고 다른 서비스가 조회합니다.

```js
registry.register('user-service', 'http://localhost:3001');
const url = registry.lookup('user-service');
```

## 헬스 체크

각 서비스는 자신의 상태를 알리는 `/health` 엔드포인트를 제공합니다. 로드 밸런서/오케스트레이터가 주기적으로 확인하여 장애 서비스를 제외합니다.

```js
app.get('/health', (req, res) => {
  res.json({ status: 'ok', uptime: process.uptime() });
});
```

## 예제 실행

```bash
node index.js
```

user-service(3001)와 order-service(3002)가 서로 HTTP로 통신하는 과정을 보여줍니다.

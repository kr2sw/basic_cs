# 25: Redis — Caching and Pub/Sub Concepts

Redis 인메모리 데이터 저장소의 캐시와 pub/sub 개념을 학습합니다.

## Redis란?

Redis는 모든 데이터를 메모리에 저장하는 key-value 저장소입니다. 초당 수십만 건의 읽기/쓰기를 처리하며 캐시, 세션, 메시지 브로커로 사용됩니다.

## 주요 명령어

```bash
SET user:1 "홍길동" EX 60   # 60초 후 만료되는 키 저장
GET user:1                  # 값 조회
DEL user:1                  # 삭제
TTL user:1                  # 남은 만료 시간 확인
EXPIRE user:1 120           # 만료 시간 재설정
```

## 캐시 사용 패턴 (Cache-Aside)

조회 요청 시 캐시를 먼저 확인하고 없으면 DB에서 읽어 캐시에 저장합니다.

```js
async function getUser(id) {
  const cached = await redis.get(`user:${id}`);
  if (cached) return JSON.parse(cached);   // 캐시 히트

  const user = await db.find(id);          // 캐시 미스 -> DB 조회
  await redis.set(`user:${id}`, JSON.stringify(user), 'EX', 3600);
  return user;
}
```

## Pub/Sub

채널에 메시지를 발행(PUBLISH)하면 구독(SUBSCRIBE)한 모든 클라이언트가 받습니다.

```bash
SUBSCRIBE order:created
PUBLISH order:created '{"id":1}'
```

## 예제 실행

예제는 Redis 서버 없이 in-memory 시뮬레이션으로 동작합니다.

```bash
node index.js
```

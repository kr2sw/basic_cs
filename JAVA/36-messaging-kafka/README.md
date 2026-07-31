# 36: Messaging & Kafka — 메시징과 카프카

## 메시징 개념

서비스 간 결합을 낮추는 비동기 통신 방식입니다.

```
생산자(Producer) -> [브로커/큐] -> 소비자(Consumer)
```

- **프로듀서**: 메시지를 보내는 쪽
- **컨슈머**: 메시지를 받아 처리하는 쪽
- **토픽(Topic)**: 메시지를 분류하는 채널
- **파티션(Partition)**: 토픽을 나눠 병렬 처리하는 단위
- **오프셋(Offset)**: 소비 위치 기록

## Kafka 구조

```
Producer -> Topic[partition-0, partition-1, partition-2] -> Consumer Group
```

- 토픽은 여러 파티션으로 나뉘고, 컨슈머 그룹이 분산 소비
- 파티션 내부 메시지는 순서 보장
- 메시지는 로그 형태로 보관 (재소비 가능)

## 프로듀서/컨슈머 API 개념

```java
// Kafka 예 (실제 라이브러리)
producer.send(new ProducerRecord<>("orders", key, value));
consumer.poll(Duration.ofMillis(100));   // 메시지 가져오기
consumer.commitSync();                   // 오프셋 커밋
```

## JDK 로 구현하기

`BlockingQueue` 를 파티션으로 사용해 동일한 패턴을 구현할 수 있습니다.

```java
BlockingQueue<String> partition = new ArrayBlockingQueue<>(100);
producer.offer(message);
consumer.take();   // 메시지 대기
```

## 실행

```bash
cd JAVA/36-messaging-kafka
javac Main.java && java Main
```

> 카프카 없이 JDK의 BlockingQueue 로 프로듀서/컨슈머 패턴을 구현합니다.
> 실제 카프카 사용 코드는 주석으로 함께 안내합니다.

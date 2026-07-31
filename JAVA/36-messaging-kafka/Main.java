import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.atomic.*;

public class Main {

    // --- Kafka Topic/Partition 을 BlockingQueue 로 구현 ---
    static class Topic {
        private final String name;
        private final List<BlockingQueue<String>> partitions;   // 파티션별 큐

        Topic(String name, int partitionCount) {
            this.name = name;
            this.partitions = new ArrayList<>();
            for (int i = 0; i < partitionCount; i++) {
                partitions.add(new ArrayBlockingQueue<>(100));
            }
        }

        String name() { return name; }
        int partitionCount() { return partitions.size(); }

        // 키 기반으로 파티션 선택 (Kafka 는 hash(key) % partitionCount)
        BlockingQueue<String> partitionFor(String key) {
            int idx = (key.hashCode() & Integer.MAX_VALUE) % partitions.size();
            return partitions.get(idx);
        }
    }

    // --- 프로듀서 ---
    static class Producer implements Runnable {
        private final Topic topic;
        private final List<String> messages;

        Producer(Topic topic, List<String> messages) {
            this.topic = topic;
            this.messages = messages;
        }

        @Override public void run() {
            try {
                for (String msg : messages) {
                    String[] parts = msg.split(":", 2);
                    String key = parts[0];
                    String value = parts[1];
                    BlockingQueue<String> partition = topic.partitionFor(key);
                    partition.offer(key + ":" + value, 5, TimeUnit.SECONDS);
                    System.out.println("  [프로듀서] " + topic.name() + "/" +
                        partitionSuffix(topic, partition) + " 에 발행: " + msg +
                        " (key=" + key + ")");
                    Thread.sleep(30);
                }
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        }

        private static String partitionSuffix(Topic topic, BlockingQueue<String> partition) {
            return "partition-" + topic.partitions.indexOf(partition);
        }
    }

    // --- 컨슈머 (컨슈머 그룹의 멤버) ---
    static class Consumer implements Runnable {
        private final Topic topic;
        private final String groupId;
        private final String consumerId;
        private final int groupSize;
        private final Map<String, Long> consumedOffsets = new ConcurrentHashMap<>();
        private final AtomicBoolean running = new AtomicBoolean(true);

        Consumer(Topic topic, String groupId, String consumerId, int groupSize) {
            this.topic = topic;
            this.groupId = groupId;
            this.consumerId = consumerId;
            this.groupSize = groupSize;
        }

        void stop() { running.set(false); }

        // 컨슈머 그룹이 파티션을 나눠 담당 (Kafka: range/round-robin 할당)
        private List<BlockingQueue<String>> ownedPartitions() {
            List<BlockingQueue<String>> owned = new ArrayList<>();
            int myIndex = Integer.parseInt(consumerId.split("-")[1]) - 1;
            for (int i = 0; i < topic.partitions.size(); i++) {
                if (i % groupSize == myIndex) owned.add(topic.partitions.get(i));
            }
            return owned;
        }

        // 담당 파티션을 폴링하며 메시지 소비
        void pollAndProcess() throws InterruptedException {
            for (BlockingQueue<String> partition : ownedPartitions()) {
                String msg = partition.poll(20, TimeUnit.MILLISECONDS);
                if (msg != null) {
                    String[] parts = msg.split(":", 2);
                    consumedOffsets.merge(parts[0], 1L, Long::sum);
                    System.out.println("  [컨슈머 " + consumerId + " / group=" + groupId + "] " +
                        "partition-" + topic.partitions.indexOf(partition) + " 소비: " + msg + "  <- 오프셋 기록");
                }
            }
        }

        @Override public void run() {
            try {
                while (running.get()) {
                    pollAndProcess();
                }
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        }

        void printStats() {
            System.out.println("    " + consumerId + " 소비 통계: " + consumedOffsets);
        }
    }

    public static void main(String[] args) throws Exception {
        System.out.println("=== 토픽 생성 ===");

        Topic orders = new Topic("orders", 3);   // 3개 파티션
        System.out.println("  토픽 'orders' 생성: " + orders.partitionCount() + "개 파티션");

        System.out.println("\n=== 프로듀서 발행 ===");

        // key:value 형태로 발행 (key 는 파티션 결정에 사용)
        List<String> messages = List.of(
            "user-1:커피 주문",
            "user-2:도서 주문",
            "user-3:키보드 주문",
            "user-1:음료 주문",
            "user-4:모니터 주문",
            "user-2:노트 주문"
        );
        Producer producer = new Producer(orders, messages);
        Thread producerThread = new Thread(producer, "producer");
        producerThread.start();

        System.out.println("\n=== 컨슈머 그룹 소비 ===");

        Consumer c1 = new Consumer(orders, "order-process-group", "consumer-1", 2);
        Consumer c2 = new Consumer(orders, "order-process-group", "consumer-2", 2);
        Thread t1 = new Thread(c1, "consumer-1");
        Thread t2 = new Thread(c2, "consumer-2");
        t1.start();
        t2.start();

        producerThread.join();
        Thread.sleep(1500);   // 컨슈머가 남은 메시지 소비할 시간
        c1.stop();
        c2.stop();
        t1.join();
        t2.join();

        System.out.println("\n=== 컨슈머 그룹 통계 ===");

        c1.printStats();
        c2.printStats();

        System.out.println("\n=== 메시지 처리 개념 정리 ===");

        System.out.println("  - 같은 key(user-1) 메시지는 같은 파티션에 순서대로 도착");
        System.out.println("  - 컨슈머 그룹이 파티션을 나눠 병렬 처리");
        System.out.println("  - 처리 완료 시 오프셋을 커밋해 재처리 방지");

        System.out.println("\n=== 실제 Kafka 코드 형태 (주석) ===");

        /*
        // 실제 Apache Kafka (강의자료용 참고)
        // pom.xml: kafka-clients

        Properties props = new Properties();
        props.put("bootstrap.servers", "localhost:9092");
        props.put("key.serializer", "org.apache.kafka.common.serialization.StringSerializer");
        props.put("value.serializer", "org.apache.kafka.common.serialization.StringSerializer");

        KafkaProducer<String, String> producer = new KafkaProducer<>(props);
        producer.send(new ProducerRecord<>("orders", "user-1", "커피 주문"));

        // Consumer
        props.put("group.id", "order-process-group");
        KafkaConsumer<String, String> consumer = new KafkaConsumer<>(props);
        consumer.subscribe(List.of("orders"));
        while (true) {
            ConsumerRecords<String, String> records = consumer.poll(Duration.ofMillis(100));
            for (ConsumerRecord<String, String> r : records) {
                System.out.println("key=" + r.key() + ", value=" + r.value() + ", offset=" + r.offset());
            }
            consumer.commitSync();   // 오프셋 커밋
        }
        */
    }
}

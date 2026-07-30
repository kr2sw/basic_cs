# 19: Date & Time — 날짜와 시간 (Java 8+)

## java.time 패키지 (Java 8+)

Java 8에서 도입된 새로운 날짜/시간 API입니다.

| 클래스 | 설명 |
|--------|------|
| `LocalDate` | 날짜 (년, 월, 일) |
| `LocalTime` | 시간 (시, 분, 초, 나노초) |
| `LocalDateTime` | 날짜 + 시간 |
| `ZonedDateTime` | 시간대 포함 |
| `Instant` | 타임스탬프 (UTC) |
| `Duration` | 시간 간격 (초/나노초) |
| `Period` | 날짜 간격 (년/월/일) |
| `DateTimeFormatter` | 포맷팅/파싱 |

## 주요 특징

- 불변(immutable) 객체
- 스레드 안전
- null 안전 (null 대신 `Optional` 사용)
- fluent API (메서드 체이닝)

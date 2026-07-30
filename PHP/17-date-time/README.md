# 17: Date & Time — 날짜와 시간

## 날짜/시간 함수

| 함수 | 설명 |
|------|------|
| `date()` | 날짜/시간 포맷팅 |
| `time()` | 현재 Unix 타임스탬프 |
| `strtotime()` | 문자열 → 타임스탬프 |
| `mktime()` | 특정 날짜 → 타임스탬프 |

## DateTime 클래스 (PHP 5.2+)

객체 지향 방식의 날짜/시간 처리입니다.

| 메서드 | 설명 |
|--------|------|
| `new DateTime()` | 현재 시간 |
| `format()` | 포맷팅 |
| `modify()` | 시간 변경 |
| `diff()` | 차이 계산 |
| `add()` / `sub()` | DateInterval 추가/감소 |
| `setTimezone()` | 시간대 변경 |

## DateInterval

`P1Y2M3DT4H5M6S` 형식: 1년 2월 3일 4시간 5분 6초

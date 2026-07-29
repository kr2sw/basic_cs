# 17: 날짜와 시간 (datetime)

## datetime 모듈
- `datetime.datetime`: 날짜 + 시간
- `datetime.date`: 날짜만 (연, 월, 일)
- `datetime.time`: 시간만 (시, 분, 초, 마이크로초)
- `datetime.timedelta`: 시간 간격

```python
from datetime import datetime, date, timedelta
now = datetime.now()
today = date.today()
```

## timedelta
두 datetime 객체의 차이를 나타냅니다. 덧셈/뺄셈으로 날짜 계산이 가능합니다.

## strftime / strptime
- `strftime(format)`: datetime → 문자열 포맷팅
- `strptime(string, format)`: 문자열 → datetime 파싱

## timezone
`pytz`나 `zoneinfo`(Python 3.9+)로 시간대를 처리할 수 있습니다. UTC와 현지 시간 변환에 사용합니다.

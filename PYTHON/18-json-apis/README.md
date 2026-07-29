# 18: JSON과 API 통신

## json 모듈 (표준 라이브러리)
- `json.dumps(obj)` / `json.loads(str)`: 파이썬 객체 ↔ JSON 문자열
- `json.dump(obj, file)` / `json.load(file)`: 파이썬 객체 ↔ JSON 파일

```python
import json
data = {"name": "Alice", "age": 30}
text = json.dumps(data, ensure_ascii=False, indent=2)
```

## urllib.request (표준 라이브러리)
외부 라이브러리 없이 HTTP 요청을 보낼 수 있습니다. `requests` 라이브러리가 더 편리하지만, 이 예제에서는 표준 라이브러리만 사용합니다.

## GET / POST 요청
- GET: 데이터를 조회할 때 사용 (URL에 쿼리 포함)
- POST: 데이터를 생성/전송할 때 사용 (body에 데이터 포함)

## HTTP 상태 코드
- 200 OK: 성공
- 201 Created: 생성 성공
- 400 Bad Request: 잘못된 요청
- 404 Not Found: 리소스 없음
- 500 Internal Server Error: 서버 오류

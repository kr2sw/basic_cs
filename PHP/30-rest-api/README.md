# 30: REST API 설계 — 엔드포인트, 상태 코드, 버저닝

## 리소스 중심 설계

URL은 **동사가 아닌 명사(리소스)**로 표현합니다. HTTP 메서드가 동작을 담당합니다.

| 메서드 | `/tasks` | `/tasks/{id}` |
|--------|----------|----------------|
| GET | 목록 조회 | 단건 조회 |
| POST | 새 리소스 생성 | - |
| PUT | 전체 교체 | 전체 교체 |
| PATCH | - | 부분 수정 |
| DELETE | - | 삭제 |

## 상태 코드

| 코드 | 의미 |
|------|------|
| 200 | OK |
| 201 | Created (생성 성공) |
| 204 | No Content (삭제 성공) |
| 400 | Bad Request (요청 형식 오류) |
| 401 | Unauthorized (인증 필요) |
| 403 | Forbidden (권한 없음) |
| 404 | Not Found |
| 405 | Method Not Allowed |
| 409 | Conflict (중복 등) |
| 422 | Unprocessable Entity (검증 실패) |
| 500 | Internal Server Error |

## 버저닝

- **URL 버저닝**: `/api/v1/tasks` — 가장 직관적
- **쿼리 버저닝**: `/api/tasks?version=1`
- **헤더 버저닝**: `Accept: application/vnd.myapp.v1+json`

URL 버저닝이 캐싱·디버깅에 유리해 가장 널리 쓰입니다.

## 응답 형식

```json
{
  "data": { "id": 1, "title": "설계" },
  "meta": { "count": 1 }
}
```

에러는 `{"error": "Task not found"}` 형태로 일관되게 유지합니다.

## 성공/실패 규칙

- 리소스 하나: `data`는 객체, 목록: `data`는 배열
- 검증 실패: 422 + 필드별 오류 메시지
- 페이지네이션: `?page=2&limit=10` → `meta.links`

## 실행

```bash
php index.php
```

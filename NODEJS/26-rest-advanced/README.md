# 26: 고급 REST API — Validation, Error Responses, Versioning

프로덕션 수준 REST API를 설계하는 방법을 학습합니다.

## API 버저닝

호환성 유지를 위해 URL 경로에 버전을 명시합니다.

```js
app.use('/api/v1/users', usersRouter);
app.use('/api/v2/users', usersV2Router); // 이후 버전 추가
```

## 입력 검증

신뢰할 수 없는 클라이언트 입력은 항상 검증합니다.

```js
const rules = {
  name: { required: true, minLength: 2 },
  email: { required: true, email: true },
  age: { type: 'number', min: 0 },
};
```

## 통일된 에러 응답

성공/실패 여부를 구분할 수 있는 일관된 응답 형태를 사용합니다.

```json
{
  "success": false,
  "error": "게시글을 찾을 수 없습니다",
  "status": 404,
  "timestamp": "2026-07-31T10:00:00.000Z"
}
```

## 적절한 상태 코드

| 코드 | 의미 |
|------|------|
| 200 | 성공 (조회, 수정) |
| 201 | 생성 완료 |
| 400 | 잘못된 입력 (검증 실패) |
| 401 | 인증 실패 |
| 403 | 권한 없음 |
| 404 | 리소스 없음 |
| 409 | 충돌 (중복 등) |
| 422 | 처리 불가한 요청 데이터 |
| 500 | 서버 내부 오류 |

## 기타 패턴

- **페이징**: `?page=1&limit=20`, 응답에 `total`, `hasMore` 포함
- **Idempotent**: DELETE/PUT은 여러 번 호출해도 같은 결과 보장
- **async 래퍼**: 오류를 에러 핸들러로 전달

## 예제 실행

```bash
node index.js
```

```bash
curl http://localhost:3000/api/v1/users
curl -X POST -H "Content-Type: application/json" \
  -d '{"name":"홍길동","email":"bad"}' http://localhost:3000/api/v1/users
```

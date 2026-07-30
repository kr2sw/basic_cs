# 10: Superglobals — 슈퍼 글로벌 변수

PHP는 스크립트 전체에서 접근 가능한 미리 정의된 전역 변수(Superglobals)를 제공합니다.

| 변수 | 설명 |
|------|------|
| `$_GET` | URL 쿼리 파라미터 (GET 요청) |
| `$_POST` | HTTP POST 데이터 |
| `$_REQUEST` | GET + POST + COOKIE 통합 |
| `$_SERVER` | 서버/환경 정보 |
| `$_SESSION` | 세션 데이터 |
| `$_COOKIE` | 쿠키 데이터 |
| `$_FILES` | 파일 업로드 데이터 |
| `$_ENV` | 환경 변수 |
| `$GLOBALS` | 모든 전역 변수 참조 |

## $_SERVER 주요 키

| 키 | 설명 |
|----|------|
| `REQUEST_METHOD` | HTTP 메서드 (GET/POST) |
| `SERVER_NAME` | 서버 호스트명 |
| `SERVER_PORT` | 서버 포트 |
| `REQUEST_URI` | 요청 URI |
| `QUERY_STRING` | 쿼리 문자열 |
| `HTTP_USER_AGENT` | 브라우저 정보 |
| `REMOTE_ADDR` | 클라이언트 IP |
| `SCRIPT_FILENAME` | 현재 스크립트 경로 |

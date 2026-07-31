# 40: 종합 프로젝트 — CLI 기반 작업 관리 앱

## 프로젝트 소개

지금까지 배운 개념을 종합한 CLI 작업 관리(Todo) 앱입니다.

| 사용 개념 | 적용 |
|-----------|------|
| 클래스 설계 | 엔티티(Task), 저장소(Repository), 앱(Application) 분리 — SRP |
| Repository 패턴 | 데이터 접근을 인터페이스 뒤에 은닉 |
| 파일 영속성 | JSON 파일에 저장·로드 |
| JSON | `json_encode` / `json_decode` |
| CLI 파싱 | `$argv`, `match` 표현식 |

## 구조

| 클래스 | 역할 |
|--------|------|
| `Task` | 작업 엔티티 (id, title, done, createdAt) |
| `TaskRepository` | JSON 파일 저장/조회/변경 |
| `TaskApp` | 명령 처리와 출력 |

## 기능

| 명령 | 설명 |
|------|------|
| `php index.php` | 목록 (기본 명령) |
| `php index.php list` | 작업 목록 |
| `php index.php add "할 일"` | 작업 추가 |
| `php index.php done 1` | 완료/진행 토글 |
| `php index.php remove 1` | 작업 삭제 |
| `php index.php clear` | 전체 삭제 |
| `php index.php help` | 도움말 |

## 데이터 저장

파일은 시스템 임시 디렉토리에 저장됩니다. 실제 서비스라면 이 자리를 MySQL/SQLite로 바꾸면 됩니다 — Repository 패턴 덕분에 다른 코드는 수정하지 않아도 됩니다.

## 확장 아이디어

- 마감일·우선순위·카테고리 필드 추가
- 진행 중인 작업만 필터
- 완료율 통계 (`list --stats`)
- `MySQLTaskRepository` 구현으로 교체 (DIP)
- 큐(chapter 34)로 알림 작업 분리

## 실행

```bash
php index.php
php index.php add "PHP 중급 복습"
php index.php list
php index.php done 1
php index.php remove 1
```

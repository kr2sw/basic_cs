# 40: 종합 프로젝트 — CLI Task Manager

지금까지 배운 내용을 종합하여 CLI 작업 관리 도구를 만듭니다.

## 프로젝트 소개

터미널에서 할 일(Task)을 추가·조회·완료·삭제하는 도구입니다. 데이터는 JSON 파일에 영구 저장됩니다.

## 구현 내용

| 기능 | 파일 |
|------|------|
| 파일 입출력 | `fs`로 `tasks.json` 읽기/쓰기 |
| 명령어 파싱 | `process.argv` 로 입력 파싱 |
| CRUD | 추가(add), 목록(list), 완료(done), 삭제(remove) |
| 필터링 | 상태/태그 기준 조회 |
| 통계 | 완료율 계산 |

## 사용법

```bash
node index.js help

node index.js add "TypeScript 공부" --tag study
node index.js add "보고서 작성" --tag work
node index.js add "운동하기"

node index.js list
node index.js list --status done
node index.js list --tag study

node index.js done 1
node index.js remove 3
node index.js stats
```

## 배운 개념의 활용

- **모듈 시스템**: 기능별 함수 분리 (`module.exports`)
- **파일 시스템**: JSON 파일 영구 저장 (비동기 fs)
- **에러 처리**: 입력 검증, 존재하지 않는 작업 처리
- **커맨드 패턴**: 명령어별 핸들러 분기

## 예제 실행

```bash
node index.js add "첫 번째 할 일"
node index.js list
node index.js done 1
node index.js list
```
